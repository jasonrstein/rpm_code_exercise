using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WeeklyFuelPrices.Interfaces;
using WeeklyFuelPrices.Services;
using Microsoft.Extensions.Logging;
using WeeklyFuelPrices;

var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
IConfiguration config = new ConfigurationBuilder()
                            .AddEnvironmentVariables()
                            .AddUserSecrets<Program>()
                            .AddJsonFile("appsettings.json")
                            .AddJsonFile($"appsettings.{environment}.json", optional: true)
                            .Build();

var basAddress = config.GetValue<string>("eiaBase");

SocketsHttpHandler socketHandler = new()
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(15)
};

HttpClient httpClient = new HttpClient(socketHandler)
{
    BaseAddress = new Uri(basAddress)
};

IHost host =Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton(httpClient);
        services.AddSingleton(config);
        services.AddSingleton<IGetFuelData, GetFuelData>();
        services.AddSingleton<IFuelPricesRepo, FuelPricesRepository>();
    })
     .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        })
     .Build();