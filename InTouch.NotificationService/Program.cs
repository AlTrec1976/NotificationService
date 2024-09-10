using InTouch.NotificationService.DI;
using InTouch.NotificationService.Entityes;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<SmtpConnect>(configuration.GetSection(nameof(SmtpConnect)));
builder.Services.Configure<EmailGroup>(configuration.GetSection(nameof(EmailGroup)));

builder.Services.ConfigureDependencies();

var app = builder.Build();

app.Run();
