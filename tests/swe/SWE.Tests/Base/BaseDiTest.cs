using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWE.Configuration.Factory;
using SWE.Tests.Factories;
using SWE.Tests.Stubs;

namespace NSWE.Tests.Base
{
    public abstract class BaseDiTests
    {
        protected IServiceProvider ServiceProvider { get; }

        protected IServiceCollection ServiceCollection { get; }

        protected IConfiguration Configuration { get; }

        protected BaseDiTests()
            : this(ConfigurationFactory.Create())
        { }

        protected BaseDiTests(IConfiguration configuration)
        {
            Configuration = configuration;

            var logger = StubLoggerFactory.Create();

            var serviceCollection = new ServiceCollection()
                .AddSingleton(Configuration)
                .AddLogging(logging => logging.AddConsole())
                .AddSingleton(logger)
                .AddSingleton(typeof(ILogger<>), typeof(StubLogger<>))
                //.WithInfrastructureCommunicationSdkServices(Configuration)
                //.AddScoped<IClientRequest>(_ => new ClientRequest(DateTimeZoneUtilities.DutchDateTimeZone))
                //.WithRabbitMqStubConfiguration<IssueMessage>()
                ;

            //.AddSingleton<IMessageSender<IssueMessage>, StubMessageSender<IssueMessage>>();

            ServiceCollection = AddBaseServices(serviceCollection, Configuration);

            ServiceProvider = ServiceCollection
                .BuildServiceProvider();
        }

        protected virtual IServiceCollection AddBaseServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            return services;
        }
    }
}