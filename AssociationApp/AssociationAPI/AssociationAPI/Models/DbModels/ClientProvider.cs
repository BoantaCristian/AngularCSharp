using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DbModels
{
    public class ClientProvider
    {
        [Key]
        public int Id { get; set; }
        public virtual User Client { get; set; }
        public int WaterFk { get; set; }
        public int GasFk { get; set; }
        public int ElectricityFk { get; set; }
        [ForeignKey("WaterFk")]
        public virtual Provider WaterProvider { get; set; }
        [ForeignKey("GasFk")]
        public virtual Provider GasProvider { get; set; }
        [ForeignKey("ElectricityFk")]
        public virtual Provider ElectricityProvider { get; set; }
    }
}
