using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyFuelPrices.Interfaces;
using WeeklyFuelPrices.Models;

namespace WeeklyFuelPrices.Services
{
    public class GetFuelData : IGetFuelData
    {
        private readonly string fuelUri;
        private readonly HttpClient _httpClient;
        private readonly int dateRange;
        private readonly ILogger<GetFuelData> _logger;
        
        public GetFuelData (IConfiguration config, HttpClient httpClient, ILogger<GetFuelData> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            fuelUri = config.GetValue<string>("eiaUri");
            dateRange = config.GetValue<int>("dayrange");
        }
        public async Task<List<FuelPrices>> GetFuelPricesAsync()
        {
            List<FuelPrices> fuelPrices = new List<FuelPrices>();
            HttpResponseMessage response = new();

            try
            {
                response = await _httpClient.GetAsync(fuelUri);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error Downloading fuel information: {error}", ex.Message);
            }

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JObject results = JObject.Parse(json);
                    
                    var priceInfo = results["series"].Children()["data"].Children();

                    var dayRange = DateTime.Now.AddDays(-dateRange);
                    string pattern = "yyyyMMdd";
                    var provider = new CultureInfo("en-Us");

                    foreach (var item in priceInfo)
                    {
                        var itemDate = DateTime.ParseExact(item[0].ToString(), pattern, provider);
                        if (DateTime.Compare(itemDate, dayRange) >= 0)
                        {
                            fuelPrices.Add(new FuelPrices { Date_Price = item[0].ToString(), Price_Fuel = Convert.ToDouble(item[1].ToString()) });
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Error retreiving info: {ex.Message}");
                }
            }

            return fuelPrices;
        }
    }
}
