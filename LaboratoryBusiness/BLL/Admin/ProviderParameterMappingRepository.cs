using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class ProviderParameterMappingRepository : LaboratoryBusiness.Repositories.Admin.IProviderParameterMappingRepository
    {
        private readonly LabSystemDBEntities _context;

        public ProviderParameterMappingRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public ProviderParameterMappingRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.ProviderParameterMapping> GetAll()
        {
            var records = (from p in _context.Tbl_ProviderParameterMapping.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.ProviderParameterMapping
                           {
                               ProviderParameterMappingID = p.ProviderParameterMappingID,
                               CreatedBy =p.CreatedBy,
                             CreatedDate=p.CreatedDate,
                             ParameterID=p.ParameterID,
                             ProviderID=p.ProviderID,
                           
                             UpdatedBy=p.UpdatedBy,
                             UpdatedDate=p.UpdatedDate


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.ProviderParameterMapping GetByID(int ProviderParameterMappingID)
        {
            var record = (from p in _context.Tbl_ProviderParameterMapping.AsEnumerable()
                          where p.ProviderParameterMappingID == ProviderParameterMappingID
                          select new LaboratoryBusiness.POCO.Admin.ProviderParameterMapping
                          {
                              ProviderParameterMappingID = p.ProviderParameterMappingID,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              ParameterID = p.ParameterID,
                              ProviderID = p.ProviderID,

                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.ProviderParameterMapping p)
        {
            _context.Tbl_ProviderParameterMapping.Add(new Tbl_ProviderParameterMapping()
            {
               // ProviderParameterMappingID = p.ProviderParameterMappingID,
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                ParameterID = p.ParameterID,
                ProviderID = p.ProviderID,

                UpdatedBy = p.UpdatedBy,
                UpdatedDate = p.UpdatedDate


            });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.ProviderParameterMapping p)
        {
            var record = _context.Tbl_ProviderParameterMapping.Where(x => x.ProviderParameterMappingID == p.ProviderParameterMappingID).SingleOrDefault();
            if (record != null)
            {
                record.CreatedBy = p.CreatedBy;
                record.CreatedDate = p.CreatedDate;
                record.ParameterID = p.ParameterID;
                record.ProviderID = p.ProviderID;

                record.UpdatedBy = p.UpdatedBy;
                record.UpdatedDate = p.UpdatedDate;


            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int ProviderParameterMappingID)
        {
            var record = _context.Tbl_ProviderParameterMapping.Where(x => x.ProviderParameterMappingID == ProviderParameterMappingID).SingleOrDefault();
            _context.Tbl_ProviderParameterMapping.Remove(record);
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
