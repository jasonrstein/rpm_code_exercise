using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyFuelPrices.Models
{
    public class FuelPrices
    {
        public string? Date_Price { get; set; }
        public double? Price_Fuel { get; set; }
    }

}
