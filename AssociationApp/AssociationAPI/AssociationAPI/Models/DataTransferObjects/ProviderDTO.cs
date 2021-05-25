using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DataTransferObjects
{
    public class ProviderDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Program { get; set; }
        public double HotWaterLiterPrice { get; set; }
        public double ColdWaterLiterPrice { get; set; }
        public double GasPrice { get; set; }
        public double ElectricityPrice { get; set; }
    }
}
