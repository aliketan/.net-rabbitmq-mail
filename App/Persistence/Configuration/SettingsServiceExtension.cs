using App.Domain.Settings;

namespace App.Persistence.Configuration
{
    public static partial class ServiceExtension
    {
        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));
            services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));
        }

    }
}
