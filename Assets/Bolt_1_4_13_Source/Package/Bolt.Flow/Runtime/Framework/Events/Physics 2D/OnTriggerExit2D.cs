namespace Bolt
{
	/// <summary>
	/// Called when a collider exits the trigger.
	/// </summary>
	public sealed class OnTriggerExit2D : TriggerEvent2DUnit
	{
		protected override string hookName => EventHooks.OnTriggerExit2D;
	}
}