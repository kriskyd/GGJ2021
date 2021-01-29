using SA.ScriptableData;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Vector3Value playerPosition;
	[SerializeField] private Vector3Value aimPosition;
	[SerializeField] private float playerSpeed;
	[SerializeField] private Vector3Value playerFlatVelocity;

	[Header("Input")]

	[SerializeField] private PlayerInputSetup input;

	[Header("Animator")]
	
	[SerializeField] private Animator animator;
	[SerializeField] private string animForwardParam;
	[SerializeField] private string animRightParam;
	[SerializeField] private string animShootParam;

	private void Update()
	{
		GetInput();
		MovePlayer();
		GetAimPoint();
		RotatePlayer();
	}

	private void GetInput()
	{
		Vector3 move = new Vector3();
		
		move.z += Input.GetKey(input.ForwardKey) ? 0.5f : 0f;
		move.z += Input.GetKey(input.BackKey) ? -0.5f : 0f;
		move.x += Input.GetKey(input.RightKey) ? 0.5f : 0f;
		move.x += Input.GetKey(input.LeftKey) ? -0.5f : 0f;
		bool run = Input.GetKey(input.RunKey);
		move.z += (run && move.z > 0f) ? 0.5f : 0f;
		move = move.normalized * (run ? 1f : 0.5f);
		bool shoot = Input.GetKey(input.ShootKey);
		playerFlatVelocity.Value = move;
		animator.SetFloat(animForwardParam, move.z);
		animator.SetFloat(animRightParam, move.x);
		animator.SetFloat(animShootParam, shoot ? 1f : 0f);
	}

	private void MovePlayer()
	{
		Vector3 rotated = transform.rotation * playerFlatVelocity.Value * playerSpeed * Time.deltaTime;
		transform.Translate(rotated, Space.Self);
		playerPosition.Value = transform.position;
	}

	private void GetAimPoint()
	{
		Plane plane = new Plane(Vector3.up, 0f);

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
}
