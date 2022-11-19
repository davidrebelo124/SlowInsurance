namespace EmailService
{
    public class EmailConfiguration
    {
        public string? From { get; set; }
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; } = "very.slow.insurance@gmail.com";
        public string Password { get; } = "fzpo cnfo bzur gjgq";
    }
}
