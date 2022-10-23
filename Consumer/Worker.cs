using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> _logger;

	public Worker(ILogger<Worker> logger)
	{
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		ConnectionFactory factory = new ConnectionFactory();
		factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

		using (var connection = factory.CreateConnection())
		using (var channel = connection.CreateModel())
		{
			channel.ExchangeDeclare(
				"EventContracts:ValueEntered",
				ExchangeType.Fanout,
				true);

			channel.QueueDeclare(
				"myqueue",
				true,
				false,
				false,
				null);

			channel.QueueBind("myqueue", "EventContracts:ValueEntered", "123", null);

			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				await Task.Delay(1000, stoppingToken);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					var body = ea.Body.ToArray();
					var message = Encoding.UTF8.GetString(body);
					Console.WriteLine(" [x] Received {0}", message);
				};

				channel.BasicConsume(
					consumer,
					"myqueue",
					true);
			}
		}
	}
}