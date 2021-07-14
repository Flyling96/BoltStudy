namespace Bolt
{
	/// <summary>
	/// Called when a mouse wheel scrolls.
	/// </summary>
	[UnitCategory("Events/GUI")]
	[UnitOrder(20)]
	public sealed class OnScroll : PointerEventUnit
	{
		protected override string hookName => EventHooks.OnScroll;
	}
}