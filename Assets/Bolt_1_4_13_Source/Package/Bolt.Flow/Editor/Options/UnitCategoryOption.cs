﻿using System.Linq;
using Ludiq;

namespace Bolt
{
	[FuzzyOption(typeof(UnitCategory))]
	public class UnitCategoryOption : FuzzyOption<UnitCategory>
	{
		public UnitCategoryOption(UnitCategory category)
		{
			value = category;
			label = category.name.Split('/').Last().Prettify();
			UnityAPI.Async(() => icon = BoltFlow.Icons.UnitCategory(category));
			parentOnly = true;
		}
	}
}