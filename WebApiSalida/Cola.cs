using Azure.Messaging.ServiceBus;

namespace WebApiSalida
{
    public class Cola
    {
        static string connectionString = "Endpoint=sb://bussi410.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ETw+j1IH7B6Sqec9CRThtdcYVnZu6bZ9zvzYjrNBtsM=";
        static string queueName = "2_queues";
        static ServiceBusClient? client;
        static ServiceBusProcessor? processor;
        static string? msg = string.Empty;
        public static async Task<string> Receive()
        {
            // Create the client object that will be used to create sender and receiver objects
            client = new ServiceBusClient(connectionString);
            // create a processor that we can use to process the messages
            processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());
            try
            {
                processor.ProcessMessageAsync += async (ProcessMessageEventArgs args) =>
                {
                    string body = args.Message.Body.ToString();
                    Console.WriteLine($"Received: {body}");
                    if (!string.IsNullOrEmpty(body))
                    {
                        msg = body;
                    }

                    // complete the message. messages is deleted from the queue. 
                    await args.CompleteMessageAsync(args.Message);
                };
                // add handler to process any errors
                processor.ProcessErrorAsync += (ProcessErrorEventArgs args) =>
                {
                    //Console.WriteLine(args.Exception.ToString());
                    return Task.CompletedTask;
                };
                // start processing 
                await processor.StartProcessingAsync();

                //Console.ReadKey();
                await Task.Delay(3000);

                // stop processing 
                await processor.StopProcessingAsync();

                return msg;
            }
            finally
            {
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }


        }
    }
}
