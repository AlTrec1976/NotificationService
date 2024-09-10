using InTouch.NotificationService.Services;
using InTouch.NotificationService.Services.Email;

namespace InTouch.NotificationService.DI
{
    public static class DIExtensions
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEmailSendService, EmailSendService>();
            //services.AddGrpc();

            services.AddHostedService<EmailWorker>();

            return services;
        }
    }
}
