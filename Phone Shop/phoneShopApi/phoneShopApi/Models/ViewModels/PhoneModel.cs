using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phoneShopApi.Models.ViewModels
{
    public class PhoneModel
    {
        public string Name { get; set; }
        public DateTime LaunchDate { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public int CompanyId { get; set; }
    }
}
