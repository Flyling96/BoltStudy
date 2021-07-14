namespace Bolt
{
	/// <summary>
	/// Called when the pointer releases the GUI element.
	/// </summary>
	[UnitCategory("Events/GUI")]
	[UnitOrder(13)]
	public sealed class OnPointerUp : PointerEventUnit
	{
		protected override string hookName => EventHooks.OnPointerUp;
	}
}