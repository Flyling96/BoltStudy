namespace Bolt
{
	/// <summary>
	/// Called when the pointer selects the GUI element.
	/// </summary>
	[UnitCategory("Events/GUI")]
	[UnitOrder(22)]
	public sealed class OnSelect : GenericGuiEventUnit
	{
		protected override string hookName => EventHooks.OnSelect;
	}
}