using WeeklyFuelPrices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeeklyFuelPrices.Models;


namespace WeeklyFuelPrices;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IFuelPricesRepo db;
    private readonly IGetFuelData api;
    private readonly int delay;

    public Worker(ILogger<Worker> logger, IFuelPricesRepo repository, IGetFuelData getFuelData, IConfiguration config)
    {
        _logger = logger;
        db = repository;
        api = getFuelData;
        delay = config.GetValue<int>("taskDelay");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Evaluating fuel data from EIA at: {time}", DateTimeOffset.Now);

            List<FuelPrices> apiData = await api.GetFuelPricesAsync();

            foreach (var price in apiData)
            {
                var dbValue = db.GetFuelPrice(price.Date_Price!);
                if (string.IsNullOrEmpty(dbValue))
                {
                    _logger.LogInformation("Record added to DB: {date} - {price}", price.Date_Price, price.Price_Fuel);
                    db.AddFuelPrice(price);
                }
                else
                {
                    _logger.LogInformation("Record already exists in DB: {date}", price.Date_Price);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(delay), stoppingToken);
        }
    }
}

