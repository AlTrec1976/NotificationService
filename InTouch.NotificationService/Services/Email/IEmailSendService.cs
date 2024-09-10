using InTouch.Notification;

namespace InTouch.NotificationService.Services.Email
{
    public interface IEmailSendService
    {
        Task SendEmailAsync(NotificationServiceMessage message);
    }
}
