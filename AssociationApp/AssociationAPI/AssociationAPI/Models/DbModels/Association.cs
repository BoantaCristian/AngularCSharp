using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DbModels
{
    public class Association
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Program { get; set; }
        public double WorkingCapital { get; set; } //fond de rulment
        public double Sanitation { get; set; }
        public double DayPenalty { get; set; }
        public virtual ICollection<User> Representative { get; set; }
    }
}
