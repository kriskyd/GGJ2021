using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SA.AnimatedValues
{
	public delegate void AnimValueChangedEvent<T>(T prevValue, T newValue);

	public abstract class AnimValueBase<T>
	{
		#region MEMBERS

		[SerializeField] private T target;
		[SerializeField] private float speed = 2f;
		[SerializeField] private AnimatorUpdateMode updateMode;

		private double lerpPosition = 1f;
		private Coroutine animationCoroutineHandler;

		#endregion

		#region PROPERTIES

		protected T Start
		{
			get;
			private set;
		}

		public T Target
		{
			get => target;
			set
			{
				if(!target.Equals(value))
				{
					BeginAnimating(this.Value, value, this.speed);
				}
			}
		}

		public T Value
		{
			get { return SmoothLerpValue >= 1f ? Target : GetValue(); }
			set { StopAnimationAndSetValue(value); }
		}

		public float Speed
		{
			get { return speed; }
			private set { speed = value; }
		}

		public AnimatorUpdateMode UpdateMode
		{
			get { return updateMode; }
			private set { updateMode = value; }
		}

		public bool IsAnimating
		{
			get;
			private set;
		} = false;

		protected float SmoothLerpValue
		{
			get
			{
				var v = 1.0 - lerpPosition;
				var result = 1.0 - v * v * v * v;
				return (float)result;
			}
		}

		#endregion

		#region EVENTS

		public UnityEvent OnValueChangedCallbackHandler;

		public event AnimValueChangedEvent<T> OnValueChangedEvent;

		#endregion

		#region METHODS

		/// <summary> Begins new value animation. </summary>
		/// <param name="startValue"> Animation start value. </param>
		/// <param name="targetValue"> Animation end value. </param>
		public void AnimateFromTo(T startValue, T targetValue)
		{
			StopAnimationAndSetValue(startValue);
			BeginAnimating(startValue, targetValue, speed);
		}

		/// <summary> Begins new value animation. </summary>
		/// <param name="startValue"> Animation start value. </param>
		/// <param name="targetValue"> Animation end value. </param>
		/// <param name="speed"> Custom animation speed. </param>
		public void AnimateFromTo(T startValue, T targetValue, float speed)
		{
			StopAnimationAndSetValue(startValue);
			BeginAnimating(startValue, targetValue, speed);
		}

		/// <summary> Begins new value animation. </summary>
		/// <param name="startValue"> Animation start value. </param>
		/// <param name="targetValue"> Animation end value. </param>
		/// <param name="speed"> Custom animation speed. </param>
		public void AnimateFromTo(T startValue, T targetValue, float speed, AnimatorUpdateMode updateMode)
		{
			UpdateMode = updateMode;
			StopAnimationAndSetValue(startValue);
			BeginAnimating(startValue, targetValue, speed);
		}

		/// <summary> Restarts animation with previously used start and target values and speed. </summary>
		public void RestartAnimation()
		{
			StopAnimationAndSetValue(Start);
			BeginAnimating(Start, Target, Speed);
		}

		/// <summary> Skips animation to <see cref="Target"/> value. </summary>
		public void SkipAnimation()
		{
			StopAnimationAndSetValue(Target);
		}

		/// <summary> Stops animation at current <see cref="Value"/> </summary>
		public void StopAnimation()
		{
			StopAnimationAndSetValue(Value);
		}

		protected AnimValueBase(T value)
		{
			Start = value;
			target = value;
			OnValueChangedCallbackHandler = new UnityEvent();
		}

		protected AnimValueBase(T value, UnityAction callback)
		{
			Start = value;
			target = value;
			OnValueChangedCallbackHandler = new UnityEvent();
			OnValueChangedCallbackHandler.AddListener(callback);
		}

		~AnimValueBase()
		{
			if(animationCoroutineHandler != null)
				AnimValueUtils.Coroutines.StopCoroutine(animationCoroutineHandler);
			animationCoroutineHandler = null;
		}

		protected abstract T GetValue();

		protected virtual bool AreEqual(T a, T b)
		{
			return a.Equals(b);
		}

		protected void StopAnimationAndSetValue(T newValue)
		{
			bool invoke = (!AreEqual(newValue, GetValue()) || lerpPosition < 1);

			T currentValue = GetValue();
			Start = target = newValue;
			lerpPosition = 1;
			IsAnimating = false;

			if(invoke)
			{
				OnValueChangedCallbackHandler?.Invoke();
				OnValueChangedEvent?.Invoke(currentValue, newValue);
			}
		}

		private void BeginAnimating(T newStart, T newTarget, float speed)
		{
			Speed = speed;
			Start = newStart;
			target = newTarget;
			IsAnimating = true;
			lerpPosition = 0;

			if(animationCoroutineHandler != null)
				AnimValueUtils.Coroutines.StopCoroutine(animationCoroutineHandler);
			animationCoroutineHandler = AnimValueUtils.Coroutines.StartCoroutine(AnimationRoutine());
		}

		private IEnumerator AnimationRoutine()
		{
			if(!IsAnimating)
			{
				yield return null;
			}

			while(SmoothLerpValue < 1f)
			{
				T prevValue = GetValue();

				UpdateLerpPosition();

				OnValueChangedCallbackHandler?.Invoke();
				OnValueChangedEvent?.Invoke(prevValue, GetValue());

				yield return null;
			}

			IsAnimating = false;
			AnimValueUtils.Coroutines.StopCoroutine(animationCoroutineHandler);
			animationCoroutineHandler = null;
		}

		private void UpdateLerpPosition()
		{
			float deltaTime = 0f;
			switch(updateMode)
			{
				case AnimatorUpdateMode.Normal:
					deltaTime = Time.deltaTime;
					break;
				case AnimatorUpdateMode.AnimatePhysics:
					deltaTime = Time.fixedDeltaTime;
					break;
				case AnimatorUpdateMode.UnscaledTime:
					deltaTime = Time.unscaledDeltaTime;
					break;
				default:
					break;
			}
			lerpPosition = Clamp(lerpPosition + (deltaTime * Speed), 0.0, 1.0);
		}

		private static T2 Clamp<T2>(T2 val, T2 min, T2 max) where T2 : IComparable<T2>
		{
			if(val.CompareTo(min) < 0) return min;
			if(val.CompareTo(max) > 0) return max;
			return val;
		}

		#endregion
	}
}