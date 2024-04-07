using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Common.Email.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Email.Extensions;

public static class Extensions
{
    public static IHostBuilder AddEmailService(
        this IHostBuilder hostBuilder,
        IConfiguration configuration)
    {
        hostBuilder.ConfigureServices((context, services) =>
        {
            var emailSettings = new EmailSettings();
            configuration.GetSection("EmailSettings").Bind(emailSettings);

            if (string.IsNullOrEmpty(emailSettings.SmtpServer) ||
                emailSettings.SmtpPort == 0 ||
                string.IsNullOrEmpty(emailSettings.SenderEmail) ||
                string.IsNullOrEmpty(emailSettings.SenderPassword))
            {
                throw new ArgumentException("All EmailSettings must be provided");
            }

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<EmailService>();
        });

        return hostBuilder;
    }
}
