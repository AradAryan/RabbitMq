using RabbitMq;
using MassTransit;
using System.Reflection.PortableExecutable;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<RabbitConsumer>();
            x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://127.0.0.1:5672"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ReceiveEndpoint("ReadyToSend", rc =>
                {
                    rc.Durable = true;
                    //rc.UseRetry(c => c.Immediate(1));
                    rc.PrefetchCount = 1000;
                    rc.UseRateLimit(10000, TimeSpan.FromMilliseconds(10000000));
                    //rc.ConfigureConsumer<RabbitConsumer>(provider);
                    rc.Batch<PersonModel>(c =>
                    {
                        c.MessageLimit = 1000;
                        c.TimeLimit = TimeSpan.FromMilliseconds(200);
                        c.ConcurrencyLimit = 5;
                        c.Consumer<RabbitConsumer, PersonModel>(provider);
                    });

                });

            }));

        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();