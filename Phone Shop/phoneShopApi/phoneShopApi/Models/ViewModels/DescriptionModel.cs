using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phoneShopApi.Models.ViewModels
{
    public class DescriptionModel
    {
        public string Dimensions { get; set; }
        public string Weight { get; set; }
        public string DisplayType { get; set; }
        public string Resolution { get; set; }
        public string OS { get; set; }
        public string MainCamera { get; set; }
        public string SelfieCamera { get; set; }
        public string Battery { get; set; }
    }
}
