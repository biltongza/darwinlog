using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Logging
{
    public static class DarwinLoggerExtensions
    {
        public static ILoggingBuilder AddDarwinLogger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DarwinLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<DarwinLoggingOptions, DarwinLoggerProvider>(builder.Services);
            return builder;
        }

        public static ILoggingBuilder AddDarwinLogger(this ILoggingBuilder builder, Action<DarwinLoggingOptions> configure)
        {
            if(configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddDarwinLogger();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}