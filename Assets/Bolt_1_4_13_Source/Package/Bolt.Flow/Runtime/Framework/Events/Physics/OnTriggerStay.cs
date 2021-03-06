namespace Bolt
{
	/// <summary>
	/// Called once per frame for every collider that is touching the trigger.
	/// </summary>
	public sealed class OnTriggerStay : TriggerEventUnit
	{
		protected override string hookName => EventHooks.OnTriggerStay;
	}
}