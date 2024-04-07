using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Common.Email.Configuration;
using Common.Email.Interfaces;

namespace Common.Email;

public class EmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly Interfaces.ISmtpClient _smtpClient;

    public EmailService(IOptions<EmailSettings> emailSettings, Interfaces.ISmtpClient smtpClient)
    {
        _emailSettings = emailSettings.Value;
        _smtpClient = smtpClient;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string message)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Your Name", _emailSettings.SenderEmail));
        emailMessage.To.Add(new MailboxAddress("", recipientEmail));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };
        if (_emailSettings.SmtpServer == null)
        {
            throw new InvalidOperationException("SmtpServer is not set in EmailSettings");
        }
        if (_emailSettings.SenderEmail == null)
        {
            throw new InvalidOperationException("SenderEmail is not set in EmailSettings");
        }
        if (_emailSettings.SenderPassword == null)
        {
            throw new InvalidOperationException("SenderPassword is not set in EmailSettings");
        }

        await _smtpClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, false);
        await _smtpClient.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
        await _smtpClient.SendMailAsync(emailMessage);
        await _smtpClient.DisconnectAsync(true);
    }
}
