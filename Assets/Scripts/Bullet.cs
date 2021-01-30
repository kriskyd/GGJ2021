using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private Vector3 forward;

	private void FixedUpdate()
	{
		transform.Translate(forward * speed * Time.fixedDeltaTime, Space.Self);
	}
}
