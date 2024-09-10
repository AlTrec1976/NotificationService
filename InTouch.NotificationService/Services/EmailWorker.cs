using InTouch.Notification;
using InTouch.NotificationService.Entityes;
using InTouch.NotificationService.Services.Email;
using Microsoft.Extensions.Options;

namespace InTouch.NotificationService.Services
{
    public class EmailWorker : BaseKafkaWorker<NotificationServiceMessage>
    {
        private readonly ILogger<EmailWorker> _logger;
        private readonly IOptions<EmailGroup> _options;
        private readonly IOptions<SmtpConnect> _smtpoptions;
        private readonly IConfiguration _configuration;

        public EmailWorker(ILogger<EmailWorker> logger, IOptions<EmailGroup> options, IOptions<SmtpConnect> smtpoptions, IConfiguration configuration)
        : base(logger)
        {
            _logger = logger;
            _options = options;
            _smtpoptions = smtpoptions;
            _configuration = configuration;
        }

        protected override IDictionary<string, string> GetConfiguration()
        {
            return new Dictionary<string, string>
            {
                { "Host", _options.Value.Host },
                { "GroupId", _options.Value.Group },
                { "Topic", _options.Value.Topic },
            };
        }

        protected override async Task ProccesMessange(NotificationServiceMessage msg)
        {
            try
            {
                var loggerEmail = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<EmailSendService>();

                var emailService = new EmailSendService(loggerEmail, _smtpoptions);

                _logger.LogInformation($"{DateTime.Now} Сообщение для {msg.EmailTo} отправлено в рассылку");

                await emailService.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} Ошибка отправки сообщения в рассылку для {msg.EmailTo}");
                throw;
            }
        }
    }
}
