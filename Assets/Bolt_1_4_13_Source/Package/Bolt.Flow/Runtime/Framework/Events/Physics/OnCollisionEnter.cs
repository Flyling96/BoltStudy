namespace Bolt
{
	/// <summary>
	/// Called when this collider / rigidbody has begun touching another rigidbody / collider.
	/// </summary>
	public sealed class OnCollisionEnter : CollisionEventUnit
	{
		protected override string hookName => EventHooks.OnCollisionEnter;
	}
}