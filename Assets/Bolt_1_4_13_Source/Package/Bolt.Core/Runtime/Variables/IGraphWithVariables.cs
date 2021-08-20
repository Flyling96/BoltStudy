using System.Collections.Generic;
using Ludiq;

namespace Bolt
{
	public interface IGraphWithVariables : IGraph
	{
		VariableDeclarations variables { get; }

		IEnumerable<string> GetDynamicVariableNames(VariableKind kind, GraphReference reference);
	}

	public interface IGraphWithFunctions
    {
		Extend.FunctionDeclarations functions { get; }
    }
}
