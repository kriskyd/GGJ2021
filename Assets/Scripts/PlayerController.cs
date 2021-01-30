using System;
using ObjectPooling;
using SA.ScriptableData;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
	[Header("Movement")]

	[SerializeField] private Vector3Value playerPosition;
	[SerializeField] private Vector3Value aimPosition;
	[SerializeField] private float walkSpeed = 4f;
	[SerializeField] private float runSpeed = 7f;
	[SerializeField] private float movementSpeedLerp = 0.2f;
	[SerializeField] private Vector3Value playerFlatVelocity;
	[SerializeField] private AudioClip walkSound;

	[Header("Shooting")]

	[SerializeField] private Transform bulletSpawnTransform;
	[SerializeField] private int maxBulletsPerSec = 9;

	[Header("Input")]

	[SerializeField] private PlayerInputSetup input;

	[Header("Animator")]

	[SerializeField] private Animator animator;
	[SerializeField] private string animForwardParam;
	[SerializeField] private string animRightParam;
	[SerializeField] private string animShootParam;

	private bool isRunning;
	private float movementSpeed;
	private bool isShooting;
	private float shootingSpeed;
	private float lastShootTime;
	private NavMeshAgent navMeshAgent;
	private AudioSource audioSource;
	private float lastWalkSoundTime;

	/// <summary>
	/// Shoot event that uses bullet transform as parameter.
	/// </summary>
	public event Action<Transform> OnShoot;

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		GetInput();
		MovePlayer();
		GetAimPoint();
		RotatePlayer();
		Shoot();
	}

	private void GetInput()
	{
		Vector3 move = new Vector3();
		move.z += Input.GetAxis("Vertical") * 0.5f;
		move.x += Input.GetAxis("Horizontal") * 0.5f;
		isShooting = Input.GetButton("Fire1");
		isRunning = Input.GetKey(input.RunKey);

		if(isRunning)
			move += new Vector3(move.x, 0f, move.z);

		move = move.normalized * (isRunning ? 1f : 0.5f);
		movementSpeed = Mathf.Lerp(movementSpeed, isRunning ? runSpeed : walkSpeed, movementSpeedLerp);

		playerFlatVelocity.Value = move * movementSpeed;
		move = transform.rotation * move;
		animator.SetFloat(animForwardParam, move.z);
		animator.SetFloat(animRightParam, move.x);
		animator.speed = isRunning ? runSpeed / walkSpeed : 1f;
	}

	private void MovePlayer()
	{
		Vector3 moveOffset = playerFlatVelocity.Value * Time.deltaTime;
		navMeshAgent.Move(moveOffset);
		navMeshAgent.SetDestination(transform.position + moveOffset);
		playerPosition.Value = transform.position;
	}

	private void GetAimPoint()
	{
		Plane plane = new Plane(Vector3.up, -transform.position.y);

		Ray ray = CameraController.Instance.Camera.ScreenPointToRay(Input.mousePosition);
		if(plane.Raycast(ray, out float distance))
		{
			aimPosition.Value = ray.GetPoint(distance);
		}
	}

	private void RotatePlayer()
	{
		transform.LookAt(aimPosition);
	}

	private void Shoot()
	{
		float shootAnimSpeed = isShooting ? 1f : 0f;
		float timeBetweenShots = 1f / maxBulletsPerSec;

		shootingSpeed = shootAnimSpeed * 1f / animator.speed;
		animator.SetFloat(animShootParam, shootingSpeed);

		if(!isShooting)
			return;

		if(Time.time - lastShootTime > timeBetweenShots)
		{
			lastShootTime = Time.time;
			CreateBullet();
		}
	}

	private void CreateBullet()
	{
		var bullet = ObjectPoolManager.Instance?.GetPool("bullet")?.Spawn(bulletSpawnTransform.position, bulletSpawnTransform.rotation);

		OnShoot?.Invoke(bullet.transform);
	}

	/// <summary>
	/// Used in animation event
	/// </summary>
	private void PlayWalkSound()
	{
		if(Time.time - lastWalkSoundTime > 0.1f)
		{
			audioSource.PlayOneShot(walkSound);
			lastWalkSoundTime = Time.time;
		}
	}

}
