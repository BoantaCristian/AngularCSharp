using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DbModels
{
    public class Receipt
    {
        [Key]
        public int Id { get; set; }
        public virtual User Client { get; set; }
        public virtual Payment Payment { get; set; }
        public DateTime PayDate { get; set; }
        public double AmountPayed { get; set; }
        public string ReceiptPaper { get; set; }
    }
}
