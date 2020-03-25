using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class ProviderParamterRepository : LaboratoryBusiness.Repositories.Admin.IProviderParamterRepository
    {
        private readonly LabSystemDBEntities _context;

        public ProviderParamterRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public ProviderParamterRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.ProviderParamter> GetAll()
        {

            var records = (from p in _context.Tbl_ProviderParamter.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.ProviderParamter
                           {
                               ParameterID = p.ParameterID,
                               ParameterName = p.ParameterName,
                               Type = p.Type,
                               IsRequired = p.IsRequired,                             
                               CreatedBy=p.CreatedBy,
                               CreatedDate=p.CreatedDate,
                               UpdatedBy=p.UpdatedBy,
                               UpdatedDate=p.UpdatedDate
                           });
            return records;
            
        }

        public LaboratoryBusiness.POCO.Admin.ProviderParamter GetByID(int ParameterID)
        {
            var record = (from p in _context.Tbl_ProviderParamter.AsEnumerable()
                          where p.ParameterID==ParameterID
                          select new LaboratoryBusiness.POCO.Admin.ProviderParamter
                          {
                            //  ParameterID = p.ParameterID,
                              ParameterName = p.ParameterName,
                              Type = p.Type,
                              IsRequired = p.IsRequired,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate
                          }).FirstOrDefault();

            return record;
           
        }

        public void Insert(LaboratoryBusiness.POCO.Admin.ProviderParamter providerparameter)
        {
                _context.Tbl_ProviderParamter.Add(new Tbl_ProviderParamter()
                {
                   // ParameterID = p.ParameterID,
                    ParameterName = providerparameter.ParameterName,
                    Type = providerparameter.Type,
                    IsRequired = providerparameter.IsRequired,
                    CreatedBy = providerparameter.CreatedBy,
                    CreatedDate = providerparameter.CreatedDate,
                    UpdatedBy = providerparameter.UpdatedBy,
                    UpdatedDate = providerparameter.UpdatedDate

                });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.ProviderParamter providerparamter)
        {
            var record = _context.Tbl_ProviderParamter.Where(x => x.ParameterID == providerparamter.ParameterID).SingleOrDefault();
                if (record != null)
                {
                record.ParameterName = providerparamter.ParameterName;
                record.Type = providerparamter.Type;
                record.IsRequired = providerparamter.IsRequired;
                record.CreatedBy = providerparamter.CreatedBy;
                record.CreatedDate = providerparamter.CreatedDate;
                record.UpdatedBy = providerparamter.UpdatedBy;
                record.UpdatedDate = providerparamter.UpdatedDate;
                }
                else
                {
                    throw new Exception("Record not found");
                }
        }

        public void Delete(int ParameterID)
        {
            var record = _context.Tbl_ProviderParamter.Where(x => x.ParameterID == ParameterID).SingleOrDefault();
            _context.Tbl_ProviderParamter.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
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
