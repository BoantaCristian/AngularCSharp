using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccidentAPI.Models.DTO
{
    public class AccidentDTO
    {
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public string Location { get; set; }
        public string Photo { get; set; }
        public int Guilty { get; set; }
        public int Innocent { get; set; }
        public string AgentName { get; set; }
        public int Severity { get; set; }
    }
}
