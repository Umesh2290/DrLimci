using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class LabReportConfigurationRepository : LaboratoryBusiness.Repositories.User.ILabReportConfiguration
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_LabReportConfiguration labreport_entity = new Tbl_Cl_LabReportConfiguration();
        private LaboratoryBusiness.POCO.User.Cl_LabReportConfiguration reportconfig_poco = new POCO.User.Cl_LabReportConfiguration();

        public LabReportConfigurationRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public LabReportConfigurationRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }
        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_LabReportConfiguration> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_LabReportConfiguration.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_LabReportConfiguration
                           {
                               ConfigID = p.ConfigID,
                               LabID = p.LabID,
                               LabName = p.LabName,
                               LabAddress = p.LabAddress,
                               LabCompanyNumber = p.LabCompanyNumber,
                               LabUniqueCode = p.LabUniqueCode,
                               labEmail = p.labEmail,
                               LabImage = p.LabImage,
                               LabHeadOfficeAddress = p.LabHeadOfficeAddress,
                               CreatedBy = p.CreatedBy,
                               CreatedDate = p.CreatedDate,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate,
                               RegistrationNumber=p.RegistrationNumber,
                               VatRate=p.VatRate


                           });
            return records;

        }
        public void Insert(LaboratoryBusiness.POCO.User.Cl_LabReportConfiguration p)
        {
            Tbl_Cl_LabReportConfiguration inp = new Tbl_Cl_LabReportConfiguration()
            {

                ConfigID = p.ConfigID,
                LabID = p.LabID,
                LabName = p.LabName,
                LabAddress = p.LabAddress,
                LabCompanyNumber = p.LabCompanyNumber,
                LabUniqueCode = p.LabUniqueCode,
                labEmail = p.labEmail,
                LabImage = p.LabImage,
                LabHeadOfficeAddress = p.LabHeadOfficeAddress,
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                UpdatedBy = p.UpdatedBy,
                UpdatedDate = p.UpdatedDate,
                RegistrationNumber=p.RegistrationNumber,
                VatRate = p.VatRate

            };
            _context.Tbl_Cl_LabReportConfiguration.Add(inp);

            labreport_entity = inp;
            reportconfig_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_LabReportConfiguration p)
        {
            var record = _context.Tbl_Cl_LabReportConfiguration.Where(x => x.ConfigID == p.ConfigID).SingleOrDefault();
            if (record != null)
            {

               
                    record.ConfigID = p.ConfigID;
                    record.LabID = p.LabID;
                    record.LabName = p.LabName;
                    record.LabAddress = p.LabAddress;
                    record.LabCompanyNumber = p.LabCompanyNumber;
                    record.LabUniqueCode = p.LabUniqueCode;
                    record.labEmail = p.labEmail;
                    //record.LabImage = p.LabImage;
                    record.LabHeadOfficeAddress = p.LabHeadOfficeAddress;
                    record.CreatedBy = p.CreatedBy;
                    record.CreatedDate = p.CreatedDate;
                    record.UpdatedBy = p.UpdatedBy;
                    record.RegistrationNumber = p.RegistrationNumber;
                    record.VatRate = p.VatRate;
               

            }
            else
            {
                throw new Exception("Record not found");
            }
        }
        public void UpdateImage(LaboratoryBusiness.POCO.User.Cl_LabReportConfiguration p)
        {
            var record = _context.Tbl_Cl_LabReportConfiguration.Where(x => x.ConfigID == p.ConfigID).SingleOrDefault();
            if (record != null)
            {

              
                    record.ConfigID = p.ConfigID;
                    record.LabImage = p.LabImage;
                    record.CreatedBy = p.CreatedBy;
                    record.CreatedDate = p.CreatedDate;
                    record.UpdatedBy = p.UpdatedBy;
                    record.UpdatedDate = p.UpdatedDate;
               

            }
            else
            {
                throw new Exception("Record not found");
            }
        }
        public void Save()
        {
            _context.SaveChanges();
            reportconfig_poco.ConfigID = labreport_entity.ConfigID;
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
