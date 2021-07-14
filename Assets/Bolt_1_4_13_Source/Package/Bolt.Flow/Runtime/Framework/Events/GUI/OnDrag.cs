﻿namespace Bolt
{
	/// <summary>
	/// When draging is occuring this will be called every time the cursor is moved.
	/// </summary>
	[UnitCategory("Events/GUI")]
	[UnitOrder(17)]
	public sealed class OnDrag : PointerEventUnit
	{
		protected override string hookName => EventHooks.OnDrag;
	}
}