using EventContracts;
using MassTransit;

namespace ConsumerWorkerMassTransit
{
	public class ValueEnteredConsumer :
		IConsumer<ValueEntered>
	{
		public async Task Consume(ConsumeContext<ValueEntered> context)
		{
			Console.WriteLine($"Retrieved {context.Message.Value}");
		}
	}
}

namespace EventContracts
{
	public interface ValueEntered
	{
		string Value { get; }
	}
}