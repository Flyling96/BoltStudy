namespace Ludiq
{
	public interface IGraphData 
	{
		bool TryGetElementData(IGraphElementWithData element, out IGraphElementData data);

		IGraphElementData CreateElementData(IGraphElementWithData element);

		void FreeElementData(IGraphElementWithData element);

		bool TryGetChildGraphData(IGraphParentElement element, out IGraphData data);

		IGraphData CreateChildGraphData(IGraphParentElement element);

		void FreeChildGraphData(IGraphParentElement element);

		bool TryGetFunctionGraphData(IGraphFunctionElement element, out IGraphData data);

		IGraphData CreateFunctionGraphData(IGraphFunctionElement element);

		void FreeFunctionGraphData(IGraphFunctionElement element);
	}
}
