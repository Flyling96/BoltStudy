using System;
using System.Collections;
using System.Collections.Generic;
using Ludiq;

namespace Bolt.Extend
{
    [SerializationVersion("A")]
    public class FunctionDeclarations : IEnumerable<FunctionDeclaration>
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
            collection = new FunctionDeclarationCollection();
        }

        [Serialize, InspectorWide(true)]
        private FunctionDeclarationCollection collection;

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

		public FunctionDeclaration GetDeclaration(string variable)
		{
			if (collection.TryGetValue(variable, out var declaration))
			{
				return declaration;
			}

			throw new InvalidOperationException($"Variable not found: '{variable}'.");
		}

		public IEnumerator<FunctionDeclaration> GetEnumerator()
		{
			return collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)collection).GetEnumerator();
		}
	}
}
