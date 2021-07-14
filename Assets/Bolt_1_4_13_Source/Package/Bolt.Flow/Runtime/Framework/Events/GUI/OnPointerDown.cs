namespace Bolt
{
	/// <summary>
	/// Called when the pointer presses the GUI element.
	/// </summary>
	[UnitCategory("Events/GUI")]
	[UnitOrder(12)]
	public sealed class OnPointerDown : PointerEventUnit
	{
		protected override string hookName => EventHooks.OnPointerDown;
	}
}