namespace Bolt
{
	/// <summary>
	/// Called each frame where a collider on another object is touching this object's collider.
	/// </summary>
	public sealed class OnCollisionStay2D : CollisionEvent2DUnit
	{
		protected override string hookName => EventHooks.OnCollisionStay2D;
	}
}