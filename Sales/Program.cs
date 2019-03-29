
namespace Sales
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Logging;
    using NServiceBus.Sagas;
    using System;
    using System.Threading.Tasks;
    using Shared.Configuration;

    class Program
    {
        private static ILog log;

        public static IConfigurationRoot configuration;

        private static IEndpointInstance EndpointInstance { get; set; }

        public const string EndpointName = "Sales";

        static async Task Main()
        {
            // Create service collection
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            //Set console title
            Console.Title = EndpointName;

            //Configure logging
            LogManager.Use<DefaultFactory>()
                .Level(LogLevel.Info);
            log = LogManager.GetLogger<Program>();

            //Configure NSB Endpoint
            EndpointConfiguration endpointConfiguration = EndpointConfigurations.ConfigureNSB(serviceCollection, EndpointName);
            endpointConfiguration.AuditSagaStateChanges(serviceControlQueue: "particular.servicecontrol");

            //Start NSB Endpoint
            EndpointInstance = await Endpoint.Start(endpointConfiguration);

            //Support Graceful Shut Down of NSB Endpoint in PCF
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            log.Info("ENDPOINT READY");

            Console.Read();

        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (EndpointInstance != null)
            { EndpointInstance.Stop().ConfigureAwait(false); }

            log.Info("Exiting!");
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
        }

    }
}
