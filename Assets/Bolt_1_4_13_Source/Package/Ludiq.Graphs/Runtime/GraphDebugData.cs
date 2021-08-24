using System.Collections.Generic;

namespace Ludiq
{
	public class GraphDebugData : IGraphDebugData
	{
		public Dictionary<IGraphElementWithDebugData, IGraphElementDebugData> elementsData { get; } = new Dictionary<IGraphElementWithDebugData, IGraphElementDebugData>();

		public Dictionary<IGraphParentElement, IGraphDebugData> childrenGraphsData { get; } = new Dictionary<IGraphParentElement, IGraphDebugData>();

		public Dictionary<IGraphFunctionElement, IGraphDebugData> functionGraphsData { get; } = new Dictionary<IGraphFunctionElement, IGraphDebugData>();

		IEnumerable<IGraphElementDebugData> IGraphDebugData.elementsData => elementsData.Values;

		public GraphDebugData(IGraph definition) { }

		public IGraphElementDebugData GetOrCreateElementData(IGraphElementWithDebugData element)
		{
			if (!elementsData.TryGetValue(element, out var elementDebugData))
			{
				elementDebugData = element.CreateDebugData();
				elementsData.Add(element, elementDebugData);
			}

			return elementDebugData;
		}

		public IGraphDebugData GetOrCreateChildGraphData(IGraphParentElement element)
		{
			if (!childrenGraphsData.TryGetValue(element, out var data))
			{
				data = new GraphDebugData(element.childGraph);
				childrenGraphsData.Add(element, data);
			}

			return data;
		}

		public void CopyFrom(GraphDebugData data)
        {
			elementsData.Clear();
			childrenGraphsData.Clear();
			foreach (var element in data.elementsData)
            {
				elementsData.Add(element.Key, element.Value);
			}

			foreach (var child in data.childrenGraphsData)
			{
				childrenGraphsData.Add(child.Key, child.Value);
			}
		}

        public IGraphDebugData GetOrCreateFunctionGraphData(IGraphFunctionElement element)
        {
			if (!functionGraphsData.TryGetValue(element, out var data))
			{
				data = new GraphDebugData(element.childGraph);
				functionGraphsData.Add(element, data);
			}

			return data;
		}
    }
}
