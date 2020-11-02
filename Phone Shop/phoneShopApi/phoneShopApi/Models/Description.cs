using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace phoneShopApi.Models
{
    public class Description
    {
        [Key]
        public int Id { get; set; }
        public string Dimensions { get; set; }
        public string Weight { get; set; }
        public string DisplayType { get; set; }
        public string Resolution { get; set; }
        public string OS { get; set; }
        public string MainCamera { get; set; }
        public string SelfieCamera { get; set; }
        public string Battery { get; set; }
        public virtual Telephone Telephone { get; set; }
    }
}
