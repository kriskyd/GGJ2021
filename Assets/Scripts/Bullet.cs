using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ObjectPooling.IRestorable
{
	[SerializeField] private GameObject hitParticlesPrefab;
	[SerializeField] private float speed;
	[SerializeField] private Vector3 forward;
	[SerializeField] private float lifeTime;
	[SerializeField] private int damage;
	[SerializeField] private Rigidbody rigidbody;
	[SerializeField] private AudioClip hitRockSound;
	[SerializeField] private AudioClip hitMetalSound;

	private float lifeTimeBackup;

	private void Awake()
	{
		lifeTimeBackup = lifeTime;
	}

	public void Restore()
	{
		lifeTime = lifeTimeBackup;
	}

	private void FixedUpdate()
	{
		rigidbody.MovePosition(transform.position + transform.up * speed * Time.fixedDeltaTime);
	}

	private void Update()
	{
		lifeTime -= Time.deltaTime;

		if(lifeTime <= 0f)
		{
			ObjectPooling.ObjectPoolManager.Instance.GetPool("bullet").Despawn(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag.Contains("Enviro"))
		{
			ObjectPooling.ObjectPoolManager.Instance.GetPool("bullet").Despawn(gameObject);
			var audio = ObjectPooling.ObjectPoolManager.Instance.GetPool("audio_source")?.Spawn(transform.position).GetComponent<AudioSource>();
			if(other.CompareTag("Enviro Rock"))
			{
				audio.PlayOneShot(hitRockSound);
				GameManager.Instance.Coroutines.StartWaitForSecondsRealtime(1f, () =>
				{
					audio.GetComponent<ObjectPooling.PooledObject>().Despawn();
				});
			}
			else if(other.CompareTag("Enviro Rock"))
			{
				audio.PlayOneShot(hitMetalSound);
				GameManager.Instance.Coroutines.StartWaitForSecondsRealtime(1f, () =>
				{
					audio.GetComponent<ObjectPooling.PooledObject>().Despawn();
				});
			}
		}
		else
		{
			if(other.TryGetComponent(out Enemy enemy))
			{
				enemy.GotHit(damage);
			}
			var particle = ObjectPooling.ObjectPoolManager.Instance.GetPool("bullet_splash")?.Spawn(transform.position);
			particle.transform.LookAt(transform.position + transform.up);
			ObjectPooling.ObjectPoolManager.Instance.GetPool("bullet").Despawn(gameObject);
		}
	}

}
