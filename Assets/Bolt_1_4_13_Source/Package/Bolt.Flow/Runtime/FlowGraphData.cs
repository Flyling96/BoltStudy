﻿using Ludiq;

namespace Bolt
{
	public sealed class FlowGraphData : GraphData<FlowGraph>, IGraphDataWithVariables, IGraphEventListenerData
	{
		public VariableDeclarations variables { get; }

		public IGraphFunctions functions { get; }

		public bool isListening { get; set; }

		public FlowGraphData(FlowGraph definition) : base(definition)
		{
			variables = definition.variables.CloneViaFakeSerialization();
			functions = definition.functions.CloneViaFakeSerialization();
		}
	}
}
