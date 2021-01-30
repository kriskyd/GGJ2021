using UnityEngine;

[CreateAssetMenu(fileName = "Player Movement Input", menuName = "Data/Player Movement Input", order = 'p')]
public class PlayerInputSetup : ScriptableObject
{
	[SerializeField] private KeyCode forwardKey;
	[SerializeField] private KeyCode backKey;
	[SerializeField] private KeyCode rightKey;
	[SerializeField] private KeyCode leftKey;
	[SerializeField] private KeyCode runKey;
	[SerializeField] private KeyCode shootKey;

	public KeyCode ForwardKey => forwardKey;
	public KeyCode BackKey => backKey;
	public KeyCode RightKey => rightKey;
	public KeyCode LeftKey => leftKey;
	public KeyCode RunKey => runKey;
	public KeyCode ShootKey => shootKey;
}
