namespace Bolt
{
	/// <summary>
	/// Called when a collider exits the trigger.
	/// </summary>
	public sealed class OnTriggerExit : TriggerEventUnit
	{
		protected override string hookName => EventHooks.OnTriggerExit;
	}
}