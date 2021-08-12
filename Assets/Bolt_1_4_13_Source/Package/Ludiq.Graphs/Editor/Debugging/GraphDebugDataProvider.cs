using System;
using System.Collections.Generic;

namespace Ludiq
{
	[InitializeAfterPlugins]
	public class GraphDebugDataProvider
	{
		static GraphDebugDataProvider()
		{
			GraphPointer.fetchRootDebugDataBinding = FetchRootDebugData;
			GraphPointer.setRootDebugDataBinding = SetRootDebugData;
		}

		private static IGraphDebugData FetchRootDebugData(IGraphRoot root)
		{
			if (!rootDatas.TryGetValue(root, out var rootData))
			{
				rootData = new GraphDebugData(root.childGraph);
				rootDatas.Add(root, rootData);
			}

			return rootData;
		}

		private static void SetRootDebugData(IGraphRoot root,IGraphDebugData data)
        {
			if(!rootDatas.ContainsKey(root))
            {
				rootDatas.Add(root, data);
            }
			else
            {
				rootDatas[root] = data;
            }
        }

		private static Dictionary<IGraphRoot, IGraphDebugData> rootDatas = new Dictionary<IGraphRoot, IGraphDebugData>();
	}
}
