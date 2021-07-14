namespace Bolt
{
	/// <summary>
	/// Called when the mouse is released over the same GUI element or collider as it was pressed.
	/// </summary>
	[UnitCategory("Events/Input")]
	public sealed class OnMouseUpAsButton : GameObjectEventUnit<EmptyEventArgs>
	{
		protected override string hookName => EventHooks.OnMouseUpAsButton;
	}
}