namespace InTouch.NotificationService.Entityes
{
    public class SmtpConnect
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
