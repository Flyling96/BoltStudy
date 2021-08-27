using System;
using System.Text;
using Ludiq;
using UnityEngine;

namespace Bolt.Extend
{
	[SerializationVersion("A")]
    public abstract class FunctionDeclaration<TGraph, TMacro> : IGraphFunctionElement
		where TGraph : class, IGraph, new()
		where TMacro : Macro<TGraph>
	{
		[Serialize]
		public Guid guid { get; set; } = Guid.NewGuid();

		[Serialize]
        public string name { get; private set; }

		[DoNotSerialize]
		private GraphSource _source = GraphSource.Macro;

		[DoNotSerialize]
		private TMacro _macro;

		[DoNotSerialize]
		private TGraph _embed;

		Type graphType => typeof(TGraph);

		Type macroType => typeof(TMacro);

		[Serialize]
		public GraphSource source
		{
			get => _source;
			set
			{
				if (value == source)
				{
					return;
				}

				BeforeGraphChange();

				_source = value;

				AfterGraphChange();
			}
		}

		[Serialize]
		public TMacro macro
		{
			get => _macro;
			set
			{
				if (value == macro)
				{
					return;
				}

				BeforeGraphChange();

				_macro = value;

				AfterGraphChange();
			}
		}

		[Serialize]
		public TGraph embed
		{
			get => _embed;
			set
			{
				if (value == embed)
				{
					return;
				}

				BeforeGraphChange();

				_embed = value;

				AfterGraphChange();
			}
		}

		[DoNotSerialize]
		public TGraph graph
		{
			get
			{
				switch (source)
				{
					case GraphSource.Embed:
						return embed;

					case GraphSource.Macro:
						return macro?.graph;

					default:
						throw new UnexpectedEnumValueException<GraphSource>(source);
				}
			}
		}


		IMacro IGraphFunctionElement.macro
		{
			get => macro;
			set => macro = (TMacro)value;
		}

		IGraph IGraphFunctionElement.embed
		{
			get => embed;
			set => embed = (TGraph)value;
		}

		IGraph IGraphFunctionElement.graph => graph;

		Type IGraphFunctionElement.graphType => typeof(TGraph);

		Type IGraphFunctionElement.macroType => typeof(TMacro);

		[DoNotSerialize]
		public GameObject self { get; set; }

		public abstract IMachine machine { get; }

        public IGraph childGraph => graph;

        public bool isSerializationRoot => false;

        public UnityEngine.Object serializedObject => macro;

		[DoNotSerialize]
        public IGraphElement executeElement { get; set; }

        public FunctionDeclaration() { }

        public FunctionDeclaration(string name, GraphSource source = GraphSource.Embed) 
		{
			this.name = name;
			this.source = source;
			embed = (TGraph)DefaultGraph();
		}

		public event Action beforeGraphChange;

		public event Action afterGraphChange;

		private void BeforeGraphChange()
		{
			beforeGraphChange?.Invoke();
		}

		private void AfterGraphChange()
		{
			afterGraphChange?.Invoke();
		}

		public abstract IGraph DefaultGraph();

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(name);
			sb.Append("#");
			sb.Append(guid.ToString().Substring(0, 5));
			sb.Append("...");
			return sb.ToString();
		}
	}
}
