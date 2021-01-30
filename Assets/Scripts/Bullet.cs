using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ObjectPooling.IRestorable
{
	[SerializeField] private float speed;
	[SerializeField] private Vector3 forward;
	[SerializeField] private float lifeTime;
	[SerializeField] private int damage;

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
		transform.Translate(forward * speed * Time.fixedDeltaTime, Space.Self);
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
        if(other.TryGetComponent(out Enemy enemy))
        {
			enemy.GotHit(damage);
		}
		ObjectPooling.ObjectPoolManager.Instance.GetPool("bullet").Despawn(gameObject);
	}
}
