namespace Bolt
{
	/// <summary>
	/// Called when the renderer became visible by any camera.
	/// </summary>
	[UnitCategory("Events/Rendering")]
	public sealed class OnBecameVisible : GameObjectEventUnit<EmptyEventArgs>
	{
		protected override string hookName => EventHooks.OnBecameVisible;
	}
}