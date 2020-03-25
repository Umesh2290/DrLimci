using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LaboratoryBusiness.BLL.Admin
{
    public  class SalesAndContactQueryRepository : LaboratoryBusiness.Repositories.Admin.ISalesAndContactQueryRepository
    {
        private readonly LabSystemDBEntities _context;

        public SalesAndContactQueryRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public SalesAndContactQueryRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.SalesAndContactQuery> GetAll()
        {
            var records = (from p in _context.Tbl_SalesAndContactQuery.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.SalesAndContactQuery
                           {
                              ContactNo=p.ContactNo,
                              Description=p.Description,
                              Email=p.Email,
                              Name=p.Name,
                              NewActionBy=p.NewActionBy,
                              NewActionComments=p.NewActionComments,
                              NewActionDate=p.NewActionDate,
                              NewActionStatusID=p.NewActionStatusID,
                              PendingActionBy=p.PendingActionBy,
                              PendingActionComments=p.PendingActionComments,
                              PendingActionDate=p.PendingActionDate,
                              RequestCreatedDate=p.RequestCreatedDate,
                              SalesAndContactQueryID=p.SalesAndContactQueryID,
                              StatusID=p.StatusID,
                              Subject=p.Subject,
                              TypeID=p.TypeID

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.SalesAndContactQuery GetByID(int SalesAndContactQueryID)
        {
            var record = (from p in _context.Tbl_SalesAndContactQuery.AsEnumerable()
                          where p.SalesAndContactQueryID == SalesAndContactQueryID
                          select new LaboratoryBusiness.POCO.Admin.SalesAndContactQuery
                          {
                              SalesAndContactQueryID = p.SalesAndContactQueryID,
                              ContactNo = p.ContactNo,
                              Description = p.Description,
                              Email = p.Email,
                              Name = p.Name,
                              NewActionBy = p.NewActionBy,
                              NewActionComments = p.NewActionComments,
                              NewActionDate = p.NewActionDate,
                              NewActionStatusID = p.NewActionStatusID,
                              PendingActionBy = p.PendingActionBy,
                              PendingActionComments = p.PendingActionComments,
                              PendingActionDate = p.PendingActionDate,
                              RequestCreatedDate = p.RequestCreatedDate,
                              
                              StatusID = p.StatusID,
                              Subject = p.Subject,
                              TypeID = p.TypeID
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.SalesAndContactQuery p)
        {
            _context.Tbl_SalesAndContactQuery.Add(new Tbl_SalesAndContactQuery()
            {
                SalesAndContactQueryID = p.SalesAndContactQueryID,
                ContactNo = p.ContactNo,
                Description = p.Description,
                Email = p.Email,
                Name = p.Name,
                NewActionBy = p.NewActionBy,
                NewActionComments = p.NewActionComments,
                NewActionDate = p.NewActionDate,
                NewActionStatusID = p.NewActionStatusID,
                PendingActionBy = p.PendingActionBy,
                PendingActionComments = p.PendingActionComments,
                PendingActionDate = p.PendingActionDate,
                RequestCreatedDate = p.RequestCreatedDate,

                StatusID = p.StatusID,
                Subject = p.Subject,
                TypeID = p.TypeID


            });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.SalesAndContactQuery p)
        {
            var record = _context.Tbl_SalesAndContactQuery.Where(x => x.SalesAndContactQueryID == p.SalesAndContactQueryID).SingleOrDefault();
            if (record != null)
            {
                record.SalesAndContactQueryID = p.SalesAndContactQueryID;
                record.ContactNo = p.ContactNo;
                record.Description = p.Description;
                record.Email = p.Email;
                record.Name = p.Name;
                record.NewActionBy = p.NewActionBy;
                record.NewActionComments = p.NewActionComments;
                record.NewActionDate = p.NewActionDate;
                record.NewActionStatusID = p.NewActionStatusID;
                record.PendingActionBy = p.PendingActionBy;
                record.PendingActionComments = p.PendingActionComments;
                record.PendingActionDate = p.PendingActionDate;
                record.RequestCreatedDate = p.RequestCreatedDate;

                record.StatusID = p.StatusID;
                record.Subject = p.Subject;
                record.TypeID = p.TypeID;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int SalesAndContactQueryID)
        {
            var record = _context.Tbl_SalesAndContactQuery.Where(x => x.SalesAndContactQueryID == SalesAndContactQueryID).SingleOrDefault();
            _context.Tbl_SalesAndContactQuery.Remove(record);
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
