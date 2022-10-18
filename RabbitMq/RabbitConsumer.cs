using MassTransit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq
{
    public class RabbitConsumer : IConsumer<Batch<BatchModel>>
    {
        public IBus Bus { get; set; }
        public RabbitConsumer(IBus bus)
        {
            Bus = bus;
        }

        public async Task Consume(ConsumeContext<Batch<BatchModel>> context)
        {
            Console.WriteLine(context.Message.Count());
            var obj = context.Message.FirstOrDefault().Message;

            //BatchModel test = new() { Data = JsonConvert.SerializeObject(context.Message.ToList()) };
            //var sendEndPoint = await Bus.GetSendEndpoint(new("queue:ReadyToSend_Batch"));
            //await sendEndPoint.Send(test);
        }

    }
}
