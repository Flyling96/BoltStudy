namespace Bolt
{
	/// <summary>
	/// Called when the submit button is pressed.
	/// </summary>
	[UnitCategory("Events/GUI")]
	[UnitOrder(24)]
	public sealed class OnSubmit : GenericGuiEventUnit
	{
		protected override string hookName => EventHooks.OnSubmit;
	}
}