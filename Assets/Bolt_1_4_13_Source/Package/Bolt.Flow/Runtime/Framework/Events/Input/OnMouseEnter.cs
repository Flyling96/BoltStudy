namespace Bolt
{
	/// <summary>
	/// Called when the mouse enters the GUI element or collider.
	/// </summary>
	[UnitCategory("Events/Input")]
	public sealed class OnMouseEnter : GameObjectEventUnit<EmptyEventArgs>
	{
		protected override string hookName => EventHooks.OnMouseEnter;
	}
}