using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoneMontaneApi.Models.ViewModels
{
    public class MemberModel
    {
        public string Name { get; set; }
        public long Telephone { get; set; }
        public int Experience { get; set; }
        public int TeamId { get; set; }
    }
}
