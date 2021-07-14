namespace Bolt
{
	/// <summary>
	/// Called once per frame for every collider / rigidbody that is touching rigidbody / collider.
	/// </summary>
	public sealed class OnCollisionStay : CollisionEventUnit
	{ 
		protected override string hookName => EventHooks.OnCollisionStay;
	}
}