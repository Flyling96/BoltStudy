using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Extend
{
    [InitializeAfterPlugins]
    public static class SubVariableHelper
    {
		static SubVariableHelper()
		{
			FlowCanvas.SubVariableOptions = SubVariableOptions;
		}

		private static IEnumerable<IUnitOption> SubVariableOptions(AbstractSubVariable subVariable)
        {
			if((subVariable.Mask & 1 ) != 0)
            {
                var unit = new SubFlowUnit(subVariable.gameObject.name);
                yield return new UnitOption<SubFlowUnit>(unit);
            }

            if ((subVariable.Mask & 1 << 1) != 0)
            {
                var unit = new SceneObjectUnit(subVariable.gameObject.name);
                yield return new UnitOption<SceneObjectUnit>(unit);
            }
        }
	}
}
