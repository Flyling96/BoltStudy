using System;
using System.Collections.Generic;
using UnityEngine;

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
		IGraphElement executeElement { get; set; }

		Type graphType { get; }
		Type macroType { get; }

		GameObject self { get; set; }
		IMachine machine { get; }
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
