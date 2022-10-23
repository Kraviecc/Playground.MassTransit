using EventContracts;
using MassTransit;

namespace ProducerWorker
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;

		public Worker(ILogger<Worker> logger)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var busControl = Bus.Factory.CreateUsingRabbitMq(
				cfg =>
				{
					cfg.Send<ValueEntered>(
						x =>
						{
							x.UseRoutingKeyFormatter(context => "123");
						});
				});

			await busControl.StartAsync(stoppingToken);

			try
			{
				while (!stoppingToken.IsCancellationRequested)
				{
					_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
					await Task.Delay(1000, stoppingToken);
					await busControl.Publish<ValueEntered>(
						new
						{
							Value = 1
						},
						stoppingToken);
				}
			}
			finally
			{
				await busControl.StopAsync(stoppingToken);
			}
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