namespace Bolt
{
	/// <summary>
	/// Called when a collider enters the trigger.
	/// </summary>
	public sealed class OnTriggerEnter : TriggerEventUnit
	{
		protected override string hookName => EventHooks.OnTriggerEnter;
	}
}