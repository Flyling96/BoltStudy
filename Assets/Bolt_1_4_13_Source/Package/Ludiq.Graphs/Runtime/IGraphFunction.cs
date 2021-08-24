using System;
using System.Collections.Generic;

namespace Ludiq
{
	public interface IGraphFunctionElement:IGraphParent
	{
		Guid guid { get; }
		string name { get; }
		GraphSource source { get; }
		IGraph embed { get; set; }
		IMacro macro { get; set; }
		IGraph graph { get; }
		IGraphFunctions parent { get; set; }

		Type graphType { get; }
		Type macroType { get; }
	}

	public interface IGraphFunctions: IEnumerable<IGraphFunctionElement>
	{
		bool Editable { get; }
		void Clear();
		bool IsDefined(string key);
		IGraphFunctions GetDeclaration(string key);
		IGraphFunctionElement CreateFunction(string key);
	}


}
