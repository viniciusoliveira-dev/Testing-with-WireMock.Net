using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaService
{
    class Program
    {
        public static async void ConsumerMessage()
        {
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "127.0.0.1:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumerBuilder = new ConsumerBuilder<Ignore, string>(conf).Build();
            {
                consumerBuilder.Subscribe("test-topic");

                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumer = consumerBuilder.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{consumer.Message}' at: '{consumer.TopicPartitionOffset}'.");
                        }
                        catch (ConsumeException ex)
                        {
                            Console.WriteLine($"Error occured: {ex.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
        }

        static async Task Main()
        {
            ConsumerMessage();
            var config = new ProducerConfig { BootstrapServers = "127.0.0.1:9092" };
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            {
                try
                {
                    var count = 0;
                    while (true)
                    {
                        var dr = await producer.ProduceAsync("test-topic",
                            new Message<Null, string> { Value = $"test: {count++}" });

                        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset} | {count}'");

                        Thread.Sleep(2000);
                    }
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
