namespace Bolt
{
	/// <summary>
	/// Returns the unit length version of a 3D vector.
	/// </summary>
	[UnitCategory("Math/Vector 3")]
	[UnitTitle("Normalize")]
	public sealed class Vector3Normalize : Normalize<UnityEngine.Vector3>
	{
		public override UnityEngine.Vector3 Operation(UnityEngine.Vector3 input)
		{
			return UnityEngine.Vector3.Normalize(input);
		}
	}
}