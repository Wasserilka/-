using Consul;
using System.Net;

namespace Lesson_1_2.Consul
{
    public class ConsulHostedService : IHostedService
    {
        private readonly IConsulClient ConsulClient;
        private CancellationTokenSource CancellationTokenSource;
        private string ServiceId;

        public ConsulHostedService(IConsulClient consulClient)
        {
            ConsulClient = consulClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var uri = new Uri("https://localhost:7032");
            ServiceId = $"{Dns.GetHostName()}-{uri.AbsoluteUri}";
            var registration = new AgentServiceRegistration()
            {
                ID = ServiceId,
                Name = "Service",
                Address = uri.Host,
                Port = uri.Port,
                Tags = new[] { "api" },
                Check = new AgentServiceCheck()
                {
                    HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}/healthz",
                    Timeout = TimeSpan.FromSeconds(2),
                    Interval = TimeSpan.FromSeconds(10)
                }
            };

            await ConsulClient.Agent.ServiceDeregister(registration.ID, CancellationTokenSource.Token);
            await ConsulClient.Agent.ServiceRegister(registration, CancellationTokenSource.Token);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            CancellationTokenSource.Cancel();
            await ConsulClient.Agent.ServiceDeregister(ServiceId, cancellationToken);
        }
    }
}
