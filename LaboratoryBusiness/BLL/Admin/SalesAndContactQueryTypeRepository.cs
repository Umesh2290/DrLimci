using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LaboratoryBusiness.BLL.Admin
{
    public class SalesAndContactQueryTypeRepository : LaboratoryBusiness.Repositories.Admin.ISalesAndContactQueryTypeRepository
    {
        private readonly LabSystemDBEntities _context;

        public SalesAndContactQueryTypeRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public SalesAndContactQueryTypeRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.SalesAndContactQueryType> GetAll()
        {
            var records = (from p in _context.Tbl_SalesAndContactQueryType.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.SalesAndContactQueryType
                           {
                             Description=p.Description,
                             SalesAndContactQueryTypeID=p.SalesAndContactQueryTypeID,
                             TypeName=p.TypeName

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.SalesAndContactQueryType GetByID(int SalesAndContactQueryTypeID)
        {
            var record = (from p in _context.Tbl_SalesAndContactQueryType.AsEnumerable()
                          where p.SalesAndContactQueryTypeID == SalesAndContactQueryTypeID
                          select new LaboratoryBusiness.POCO.Admin.SalesAndContactQueryType
                          {
                              Description = p.Description,
                              SalesAndContactQueryTypeID = p.SalesAndContactQueryTypeID,
                              TypeName = p.TypeName
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.SalesAndContactQueryType SalesAndContactQueryType)
        {
            _context.Tbl_SalesAndContactQueryType.Add(new Tbl_SalesAndContactQueryType()
            {
                //SystemUserID = p.SystemUserID,
                Description = SalesAndContactQueryType.Description,
            //    SalesAndContactQueryTypeID = SalesAndContactQueryType.SalesAndContactQueryTypeID,
                TypeName = SalesAndContactQueryType.TypeName


            });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.SalesAndContactQueryType SalesAndContactQueryType)
        {
            var record = _context.Tbl_SalesAndContactQueryType.Where(x => x.SalesAndContactQueryTypeID == SalesAndContactQueryType.SalesAndContactQueryTypeID).SingleOrDefault();
            if (record != null)
            {
                record.Description = SalesAndContactQueryType.Description;
                //    SalesAndContactQueryTypeID = SalesAndContactQueryType.SalesAndContactQueryTypeID,
                record.TypeName = SalesAndContactQueryType.TypeName;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int SalesAndContactQueryType)
        {
            var record = _context.Tbl_SalesAndContactQueryType.Where(x => x.SalesAndContactQueryTypeID == SalesAndContactQueryType).SingleOrDefault();
            _context.Tbl_SalesAndContactQueryType.Remove(record);
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
