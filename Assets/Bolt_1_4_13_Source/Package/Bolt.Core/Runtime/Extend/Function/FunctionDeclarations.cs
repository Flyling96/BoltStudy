using System;
using System.Collections;
using System.Collections.Generic;
using Ludiq;

namespace Bolt.Extend
{
    [SerializationVersion("A")]
    public abstract class FunctionDeclarations<TElement> : IGraphFunctions
		where TElement : IGraphFunctionElement
	{
        private bool m_Editable;
        public bool Editable
        {
            get
            {
                return m_Editable;
            }
        }

		public FunctionDeclarations(bool editable = true)
        {
            m_Editable = editable;
            collection = new FunctionDeclarationCollection<TElement>(this);
        }

        [Serialize, InspectorWide(true)]
        private FunctionDeclarationCollection<TElement> collection;

		public void Clear()
		{
			collection.Clear();
		}

		public void Add(TElement element)
        {
			collection.Add(element);
		}

		public bool IsDefined([InspectorVariableName(ActionDirection.Any)] string variable)
		{
			if (string.IsNullOrEmpty(variable))
			{
				throw new ArgumentException("No variable name specified.", nameof(variable));
			}

			return collection.Contains(variable);
		}

		public IGraphFunctions GetDeclaration(string variable)
		{
			if (collection.TryGetValue(variable, out var declaration))
			{
				return declaration as IGraphFunctions;
			}

			throw new InvalidOperationException($"Variable not found: '{variable}'.");
		}

		public IEnumerator<TElement> GetEnumerator()
		{
			return collection.GetEnumerator();
		}

		IEnumerator<IGraphFunctionElement> IEnumerable<IGraphFunctionElement>.GetEnumerator()
		{
			return GetEnumerator() as IEnumerator<IGraphFunctionElement>;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public abstract IGraphFunctionElement CreateFunction(string key);

    }
}
