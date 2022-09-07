using MassTransit;

namespace RabbitMq
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> Logger;
        public IBus Bus { get; set; }
        public Worker(ILogger<Worker> logger, IBus bus)
        {
            Bus = bus;
            Logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int counter = 0;
            Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                counter++;
                var endpoint = await Bus.GetSendEndpoint(new Uri("queue:ReadyToSend"));
                await endpoint.Send(new PersonModel
                {
                    Id = counter
                });

                //await Task.Delay(10000, stoppingToken);
            }
        }
    }
}