


namespace Producer
{

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Shared.Messages.Commands;
    using Shared.Configuration;
    using System.Collections.Generic;

    class Program
    {
        private static ILog log;

        public static IConfigurationRoot configuration;

        private static IEndpointInstance EndpointInstance { get; set; }

        public const string EndpointName = "Producer";

        private static Random rnd = new Random();

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

            //Start NSB Endpoint
            EndpointInstance = await Endpoint.Start(endpointConfiguration);

            //Support Graceful Shut Down of NSB Endpoint in PCF
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            log.Info("ENDPOINT READY");

            while (true)
            {

                var guid = Guid.NewGuid();
                log.Info($"Requesting to get data by id: {guid:N}");
                ProcessSale message = GenerateMessage(guid);

                //Send a message to a specific queue
                await EndpointInstance.Send("Sales", message);

                // Sleep as long as you need.
                Thread.Sleep(1000);
            }

        }

        private static ProcessSale GenerateMessage(Guid guid)
        {
            //create a message
            var sale = new ProcessSale
            {
                OrderId = guid.ToString(),
                OrderDate = DateTime.UtcNow
            };

            var list = new List<ProcessSale.OrderItem>();
            var itemCount = rnd.Next(1, 5);
            for(int i=0; i<itemCount; i++)
            {
                list.Add(new ProcessSale.OrderItem
                {
                    Price = rnd.NextDouble() + rnd.Next(0, 50),
                    Quantity = rnd.Next(1,10)
                });
            }

            sale.Items = list.ToArray();

            return sale;
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
               //.AddEnvironmentVariables()
               //.AddCloudFoundry()
               .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
        }

        private static string GetConnectionString()
        {
            string connection = configuration.GetConnectionString("");

            if (string.IsNullOrEmpty(connection))
                throw new Exception("Environment Variable 'AzureServiceBus_ConnectionString' not set");

            return connection;

        }

    }
}
