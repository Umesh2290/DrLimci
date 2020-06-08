using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class InvoiceRepository: LaboratoryBusiness.Repositories.User.IInvoiceRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_Invoice invoice_entity = new Tbl_Cl_Invoice();
        private LaboratoryBusiness.POCO.User.Invoice invoice_poco = new POCO.User.Invoice();

        public InvoiceRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public InvoiceRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }
        public IEnumerable<LaboratoryBusiness.POCO.User.Invoice> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_Invoice.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Invoice
                           {
                               InvoiceID = p.InvoiceID,
                               HospitalId = p.HospitalId,
                               Amount = p.Amount,
                               DueDate = p.DueDate,
                               InvoiceDate=p.InvoiceDate,
                               PDFLink = p.PDFLink


                           });
            return records;

        }
        public LaboratoryBusiness.POCO.User.Invoice GetByID(int InvoiceID)
        {
            var record = (from p in _context.Tbl_Cl_Invoice.AsEnumerable()
                          where p.InvoiceID == InvoiceID
                          select new LaboratoryBusiness.POCO.User.Invoice
                          {
                              InvoiceID = p.InvoiceID,
                              HospitalId = p.HospitalId,
                              Amount = p.Amount,
                              DueDate = p.DueDate,
                              InvoiceDate = p.InvoiceDate,
                              PDFLink = p.PDFLink
                          }).FirstOrDefault();
            return record;

        }
        public void Insert(LaboratoryBusiness.POCO.User.Invoice p)
        {
            Tbl_Cl_Invoice inp = new Tbl_Cl_Invoice()
            {

                InvoiceID = p.InvoiceID,
                HospitalId = p.HospitalId,
                Amount = p.Amount,
                DueDate = p.DueDate,
                InvoiceDate = p.InvoiceDate,
                PDFLink = p.PDFLink

            };
            _context.Tbl_Cl_Invoice.Add(inp);

            invoice_entity = inp;
            invoice_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Invoice p)
        {
            var record = _context.Tbl_Cl_Invoice.Where(x => x.InvoiceID == p.InvoiceID).SingleOrDefault();
            if (record != null)
            {


                record.InvoiceID = p.InvoiceID;
                record.HospitalId = p.HospitalId;
                record.Amount = p.Amount;
                record.DueDate = p.DueDate;
                record.InvoiceDate = p.InvoiceDate;
                record.PDFLink = p.PDFLink;
                         }
            else
            {
                throw new Exception("Record not found");
            }
        }
    
        public void Save()
        {
            _context.SaveChanges();
            invoice_poco.InvoiceID = invoice_entity.InvoiceID;
        }
        public void Delete(int InvoiceID)
        {
            var record = _context.Tbl_Cl_Invoice.Where(x => x.InvoiceID == InvoiceID).SingleOrDefault();
            _context.Tbl_Cl_Invoice.Remove(record);
        }


        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
