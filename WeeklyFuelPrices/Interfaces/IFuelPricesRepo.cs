using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeeklyFuelPrices.Models;

namespace WeeklyFuelPrices.Interfaces
{
    public interface IFuelPricesRepo
    {
        void AddFuelPrice(FuelPrices fuelPricesRepo);
        string GetFuelPrice(string date);
    }
}
