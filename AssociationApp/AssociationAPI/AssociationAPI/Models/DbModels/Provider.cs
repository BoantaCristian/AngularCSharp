using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DbModels
{
    public class Provider
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Program { get; set; }
        public double HotWaterLiterPrice { get; set; }
        public double ColdWaterLiterPrice { get; set; }
        public double GasPrice { get; set; }
        public double ElectricityPrice { get; set; }
        public virtual ICollection<ClientProvider> WaterProvider { get; set; }
        public virtual ICollection<ClientProvider> GasProvider { get; set; }
        public virtual ICollection<ClientProvider> ElectricityProvider { get; set; }
    }
}
