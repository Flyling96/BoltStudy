using System.Linq;
using Ludiq;

namespace Bolt.Extend
{
	[Descriptor(typeof(CustomSuperUnit))]
	public class CustomSuperUnitDescriptor : UnitDescriptor<CustomSuperUnit>
	{
		public CustomSuperUnitDescriptor(CustomSuperUnit unit) : base(unit) { }

		protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
		{
			base.DefinedPort(port, description);

			if (unit.graph == null)
			{
				return;
			}

			var flowGraph = unit.nest.graph as FlowGraph;
			if(flowGraph == null)
			{
				return;
			}

			var definition = flowGraph.validPortDefinitions.SingleOrDefault(d => d.key == port.key);

			if (definition != null)
			{
				description.label = definition.Label();
				description.summary = definition.summary;

				if (definition.hideLabel)
				{
					description.showLabel = false;
				}
			}
		}
	}
}