using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DataTransferObjects
{
    public class PayDTO
    {
        public int PaymentId { get; set; }
        public string ClientUserName { get; set; }
        public double AmountPaid { get; set; }
        public bool WorkingCapital { get; set; }
        public bool Sanitation { get; set; }
        public string ReceiptPaper { get; set; }
    }
}
