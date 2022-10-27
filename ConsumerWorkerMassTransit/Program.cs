using ConsumerWorkerMassTransit;
using MassTransit;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
   .ConfigureServices(
		services =>
		{
			services.AddMassTransit(
				x =>
				{
					//x.AddConsumer<ValueEnteredConsumer>();
					x.AddConsumer<TestExchangeConsumer>();
					x.UsingRabbitMq(
						(context, cfg) =>
						{
							cfg.ReceiveEndpoint("testexchange222",
								e =>
								{
									e.ConfigureConsumeTopology = false;
									
									e.Bind("testexchange2",
										config =>
										{
											config.ExchangeType = ExchangeType.Direct;
											config.Durable = true;
											config.AutoDelete = false;
											config.RoutingKey = "urn:message:EventContracts:TestValue";
										});
									e.ConfigureConsumer<TestExchangeConsumer>(context);
								});
							
							cfg.ConfigureEndpoints(context);
						});
				});

			services.AddHostedService<Worker>();
		})
   .Build();

await host.RunAsync();