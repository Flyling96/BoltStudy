using Ludiq;

namespace Bolt.Extend
{
	[Descriptor(typeof(SubFlowMachine))]
	public sealed class SubFlowMachineDescriptor : MachineDescriptor<SubFlowMachine, MachineDescription>
	{
		public SubFlowMachineDescriptor(SubFlowMachine target) : base(target) { }
	}
}