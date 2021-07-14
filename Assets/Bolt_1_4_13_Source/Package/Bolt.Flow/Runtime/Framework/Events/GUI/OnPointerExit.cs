namespace Bolt
{
	/// <summary>
	/// Called when the pointer exits the GUI element.
	/// </summary>
	[UnitCategory("Events/GUI")]
	[UnitOrder(15)]
	public sealed class OnPointerExit : PointerEventUnit
	{
		protected override string hookName => EventHooks.OnPointerExit;
	}
}