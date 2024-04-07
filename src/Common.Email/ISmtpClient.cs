using System.Net.Mail;

namespace Common.Email;

public interface ISmtpClient
{
    Task ConnectAsync(string smtpServer, int smtpPort, bool useSsl);
    Task AuthenticateAsync(string username, string password);
    Task SendMailAsync(MimeKit.MimeMessage mailMessage);
    Task DisconnectAsync(bool quit);
}
