namespace Bolt
{
	/// <summary>
	/// Called every frame while the mouse is over the GUI element or collider.
	/// </summary>
	[UnitCategory("Events/Input")]
	public sealed class OnMouseOver : GameObjectEventUnit<EmptyEventArgs>
	{
		protected override string hookName => EventHooks.OnMouseOver;
	}
}