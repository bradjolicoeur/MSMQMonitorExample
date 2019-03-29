using Microsoft.Extensions.DependencyInjection;
using System;
using NServiceBus;

namespace Shared.Configuration
{
    public static class EndpointConfigurations
    {

        public static EndpointConfiguration ConfigureNSB(ServiceCollection serviceCollection, string endpointName)
        {

            var endpointConfiguration = new EndpointConfiguration(endpointName);

            //heartbeat configuration, this is to identify when an endpoint is off or unresponsive
            endpointConfiguration.SendHeartbeatTo(
                    serviceControlQueue: "particular.servicecontrol",
                    frequency: TimeSpan.FromSeconds(15),
                    timeToLive: TimeSpan.FromSeconds(30));

            //metrics configuration for servicepulse 
            var metrics = endpointConfiguration.EnableMetrics();

            metrics.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: "particular.monitoring",
                interval: TimeSpan.FromMinutes(1),
                instanceId: "INSTANCE_ID_OPTIONAL");

            //performance counter configuration
            var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();
            performanceCounters.EnableSLAPerformanceCounters(TimeSpan.FromSeconds(10));

            //configuring audit queue and error queue
            endpointConfiguration.AuditProcessedMessagesTo("audit"); //copy of message after processing will go here for servicecontroller
            endpointConfiguration.SendFailedMessagesTo("error"); //after specified retries is hit, message will be moved here for alerting and recovery

            //forcing a quick failure to error queue for demo purposes, normally you would not want to disable retries
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                delayed =>
                {
                    delayed.NumberOfRetries(0);
                });

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.Transactions(TransportTransactionMode.TransactionScope);

            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            var conventions = endpointConfiguration.Conventions();
            NSBConventions.ConfigureConventions(conventions);

            endpointConfiguration.EnableInstallers(); //not for production

            endpointConfiguration.UseContainer<ServicesBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingServices(serviceCollection);
            });

            return endpointConfiguration;

        }
    }
}
