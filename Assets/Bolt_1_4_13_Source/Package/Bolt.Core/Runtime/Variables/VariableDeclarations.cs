using System;
using System.Collections;
using System.Collections.Generic;
using Ludiq;

namespace Bolt
{
	[SerializationVersion("A")]
	[Serializable]
	public sealed class VariableDeclarations : IEnumerable<VariableDeclaration>, ISpecifiesCloner
	{
		private bool m_Editable;
		public bool Editable {
			get {
				return m_Editable;
			}
		}

		public VariableDeclarations(bool editable = true)
		{
			m_Editable = editable;
			collection = new VariableDeclarationCollection();
		}

		[Serialize, InspectorWide(true)]
		private VariableDeclarationCollection collection;

		public object this[[InspectorVariableName(ActionDirection.Any)] string variable]
		{
			get => Get(variable);
			set => Set(variable, value);
		}

		public void Set([InspectorVariableName(ActionDirection.Set)] string variable, object value)
		{
			if (string.IsNullOrEmpty(variable))
			{
				// Do nothing: safer for e.g. default event configurations,
				// where we don't specify a variable name. If we specified a default,
				// we might override a user value inadvertently.
				return;
			}

			if (collection.TryGetValue(variable, out var declaration))
			{
				declaration.value = value;
			}
			else
			{
				collection.Add(new VariableDeclaration(variable, value));
			}
		}

		public void Set(VariableDeclaration declaration)
        {
			collection.Add(declaration);
        }

		public void Set([InspectorVariableName(ActionDirection.Set)] string variable, object value,Type type)
		{
			if (string.IsNullOrEmpty(variable))
			{
				// Do nothing: safer for e.g. default event configurations,
				// where we don't specify a variable name. If we specified a default,
				// we might override a user value inadvertently.
				return;
			}

			if (collection.TryGetValue(variable, out var declaration))
			{
				declaration.value = value;
				declaration.type = type;
			}
			else
			{
				collection.Add(new VariableDeclaration(variable, value));
			}
		}

		public object Get([InspectorVariableName(ActionDirection.Get)] string variable)
		{
			if (string.IsNullOrEmpty(variable))
			{
				throw new ArgumentException("No variable name specified.", nameof(variable));
			}

			if (collection.TryGetValue(variable, out var declaration))
			{
				return declaration.value;
			}

			throw new InvalidOperationException($"Variable not found: '{variable}'.");
		}

		public T Get<T>([InspectorVariableName(ActionDirection.Get)] string variable)
		{
			return (T)Get(variable, typeof(T));
		}

		public object Get([InspectorVariableName(ActionDirection.Get)] string variable, Type expectedType)
		{
			return ConversionUtility.Convert(Get(variable), expectedType);
		}

		public void Remove(string key)
        {
			collection.Remove(key);
        }

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

		public VariableDeclaration GetDeclaration(string variable)
		{
			if (collection.TryGetValue(variable, out var declaration))
			{
				return declaration;
			}

			throw new InvalidOperationException($"Variable not found: '{variable}'.");
		}

		public IEnumerator<VariableDeclaration> GetEnumerator()
		{
			return collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)collection).GetEnumerator();
		}

		ICloner ISpecifiesCloner.cloner => VariableDeclarationsCloner.instance;
	}
}