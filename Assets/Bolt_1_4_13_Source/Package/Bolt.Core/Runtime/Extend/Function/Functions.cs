using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;

namespace Bolt.Extend
{
    public static class Functions
    {
		public static IGraphFunctions Graph(GraphPointer pointer)
		{
			Ensure.That(nameof(pointer)).IsNotNull(pointer);

			if (pointer.hasData)
			{
				return GraphInstance(pointer);
			}
			else
			{
				return GraphDefinition(pointer);
			}
		}

		public static IGraphFunctions GraphInstance(GraphPointer pointer)
		{
			return null;
			//return pointer.GetGraphData<IGraphDataWithVariables>().functions;
		}

		public static IGraphFunctions GraphDefinition(GraphPointer pointer)
		{
			return GraphDefinition((IGraphWithVariables)pointer.graph);
		}

		public static IGraphFunctions GraphDefinition(IGraphWithVariables graph)
		{
			return graph.functions;
		}
	}
}
