using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using System;

namespace Bolt.Extend
{
    [Widget(typeof(CustomSuperUnit))]
    public class CustomSuperUnitWidget : UnitWidget<CustomSuperUnit>
    {
		public CustomSuperUnitWidget(FlowCanvas canvas, CustomSuperUnit unit) : base(canvas, unit) { }

		protected override IEnumerable<DropdownOption> contextOptions
		{
			get
			{
				var childReference = reference.ChildReference(unit, false);

				if (childReference != null)
				{
					yield return new DropdownOption((Action)(() => window.reference = childReference), "Open");
					yield return new DropdownOption((Action)(() => GraphWindow.OpenTab(childReference)), "Open in new window");
				}

				foreach (var baseOption in base.contextOptions)
				{
					yield return baseOption;
				}
			}
		}

		protected override void OnDoubleClick()
		{
			if (unit.graph.zoom == 1)
			{
				var childReference = reference.ChildReference(unit, false);

				if (childReference != null)
				{
					if (e.ctrlOrCmd)
					{
						GraphWindow.OpenTab(childReference);
					}
					else
					{
						window.reference = childReference;
					}
				}

				e.Use();
			}
			else
			{
				base.OnDoubleClick();
			}
		}
	}
}
