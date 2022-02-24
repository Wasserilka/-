using ConsoleApp;

var Client = new ConsulServiceProvider();
var result = await Client.GetServicesAsync();
foreach (var service in result)
{
    Console.WriteLine(service);
}

