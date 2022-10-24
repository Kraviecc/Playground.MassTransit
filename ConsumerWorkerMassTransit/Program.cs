using ConsumerWorkerMassTransit;
using MassTransit;

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
							cfg.ReceiveEndpoint("testexchange",
								e =>
								{
									e.Bind("testexchange");
									e.ConfigureConsumer<TestExchangeConsumer>(context);
								});
							cfg.ConfigureEndpoints(context);
						});
				});

			services.AddHostedService<Worker>();
		})
   .Build();

await host.RunAsync();