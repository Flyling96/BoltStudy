namespace Bolt
{
	/// <summary>
	/// Called when the list of children of the transform of the game object has changed.
	/// </summary>
	[UnitCategory("Events/Hierarchy")]
	public sealed class OnTransformChildrenChanged : GameObjectEventUnit<EmptyEventArgs>
	{
		protected override string hookName => EventHooks.OnTransformChildrenChanged;
	}
}