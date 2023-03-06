﻿using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using WeeklyFuelPrices.Interfaces;
using WeeklyFuelPrices.Models;

namespace WeeklyFuelPrices.Services
{
    public class FuelPricesRepository : IFuelPricesRepo
    {
        private readonly string connectionString;
        private readonly ILogger<FuelPricesRepository> _logger;

        public FuelPricesRepository(IConfiguration config, ILogger<FuelPricesRepository> logger)
        {
            connectionString = config.GetConnectionString("default");
            _logger = logger;
        }

        public void AddFuelPrice(FuelPrices fuelPrices) 
        {
            using var connection = new MySqlConnection(connectionString);
            int results = connection.Execute(@"insert fuelprices(Date_Price, Price_Fuel) values (@Date_Price, @Price_Fuel) on duplicate key update Date_Price = @Date_Price", new { fuelPrices.Price_Fuel, fuelPrices.Date_Price });

            if (results == 0)
            {
                _logger.LogError("Could not upoad data {date} - {price}", fuelPrices.Price_Fuel, fuelPrices.Date_Price);
            }
            else
            {
                _logger.LogInformation("Uploaded data {date} - {price}", fuelPrices.Price_Fuel, fuelPrices.Date_Price);
            }
        }

        public string GetFuelPrice(string date)
        {
            using var connection = new MySqlConnection(connectionString);
            string dbvalue = connection.Query<string>("select Date_Price from fuelprices where Date_Price = @Date_Price", new { date }).FirstOrDefault("");
            return dbvalue;
        }
    }
}
