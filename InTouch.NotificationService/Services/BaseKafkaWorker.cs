using Confluent.Kafka;
using System.Text.Json;

namespace InTouch.NotificationService.Services
{
    public abstract class BaseKafkaWorker<T> : BackgroundService
    {
        protected abstract IDictionary<string, string> GetConfiguration();
        protected abstract Task ProccesMessange(T msg);

        private readonly ILogger<BaseKafkaWorker<T>> _logger;

        public BaseKafkaWorker(ILogger<BaseKafkaWorker<T>> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(async () => await HandleMessageAsync(stoppingToken));
        }

        private async Task HandleMessageAsync(CancellationToken stoppingToken)
        {
            try
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = GetConfiguration()["Host"],
                    GroupId = GetConfiguration()["GroupId"],
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe(GetConfiguration()["Topic"]);

                    while (true)
                    {
                        var consumeResult = JsonSerializer.Deserialize<T>(consumer.Consume(stoppingToken).Message.Value);

                        await ProccesMessange(consumeResult);
                    }

                    consumer.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} {ex.Message}");
                throw;
            }
        }
    }
}
