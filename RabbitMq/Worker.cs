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

            //await Test();
            //await Test();
            //List<Task> tasks1 = new List<Task>();
            //tasks1.Add(Test());
            //tasks1.Add(Test());
            //await Task.WhenAll(tasks1);
            //await Task.Run( Test);

            var endpoint = await Bus.GetSendEndpoint(new Uri("queue:ReadyToSend"));
            var obj = new BatchModel
            {
                Data = "test"
            };
            //for (int i = 0; i < 10; i++)
            //{
            List<BatchModel> tasks = new();
            for (int j = 0; j < 10000; j++)
            {
                //tasks.Add(obj);
                //tasks.Add(endpoint.Send(obj));
                //await endpoint.Send(obj);
            }

            Console.WriteLine("test");
            //await Task.WhenAll(endpoint.SendBatch(tasks));
            //await endpoint.Send(tasks);
            //}


            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    //counter++;
            //    await endpoint.Send(new BatchModel
            //    {
            //        Data = "test"
            //    });

            //    //await Task.Delay(10000, stoppingToken);
            //}
        }

        public void Test()
        {
            Task.Delay(10000);
            //Console.WriteLine("test");
        }
    }

    public class BatchModel
    {
        public string Data { get; set; }
    }

}