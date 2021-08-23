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
		Extend.IFunctions functions { get; }
    }
}
