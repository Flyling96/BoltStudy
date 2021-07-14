namespace Bolt
{
	/// <summary>
	/// Called when the parent property of the transform of the game object has changed.
	/// </summary>
	[UnitCategory("Events/Hierarchy")]
	public sealed class OnTransformParentChanged : GameObjectEventUnit<EmptyEventArgs>
	{
		protected override string hookName => EventHooks.OnTransformParentChanged;
	}
}