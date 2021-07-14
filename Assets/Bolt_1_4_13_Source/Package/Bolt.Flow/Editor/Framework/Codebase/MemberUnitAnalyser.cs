using System;
using System.Collections.Generic;
using Ludiq;

namespace Bolt
{
	[Analyser(typeof(MemberUnit))]
	public class MemberUnitAnalyser : UnitAnalyser<MemberUnit>
	{
		public MemberUnitAnalyser(GraphReference reference, MemberUnit target) : base(reference, target) { }

		protected override IEnumerable<Warning> Warnings()
		{
			foreach (var baseWarning in base.Warnings())
			{
				yield return baseWarning;
			}

			if (target.member != null && target.member.isReflected)
			{
				var obsoleteAttribute = target.member.info.GetAttribute<ObsoleteAttribute>();

				if (obsoleteAttribute != null)
				{
					if (obsoleteAttribute.Message != null)
					{
						yield return Warning.Caution("Obsolete: " + obsoleteAttribute.Message);
					}
					else
					{
						yield return Warning.Caution("Member is obsolete.");
					}
				}
			}
		}
	}
}
