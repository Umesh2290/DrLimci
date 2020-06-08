using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public Nullable<int> HospitalId { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string PDFLink { get; set; }
    }
}
