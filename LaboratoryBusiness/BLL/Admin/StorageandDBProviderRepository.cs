using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class StorageandDBProviderRepository : LaboratoryBusiness.Repositories.Admin.IStorageandDBProviderRepository
    {
        private readonly LabSystemDBEntities _context;

        public StorageandDBProviderRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public StorageandDBProviderRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.StorageandDBProvider> GetAll()
        {

            var records = (from p in _context.Tbl_StorageandDBProvider.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.StorageandDBProvider
                           {
                               Description = p.Description,
                               ProviderName = p.ProviderName,
                               ProviderID = p.ProviderID
                               
                           });
            return records;
            
        }

        public LaboratoryBusiness.POCO.Admin.StorageandDBProvider GetByID(int StorageandDBProviderID)
        {
            var record = (from p in _context.Tbl_StorageandDBProvider.AsEnumerable()
                          where p.ProviderID == StorageandDBProviderID
                          select new LaboratoryBusiness.POCO.Admin.StorageandDBProvider
                          {
                              Description = p.Description,
                              ProviderName = p.ProviderName,
                              ProviderID = p.ProviderID
                          }).FirstOrDefault();

            return record;
           
           
        }

        public void Insert(LaboratoryBusiness.POCO.Admin.StorageandDBProvider storageanddbprovider)
        {
            _context.Tbl_StorageandDBProvider.Add(new Tbl_StorageandDBProvider()
                {
                    Description = storageanddbprovider.Description,
                     ProviderName = storageanddbprovider.ProviderName
                    
                });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.StorageandDBProvider storageanddbprovider)
        {
            var record = _context.Tbl_StorageandDBProvider.Where(x => x.ProviderID == storageanddbprovider.ProviderID).SingleOrDefault();
                if (record != null)
                {
                    record.Description = storageanddbprovider.Description;
                    record.ProviderName = storageanddbprovider.ProviderName;
                }
                else
                {
                    throw new Exception("Record not found");
                }
        }

        public void Delete(int StorageandDBProviderID)
        {
            var record = _context.Tbl_StorageandDBProvider.Where(x => x.ProviderID == StorageandDBProviderID).SingleOrDefault();
            _context.Tbl_StorageandDBProvider.Remove(record);
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
