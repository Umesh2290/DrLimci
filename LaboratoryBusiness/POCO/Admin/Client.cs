using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.Admin
{
    public class Client
    {
        public int ClientDetailID { get; set; }
        public string CompanyName { get; set; }
        public string Subdomain { get; set; }
        public Nullable<int> PlanID { get; set; }
        public Nullable<int> PlanDuration { get; set; }
        public Nullable<decimal> TotalLicenseCost { get; set; }
        public string PriceUnit { get; set; }
        public Nullable<int> STandDBprovdiderID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public string InvoicePDFLink { get; set; }
        public Nullable<bool> IsDBCreated { get; set; }
        public Nullable<bool> IsDbError { get; set; }
        public string DBErrorDetail { get; set; }
        public Nullable<bool> IsStorageConfigured { get; set; }
        public Nullable<bool> IsStorageError { get; set; }
        public string StorageErrorDetail { get; set; }
        public Nullable<System.DateTime> LastActionOnDB { get; set; }
        public Nullable<System.DateTime> LastActionOnStorage { get; set; }
        public string LogoImgLink { get; set; }
        public string BackgroundImgLink { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
