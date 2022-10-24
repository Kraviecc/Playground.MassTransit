using EventContracts;
using MassTransit;

namespace ConsumerWorkerMassTransit
{
	public class TestExchangeConsumer :
		IConsumer<TestValue>
	{
		public async Task Consume(ConsumeContext<TestValue> context)
		{
			Console.WriteLine($"Retrieved (int) {context.Message.Value}");
		}
	}
}

namespace EventContracts
{
	public interface TestValue
	{
		int Value { get; }
	}
}