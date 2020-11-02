using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccidentAPI.Models
{
    public class People
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public int AccidentsInvolved { get; set; }
        public int AccidentsCommitted { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Accident> Guilty { get; set; }
        public virtual ICollection<Accident> Innocent    { get; set; }
    }
}
