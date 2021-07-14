using Ludiq;

namespace Bolt
{
	/// <summary>
	/// Called on the drag object when a drag finishes.
	/// </summary>
	[UnitCategory("Events/GUI")]
	[TypeIcon(typeof(OnDrag))]
	[UnitOrder(18)]
	public sealed class OnEndDrag : PointerEventUnit
	{
		protected override string hookName => EventHooks.OnEndDrag;
	}
}