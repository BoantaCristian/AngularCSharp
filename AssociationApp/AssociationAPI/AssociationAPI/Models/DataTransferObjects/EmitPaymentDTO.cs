using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DataTransferObjects
{
    public class EmitPaymentDTO
    {
        public string ClientId { get; set; }
        public DateTime Date { get; set; }
        public string UtilitiesPaper { get; set; }
        public double HotWaterKitchenQuantity { get; set; }
        public double ColdWaterKitchenQuantity { get; set; }
        public double HotWaterBathroomQuantity { get; set; }
        public double ColdWaterBathroomQuantity { get; set; }
        public double GasQuantity { get; set; }
        public double ElectricityQuantity { get; set; }
    }
}
