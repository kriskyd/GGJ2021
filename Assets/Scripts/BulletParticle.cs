using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour
{
	private void OnParticleSystemStopped()
	{
		GetComponent<ObjectPooling.PooledObject>().Despawn();
	}
}
