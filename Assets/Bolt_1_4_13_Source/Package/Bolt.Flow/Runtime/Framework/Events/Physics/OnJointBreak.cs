using Ludiq;

namespace Bolt
{
	/// <summary>
	/// Called when a joint attached to the same game object broke.
	/// </summary>
	[UnitCategory("Events/Physics")]
	public sealed class OnJointBreak : GameObjectEventUnit<float>
	{
		protected override string hookName => EventHooks.OnJointBreak;

		/// <summary>
		/// The force that was applied for this joint to break.
		/// </summary>
		[DoNotSerialize]
		public ValueOutput breakForce { get; private set; }

		protected override void Definition()
		{
			base.Definition();
			
			breakForce = ValueOutput<float>(nameof(breakForce));
		}

		protected override void AssignArguments(Flow flow, float breakForce)
		{
			flow.SetValue(this.breakForce, breakForce);
		}
	}
}