namespace Bolt
{
	/// <summary>
	/// Called when the mouse is not any longer over the GUI element or collider.
	/// </summary>
	[UnitCategory("Events/Input")]
	public sealed class OnMouseExit : GameObjectEventUnit<EmptyEventArgs>
	{
		protected override string hookName => EventHooks.OnMouseExit;
	}
}