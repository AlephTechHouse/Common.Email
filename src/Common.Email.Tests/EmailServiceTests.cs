using Common.Email.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;

namespace Common.Email.Tests;

public class EmailServiceTests
{
    [Fact]
    public async void SendEmail_WithValidParameters_SendsEmailSuccessfully()
    {
        // Arrange
        var emailSettings = new EmailSettings
        {
            SmtpServer = "localhost",
            SmtpPort = 1025,
            SenderEmail = "test@mailhog.local",
            SenderPassword = "your-password"
        };
        var expectedRecipient = "test@example.com";
        var expectedSubject = "Test subject";
        var expectedBody = "Test body";
        var mockOptions = new Mock<IOptions<EmailSettings>>();
        mockOptions.Setup(o => o.Value).Returns(emailSettings);

        var mockSmtpClient = new Mock<Interfaces.ISmtpClient>();
        mockSmtpClient.Setup(c => c.SendMailAsync(It.IsAny<MimeMessage>())).Returns(Task.CompletedTask);

        var emailService = new EmailService(mockOptions.Object, mockSmtpClient.Object);

        // Act
        await emailService.SendEmailAsync("test@example.com", "Test subject", "Test body");

        // Assert
        mockSmtpClient.Verify(c => c.SendMailAsync(It.Is<MimeMessage>(m =>
        m.To.Mailboxes.Any(mb => mb.Address == expectedRecipient) &&
        m.Subject == expectedSubject &&
        m.Body.ToString().Contains(expectedBody))), Times.Once);

    }

    [Fact]
    public async void SendEmail_WithInvalidRecipient_ThrowsArgumentException()
    {
        // Arrange
        var emailSettings = new EmailSettings
        {
            SmtpServer = "localhost",
            SmtpPort = 1025,
            SenderEmail = "test@mailhog.local",
            SenderPassword = "your-password"
        };
        var invalidRecipient = string.Empty; // Invalid recipient
        var expectedSubject = "Test subject";
        var expectedBody = "Test body";

        var mockOptions = new Mock<IOptions<EmailSettings>>();
        mockOptions.Setup(o => o.Value).Returns(emailSettings);

        var mockSmtpClient = new Mock<Interfaces.ISmtpClient>();
        mockSmtpClient.Setup(c => c.SendMailAsync(It.IsAny<MimeMessage>()));

        var emailService = new EmailService(mockOptions.Object, mockSmtpClient.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => emailService.SendEmailAsync(invalidRecipient, expectedSubject, expectedBody));
    }
}