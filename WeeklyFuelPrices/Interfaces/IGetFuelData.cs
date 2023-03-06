using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyFuelPrices.Models;

namespace WeeklyFuelPrices.Interfaces
{
    public interface IGetFuelData
    {
        Task<List<FuelPrices>> GetFuelPricesAsync();
    }
}
