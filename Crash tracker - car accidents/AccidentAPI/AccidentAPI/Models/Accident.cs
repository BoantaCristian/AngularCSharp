using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccidentAPI.Models
{
    public class Accident
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public string Location { get; set; }
        public string Photo { get; set; }
        public bool Settled { get; set; }
        public  DateTime SettledDate { get; set; }
        //pers vinovate
        public int Guilty { get; set; }
        public int Innocent { get; set; }
        [ForeignKey("Guilty")]
        public virtual People GuiltyPeople { get; set; }
        [ForeignKey("Innocent")]
        public virtual People InnocentPeople { get; set; }
        public virtual User Agent { get; set; }
        public virtual Severity Severity { get; set; }
    }
}
