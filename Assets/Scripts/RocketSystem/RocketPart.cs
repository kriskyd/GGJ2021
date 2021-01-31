using SA.ScriptableData.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RocketSystem
{
	public class RocketPart : MonoBehaviour, IScreenMarkable
	{
		public event Action<RocketPart> PickedUp;
		public event Action<RocketPart> DroppedDown;

		public static List<RocketPart> AllRocketParts = new List<RocketPart>();

		[SerializeField]
		private RocketPartData rocketPartData;
		[SerializeField]
		private Outline outline;
		[SerializeField]
		private ListVector3Value rocketPartPositions;

		[SerializeField]
		private AudioSource pickupAudioSource;
		[SerializeField]
		private List<AudioClip> pickupAudioClips;

		private int idx = -1;
		public int Idx { get => idx; }

		private Vector3 lastPosition;

		public RocketPartData RocketPartData => rocketPartData;

		public bool ShowMarkerOnScreen => !gameObject.activeInHierarchy;

		public void PickUp()
		{
			PickedUp?.Invoke(this);
			pickupAudioSource.clip = pickupAudioClips.ElementAt(UnityEngine.Random.Range(0, pickupAudioClips.Count()));
			pickupAudioSource.Play();
		}

		public void DropDown()
		{
			DroppedDown?.Invoke(this);
		}

		private void Awake()
		{
			AllRocketParts.Add(this);
		}

		private void OnDestroy()
		{
			AllRocketParts.Remove(this);
		}

		private void Start()
		{
			rocketPartData.RocketPart = this;
			outline.OutlineColor = rocketPartData.OutlineColor;

			idx = rocketPartPositions.Value.Count;
			rocketPartPositions.Value.Add(transform.position);
		}

		private void Update()
		{
			if(lastPosition != transform.position)
			{
				rocketPartPositions[idx] = transform.position;
			}
		}
	}
}
