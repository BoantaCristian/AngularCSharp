using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteApi.Models.ViewModels
{
    public class ChildModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Parent { get; set; }
    }
}
