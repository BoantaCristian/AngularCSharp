using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DataTransferObjects
{
    public class RegisterDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CNP { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public int AssociationId { get; set; }
        public string RepresentativeId { get; set; }
        public int WaterProvider { get; set; }
        public int GasProvider { get; set; }
        public int ElectricityProvider { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
