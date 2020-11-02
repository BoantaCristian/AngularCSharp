using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccidentAPI.Models
{
    public class Severity
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Vehicle { get; set; }
        public virtual ICollection<Accident> Accidents { get; set; }
    }
}
