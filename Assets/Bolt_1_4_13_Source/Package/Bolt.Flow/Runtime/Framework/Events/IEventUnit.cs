namespace Bolt
{
	public interface IEventUnit : IUnit, IGraphEventListener
	{
		bool coroutine { get; }
	}
}
