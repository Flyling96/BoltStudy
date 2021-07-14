namespace Bolt
{
	/// <summary>
	/// Called when the pointer enters the GUI element.
	/// </summary>
	[UnitCategory("Events/GUI")]
	[UnitOrder(14)]
	public sealed class OnPointerEnter : PointerEventUnit
	{
		protected override string hookName => EventHooks.OnPointerEnter;
	}
}