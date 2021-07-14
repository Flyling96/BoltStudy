namespace Bolt
{
	/// <summary>
	/// Called when the cancel button is pressed.
	/// </summary>
	[UnitCategory("Events/GUI")]
	[UnitOrder(25)]
	public sealed class OnCancel : GenericGuiEventUnit
	{
		protected override string hookName => EventHooks.OnCancel;
	}
}