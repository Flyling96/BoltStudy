namespace Bolt
{
	/// <summary>
	/// Called when the user has released the mouse button.
	/// </summary>
	[UnitCategory("Events/Input")]
	public sealed class OnMouseUp : GameObjectEventUnit<EmptyEventArgs>
	{
		protected override string hookName => EventHooks.OnMouseUp;
	}
}