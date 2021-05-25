using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DataTransferObjects
{
    public class AssociationDTO
    {
        public string Description { get; set; }
        public string Location { get; set; }
        public string Program { get; set; }
        public double WorkingCapital { get; set; } //fond de rulment
        public double Sanitation { get; set; }
        public double DayPenalty { get; set; }
    }
}
