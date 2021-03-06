namespace Bolt
{
	/// <summary>
	/// Called when the user has pressed the mouse button while over the GUI element or collider.
	/// </summary>
	[UnitCategory("Events/Input")]
	public sealed class OnMouseDown : GameObjectEventUnit<EmptyEventArgs>
	{
		protected override string hookName => EventHooks.OnMouseDown;
	}
}