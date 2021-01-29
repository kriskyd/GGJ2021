using UnityEngine;

namespace SA.ScriptableData
{
	[CreateAssetMenu(fileName = "MaterialValue", menuName = "Scriptable Data/Graphics/Material", order = 'm')]
	public class MaterialValue : GenericObjectValue<Material> { }
}