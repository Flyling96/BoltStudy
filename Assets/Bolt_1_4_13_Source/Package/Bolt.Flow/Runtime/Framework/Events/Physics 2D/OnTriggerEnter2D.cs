namespace Bolt
{
	/// <summary>
	/// Called when a collider enters the trigger.
	/// </summary>
	public sealed class OnTriggerEnter2D : TriggerEvent2DUnit
	{
		protected override string hookName => EventHooks.OnTriggerEnter2D;
	}
}