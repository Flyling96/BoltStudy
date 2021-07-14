namespace Bolt
{
	/// <summary>
	/// Called when an incoming collider makes contact with this object's collider.
	/// </summary>
	public sealed class OnCollisionEnter2D : CollisionEvent2DUnit
	{
		protected override string hookName => EventHooks.OnCollisionEnter2D;
	}
}