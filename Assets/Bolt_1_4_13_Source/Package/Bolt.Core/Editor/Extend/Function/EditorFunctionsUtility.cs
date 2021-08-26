using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bolt.Extend
{
	public static class EditorFunctionsUtility
	{
		public static bool isDraggingFunction => functionElement != null;

		public static IGraphFunctionElement functionElement => ((DragAndDrop.GetGenericData(DraggedListItem.TypeName) as DraggedListItem)?.item as IGraphFunctionElement);

	}

}
