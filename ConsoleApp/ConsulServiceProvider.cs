using Consul;

namespace ConsoleApp
{
    public class ConsulServiceProvider
    {
        public async Task<List<string>> GetServicesAsync()
        {
            var consuleClient = new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri("http://localhost:8500");
            });
            var queryResult = await consuleClient.Health.Service("Service", string.Empty);
            var result = new List<string>();
            foreach (var serviceEntry in queryResult.Response)
            {
                result.Add($"{serviceEntry.Service.Address}:{serviceEntry.Service.Port}, {serviceEntry.Checks[0].Name}:{serviceEntry.Checks[0].Status.Status}");
            }
            return result;
        }
    }
}
