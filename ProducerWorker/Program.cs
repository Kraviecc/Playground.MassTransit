using MassTransit;
using ProducerWorker;

IHost host = Host.CreateDefaultBuilder(args)
   .ConfigureServices(
		services =>
		{
			services.AddMassTransit(
				x =>
				{
					x.UsingRabbitMq(
						(context, cfg) =>
						{
							cfg.Host(
								"localhost",
								"/",
								h =>
								{
									h.Username("guest");
									h.Password("guest");
								});

							cfg.ConfigureEndpoints(context);
						});
				});

			services.AddHostedService<Worker>();
		})
   .Build();

await host.RunAsync();