using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DbModels
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public virtual User Client { get; set; }
        public DateTime Date { get; set; } //1-12
        public double DaysDelay { get; set; }
        public double Penalties { get; set; }
        public double TotalPaid { get; set; }
        public double TotalDueWithPenalties { get; set; }
        public double RemainingToPay { get; set; }
        public bool WorkingCapitalStatus { get; set; }
        public bool SanitationStatus { get; set; }
        public string UtilitiesPaper { get; set; }
        public bool PaymentStatus { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
