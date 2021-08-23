using System;
using System.Collections;
using System.Collections.Generic;
using Ludiq;

namespace Bolt.Extend
{
	public interface IFunctions
    {
		bool Editable { get; }
		void Clear();
		bool IsDefined(string key);
		IFunctions GetDeclaration(string key);
		IFunctionElement CreateFunction(string key);
	}

    [SerializationVersion("A")]
    public class FunctionDeclarations<TGraph, TMacro> : IEnumerable<FunctionDeclaration<TGraph, TMacro>>, IFunctions
		where TGraph : class, IGraph, new()
		where TMacro : Macro<TGraph>
	{
        private bool m_Editable;
        public bool Editable
        {
            get
            {
                return m_Editable;
            }
        }

		public IFunctionElement CreateFunction(string key)
        {
			return new FunctionDeclaration<TGraph, TMacro>(key);
        }

		public FunctionDeclarations(bool editable = true)
        {
            m_Editable = editable;
            collection = new FunctionDeclarationCollection<FunctionDeclaration<TGraph,TMacro>>();
        }

        [Serialize, InspectorWide(true)]
        private FunctionDeclarationCollection<FunctionDeclaration<TGraph, TMacro>> collection;

		public void Clear()
		{
			collection.Clear();
		}

		public bool IsDefined([InspectorVariableName(ActionDirection.Any)] string variable)
		{
			if (string.IsNullOrEmpty(variable))
			{
				throw new ArgumentException("No variable name specified.", nameof(variable));
			}

			return collection.Contains(variable);
		}

		public IFunctions GetDeclaration(string variable)
		{
			if (collection.TryGetValue(variable, out var declaration))
			{
				return declaration as IFunctions;
			}

			throw new InvalidOperationException($"Variable not found: '{variable}'.");
		}

		public IEnumerator<FunctionDeclaration<TGraph, TMacro>> GetEnumerator()
		{
			return collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)collection).GetEnumerator();
		}
	}
}
