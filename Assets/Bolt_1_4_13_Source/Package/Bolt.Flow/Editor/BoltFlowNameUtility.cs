using System;
using Ludiq;

namespace Bolt
{
	public static class BoltFlowNameUtility
	{
		public static string UnitTitle(Type unitType, bool @short)
		{
			if (@short)
			{
				var shortTitle = unitType.GetAttribute<UnitShortTitleAttribute>()?.title;

				if (shortTitle != null)
				{
					return shortTitle;
				}
			}

			var title = unitType.GetAttribute<UnitTitleAttribute>()?.title;

			if (title != null)
			{
				return title;
			}

			return unitType.HumanName();
		}
	}
}