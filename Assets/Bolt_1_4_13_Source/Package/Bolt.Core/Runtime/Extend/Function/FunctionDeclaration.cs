using System;
using Ludiq;

namespace Bolt.Extend
{
	public interface IFunctionElement
	{
		string name { get; }
		GraphSource source { get; }
		IGraph embed { get; set; }
		IMacro macro { get; set; }
		IGraph graph { get; }

		Type graphType { get; }
		Type macroType { get; }
	}

	[SerializationVersion("A")]
    public sealed class FunctionDeclaration<TGraph, TMacro> : IFunctionElement
		where TGraph : class, IGraph, new()
		where TMacro : Macro<TGraph>
	{
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

		IMacro IFunctionElement.macro
		{
			get => macro;
			set => macro = (TMacro)value;
		}

		IGraph IFunctionElement.embed
		{
			get => embed;
			set => embed = (TGraph)value;
		}

		IGraph IFunctionElement.graph => graph;

		Type IFunctionElement.graphType => typeof(TGraph);

		Type IFunctionElement.macroType => typeof(TMacro);

		public FunctionDeclaration() { }

        public FunctionDeclaration(string name, GraphSource source = GraphSource.Embed) 
		{
			this.name = name;
			this.source = source;
		}

		private void BeforeGraphChange()
		{

		}

		private void AfterGraphChange()
		{

		}
	}
}
