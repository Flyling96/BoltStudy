using System;
using Ludiq;

namespace Bolt.Extend
{
    [SerializationVersion("A")]
    public sealed class FunctionDeclaration
    {
        [Serialize]
        public string name { get; private set; }

		[DoNotSerialize]
		private GraphSource _source = GraphSource.Macro;

		[DoNotSerialize]
		private IMacro _macro;

		[DoNotSerialize]
		private IGraph _embed;

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
		public IMacro macro
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
		public IGraph embed
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
