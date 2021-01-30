using UnityEngine;

namespace RocketSystem
{
	[CreateAssetMenu(fileName = "RocketPartData", menuName = "RocketSystem/RocketParts/RocketPartData")]
	public class RocketPartData : ScriptableObject
	{
		[SerializeField]
		private Color outlineColor;

		public Color OutlineColor => outlineColor;
		public RocketPart RocketPart { get; set; }
		public RocketPartSlot RocketPartSlot { get; set; }
	}
}
