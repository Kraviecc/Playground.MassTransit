using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ProducerRaw;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> _logger;

	public Worker(ILogger<Worker> logger)
	{
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		ConnectionFactory factory = new()
		{
			Uri = new Uri("amqp://guest:guest@localhost:5672")
		};

		using var connection = factory.CreateConnection();
		using var channel = connection.CreateModel();

		channel.ExchangeDeclare(
			"testexchange2",
			ExchangeType.Direct,
			true);

		while (!stoppingToken.IsCancellationRequested)
		{
			_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
			await Task.Delay(1000, stoppingToken);

			var json = @"{""message"":{""value"":1}, ""messageType"":[""urn:message:EventContracts:TestValue""]}";
			var body = Encoding.UTF8.GetBytes(json);

			channel.BasicPublish(
				"testexchange2",
				"urn:message:EventContracts:TestValue",
				body:body);
		}
	}
}