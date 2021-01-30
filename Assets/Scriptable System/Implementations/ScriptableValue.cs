using System;
using UnityEngine;

namespace SA.ScriptableData
{
	public abstract class ScriptableValue<T> : ScriptableObject
	{
		[SerializeField] private T value = default;

		protected T prevValue;

		private bool modified = false;
		private T valueOnInit;

		public virtual T Value
		{
			get => value;
			set
			{
				if(modified == false && Application.isPlaying)
				{
					valueOnInit = prevValue;
					modified = true;
#if UNITY_EDITOR && !SCRIPTABLE_SYSTEM_KEEP_EDITOR_CHANGES
					AttachExitingPlayModeEvent();
#endif
				}

				if(this.value == null || !this.value.Equals(value))
				{
					prevValue = this.value;
					this.value = value;
					valueChangedWithParams?.Invoke(prevValue, this.value);
					ValueChanged?.Invoke();
				}
			}
		}

		private event Action<T, T> valueChangedWithParams;

		public virtual event Action<T, T> ValueChangedWithParams
		{
			add => valueChangedWithParams += value;
			remove => valueChangedWithParams -= value;
		}

		public event Action ValueChanged;

#if UNITY_EDITOR
		public virtual void OnValueChanged_EDITOR()
		{
			ValueChanged?.Invoke();
		}

		public void OnValueChangedParams_EDITOR(T previousValue, T currentValue)
		{
			if(modified == false && UnityEditor.EditorApplication.isPlaying)
			{
				valueOnInit = previousValue;
				modified = true;
#if !SCRIPTABLE_SYSTEM_KEEP_EDITOR_CHANGES
				AttachExitingPlayModeEvent();
			}
#endif

			valueChangedWithParams?.Invoke(previousValue, currentValue);
		}
#endif

		public void Reset()
		{
			value = valueOnInit;
			prevValue = valueOnInit;
		}

#if UNITY_EDITOR && !SCRIPTABLE_SYSTEM_KEEP_EDITOR_CHANGES
		private void AttachExitingPlayModeEvent()
		{
			UnityEditor.EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;
		}

		private void EditorApplicationOnPlayModeStateChanged(UnityEditor.PlayModeStateChange playModeState)
		{
			if(playModeState == UnityEditor.PlayModeStateChange.ExitingPlayMode)
			{
				Reset();
				modified = false;

				UnityEditor.EditorApplication.playModeStateChanged -= EditorApplicationOnPlayModeStateChanged;
			}
		}
#endif

		public static implicit operator T(ScriptableValue<T> scriptableValue) => scriptableValue.Value;
	}
}