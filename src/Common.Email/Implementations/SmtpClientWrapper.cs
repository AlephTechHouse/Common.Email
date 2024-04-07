using MailKit.Net.Smtp;
using MimeKit;

namespace Common.Email.Impremetations;

public class SmtpClientWrapper : Interfaces.ISmtpClient
{
    private readonly SmtpClient _smtpClient;

    public SmtpClientWrapper()
    {
        _smtpClient = new SmtpClient();
    }

    public Task ConnectAsync(string smtpServer, int smtpPort, bool useSsl)
    {
        return _smtpClient.ConnectAsync(smtpServer, smtpPort, useSsl);
    }

    public Task AuthenticateAsync(string username, string password)
    {
        return _smtpClient.AuthenticateAsync(username, password);
    }

    public Task SendMailAsync(MimeMessage mailMessage)
    {
        return _smtpClient.SendAsync(mailMessage);
    }

    public Task DisconnectAsync(bool quit)
    {
        return _smtpClient.DisconnectAsync(quit);
    }
}

