using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phoneShopApi.Models.ViewModels
{
    public class UpdatePhoneModel
    {
        public string NewName { get; set; }
        public string NewCompany { get; set; }
        public DateTime NewLaunchDate { get; set; }
        public double NewPrice { get; set; }
    }
}
