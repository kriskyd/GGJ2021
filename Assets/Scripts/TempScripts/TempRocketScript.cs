using SA.ScriptableData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RocketSystem
{
	public class TempRocketScript : MonoBehaviour, IDamageable
	{
		[SerializeField]
		private RocketValue rocketValue;
		[SerializeField]
		private Vector3Value rocketPosition;
		[SerializeField]
		private AudioSource repairAudioSource;
		[SerializeField]
		private List<AudioClip> repairAudioClips;
		[SerializeField]
		private AudioSource fallAudioSource;
		[SerializeField]
		private List<AudioClip> fallAudioClips;

		private Vector3 lastPosition;
		private int currentRocketHealth = 10;
		private List<RocketPart> mountedRocketParts = new List<RocketPart>();

		public int MountedPartsCount => mountedRocketParts.Count;

		private void Awake()
		{
			rocketValue.Value = this;
			rocketPosition.Value = transform.position;
		}

		public void MountRocketPart(RocketPart rocketPart)
		{
			rocketPart.gameObject.SetActive(false);
			rocketPart.RocketPartData.RocketPartSlot.PlaceRocketPart(rocketPart);

			mountedRocketParts.Add(rocketPart);
			repairAudioSource.clip = repairAudioClips.ElementAt(UnityEngine.Random.Range(0, repairAudioClips.Count()));
			repairAudioSource.Play();
		}

		public void DismountRocketPart(Enemy enemy, RocketPart rocketPart)
		{
			rocketPart.gameObject.SetActive(true);
			rocketPart.RocketPartData.RocketPartSlot.RemoveRocketPart(rocketPart);

			enemy.TryPickPart(rocketPart);

			mountedRocketParts.Remove(rocketPart);
			repairAudioSource.clip = fallAudioClips.ElementAt(UnityEngine.Random.Range(0, repairAudioClips.Count()));
			fallAudioSource.Play();
		}

		public void GotHit(IDamageDealer damageDealer, int damage)
		{
			Enemy enemy = damageDealer as Enemy;

			if(enemy == null)
			{
				return;
			}

			currentRocketHealth--;
			if(currentRocketHealth <= 0)
			{
				if(mountedRocketParts.Count > 0)
				{
					DismountRocketPart(enemy, mountedRocketParts.ElementAt(Random.Range(0, mountedRocketParts.Count)));
				}
				currentRocketHealth += 10;
			}
		}
	}
}
