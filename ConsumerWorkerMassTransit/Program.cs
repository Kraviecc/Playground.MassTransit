using ConsumerWorkerMassTransit;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
   .ConfigureServices(
		services =>
		{
			services.AddMassTransit(
				x =>
				{
					x.AddConsumer<ValueEnteredConsumer>();
					x.UsingRabbitMq(
						(context, cfg) =>
						{
							cfg.ConfigureEndpoints(context);
						});
				});

			services.AddHostedService<Worker>();
		})
   .Build();

await host.RunAsync();