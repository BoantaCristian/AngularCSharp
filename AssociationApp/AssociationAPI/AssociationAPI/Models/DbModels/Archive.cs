using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DbModels
{
    public class Archive
    {
        [Key]
        public int Id { get; set; }
        public virtual User Client { get; set; }
        public int Month { get; set; } //1-12
        public double HotWaterKitchenQuantity { get; set; }
        public double ColdWaterKitchenQuantity { get; set; }
        public double HotWaterBathroomQuantity { get; set; }
        public double ColdWaterBathroomQuantity { get; set; }
        public double GasQuantity { get; set; }
        public double ElectricityQuantity { get; set; }
        public double HotWaterKitchenDue { get; set; }
        public double ColdWaterKitchenDue { get; set; }
        public double HotWaterBathroomDue { get; set; }
        public double ColdWaterBathroomDue { get; set; }
        public double GasDue { get; set; }
        public double ElectricityDue { get; set; }
        public double TotalPayment { get; set; }
    }
}
