using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phoneShopApi.Models.ViewModels
{
    public class HistoricModel
    {
        public string UserName { get; set; }
        public string Order { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string Contact { get; set; }
    }
}
