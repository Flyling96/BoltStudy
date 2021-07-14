namespace Bolt
{
	/// <summary>
	/// Called once per frame for every collider that is touching the trigger.
	/// </summary>
	public sealed class OnTriggerStay2D : TriggerEvent2DUnit
	{
		protected override string hookName => EventHooks.OnTriggerStay2D;
	}
}