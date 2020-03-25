using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LaboratoryBusiness.BLL.Admin
{
    public class ClientRepository : LaboratoryBusiness.Repositories.Admin.IClientRepository
    {
        private readonly LabSystemDBEntities _context;
        private Tbl_Client _client_entity = new Tbl_Client();
        private LaboratoryBusiness.POCO.Admin.Client _client_poco = new POCO.Admin.Client();

        public ClientRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public ClientRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.Client> GetAll()
        {

            var records = (from p in _context.Tbl_Client.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.Client
                           {
                               ClientDetailID = p.ClientDetailID,
                               CompanyName = p.CompanyName,
                               CreatedBy = p.CreatedBy,
                               CreatedDate = p.CreatedDate,
                               DBErrorDetail = p.DBErrorDetail,
                               InvoiceID = p.InvoiceID,
                               InvoicePDFLink = p.InvoicePDFLink,
                               IsDBCreated = p.IsDBCreated,
                               IsDbError = p.IsDbError,
                               IsStorageConfigured = p.IsStorageConfigured,
                               IsStorageError = p.IsStorageError,
                               LastActionOnDB = p.LastActionOnDB,
                               LastActionOnStorage = p.LastActionOnStorage,
                               PlanDuration = p.PlanDuration,
                               PlanID = p.PlanID,
                               PriceUnit = p.PriceUnit,
                               STandDBprovdiderID = p.STandDBprovdiderID,
                               StorageErrorDetail = p.StorageErrorDetail,
                               Subdomain = p.Subdomain,
                               TotalLicenseCost = p.TotalLicenseCost,
                               LogoImgLink = p.LogoImgLink,
                               BackgroundImgLink = p.BackgroundImgLink,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate



                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.Client GetByID(int ClientDetailID)
        {
            var record = (from p in _context.Tbl_Client.AsEnumerable()
                          where p.ClientDetailID == ClientDetailID
                          select new LaboratoryBusiness.POCO.Admin.Client
                          {
                              ClientDetailID = p.ClientDetailID,
                              CompanyName = p.CompanyName,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              DBErrorDetail = p.DBErrorDetail,
                              InvoiceID = p.InvoiceID,
                              InvoicePDFLink = p.InvoicePDFLink,
                              IsDBCreated = p.IsDBCreated,
                              IsDbError = p.IsDbError,
                              IsStorageConfigured = p.IsStorageConfigured,
                              IsStorageError = p.IsStorageError,
                              LastActionOnDB = p.LastActionOnDB,
                              LastActionOnStorage = p.LastActionOnStorage,
                              PlanDuration = p.PlanDuration,
                              PlanID = p.PlanID,
                              PriceUnit = p.PriceUnit,
                              STandDBprovdiderID = p.STandDBprovdiderID,
                              StorageErrorDetail = p.StorageErrorDetail,
                              Subdomain = p.Subdomain,
                              TotalLicenseCost = p.TotalLicenseCost,
                              LogoImgLink = p.LogoImgLink,
                              BackgroundImgLink = p.BackgroundImgLink,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate
                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.Client Client)
        {
            Tbl_Client cl = new Tbl_Client()
            {
                // ClientDetailID = Client.ClientDetailID,
                CompanyName = Client.CompanyName,
                CreatedBy = Client.CreatedBy,
                CreatedDate = Client.CreatedDate,
                DBErrorDetail = Client.DBErrorDetail,
                InvoiceID = Client.InvoiceID,
                InvoicePDFLink = Client.InvoicePDFLink,
                IsDBCreated = Client.IsDBCreated,
                IsDbError = Client.IsDbError,
                IsStorageConfigured = Client.IsStorageConfigured,
                IsStorageError = Client.IsStorageError,
                LastActionOnDB = Client.LastActionOnDB,
                LastActionOnStorage = Client.LastActionOnStorage,
                PlanDuration = Client.PlanDuration,
                PlanID = Client.PlanID,
                PriceUnit = Client.PriceUnit,
                STandDBprovdiderID = Client.STandDBprovdiderID,
                StorageErrorDetail = Client.StorageErrorDetail,
                Subdomain = Client.Subdomain,
                TotalLicenseCost = Client.TotalLicenseCost,
                UpdatedBy = Client.UpdatedBy,
                UpdatedDate = Client.UpdatedDate,
                LogoImgLink = Client.LogoImgLink,
                BackgroundImgLink = Client.BackgroundImgLink
            };

            _context.Tbl_Client.Add(cl);

            _client_entity = cl;
            _client_poco = Client;
        }

        public void Update(LaboratoryBusiness.POCO.Admin.Client Client)
        {
            var record = _context.Tbl_Client.Where(x => x.ClientDetailID == Client.ClientDetailID).SingleOrDefault();
            if (record != null)
            {
                record.CompanyName = Client.CompanyName;
                record.CreatedBy = Client.CreatedBy;
                record.CreatedDate = Client.CreatedDate;
                record.DBErrorDetail = Client.DBErrorDetail;
                record.InvoiceID = Client.InvoiceID;
                record.InvoicePDFLink = Client.InvoicePDFLink;
                record.IsDBCreated = Client.IsDBCreated;
                record.IsDbError = Client.IsDbError;
                record.IsStorageConfigured = Client.IsStorageConfigured;
                record.IsStorageError = Client.IsStorageError;
                record.LastActionOnDB = Client.LastActionOnDB;
                record.LastActionOnStorage = Client.LastActionOnStorage;
                record.PlanDuration = Client.PlanDuration;
                record.PlanID = Client.PlanID;
                record.PriceUnit = Client.PriceUnit;
                record.STandDBprovdiderID = Client.STandDBprovdiderID;
                record.StorageErrorDetail = Client.StorageErrorDetail;
                record.Subdomain = Client.Subdomain;
                record.TotalLicenseCost = Client.TotalLicenseCost;
                record.UpdatedBy = Client.UpdatedBy;
                record.UpdatedDate = Client.UpdatedDate;
                record.LogoImgLink = Client.LogoImgLink;
                record.BackgroundImgLink = Client.BackgroundImgLink;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int ClientDetailID)
        {
            var record = _context.Tbl_Client.Where(x => x.ClientDetailID == ClientDetailID).SingleOrDefault();
            _context.Tbl_Client.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            _client_poco.ClientDetailID = _client_entity.ClientDetailID;
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
