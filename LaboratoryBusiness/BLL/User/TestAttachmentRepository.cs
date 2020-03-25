using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class TestAttachmentRepository : LaboratoryBusiness.Repositories.User.ITestAttachmentRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_TestAttachment TestAttachment_entity = new Tbl_Cl_TestAttachment();
        private LaboratoryBusiness.POCO.User.Cl_TestAttachment TestAttachment_poco = new POCO.User.Cl_TestAttachment();

        public TestAttachmentRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public TestAttachmentRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestAttachment> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_TestAttachment.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_TestAttachment
                           {

                      AttachmentTypeID=p.AttachmentTypeID,
                      CreatedBy=p.CreatedBy,
                      CreatedDate=p.CreatedDate,
                      Extension=p.Extension,
                       Link=p.Link,
                       Name=p.Name,
                       TestAttachmentID=p.TestAttachmentID,
                       TestID=p.TestID




                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_TestAttachment GetByID(int TestAttachmentID)
        {
            var record = (from p in _context.Tbl_Cl_TestAttachment.AsEnumerable()
                          where p.TestAttachmentID == TestAttachmentID
                          select new LaboratoryBusiness.POCO.User.Cl_TestAttachment
                          {

                              AttachmentTypeID = p.AttachmentTypeID,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              Extension = p.Extension,
                              Link = p.Link,
                              Name = p.Name,
                              TestAttachmentID = p.TestAttachmentID,
                              TestID = p.TestID




                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_TestAttachment p)
        {
            Tbl_Cl_TestAttachment inp = new Tbl_Cl_TestAttachment()
            {
                AttachmentTypeID = p.AttachmentTypeID,
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                Extension = p.Extension,
                Link = p.Link,
                Name = p.Name,
                TestAttachmentID = p.TestAttachmentID,
                TestID = p.TestID


            };
            _context.Tbl_Cl_TestAttachment.Add(inp);

            TestAttachment_entity = inp;
            TestAttachment_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_TestAttachment p)
        {
            var record = _context.Tbl_Cl_TestAttachment.Where(x => x.TestAttachmentID == p.TestAttachmentID).SingleOrDefault();
            if (record != null)
            {       record.AttachmentTypeID = p.AttachmentTypeID;
                              record.CreatedBy = p.CreatedBy;
                              record.CreatedDate = p.CreatedDate;
                              record.Extension = p.Extension;
                              record.Link = p.Link;
                              record.Name = p.Name;
                              record.TestAttachmentID = p.TestAttachmentID;
                              record.TestID = p.TestID;


            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int TestAttachmentID)
        {
            var record = _context.Tbl_Cl_TestAttachment.Where(x => x.TestAttachmentID == TestAttachmentID).SingleOrDefault();
            _context.Tbl_Cl_TestAttachment.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            TestAttachment_poco.TestAttachmentID = TestAttachment_entity.TestAttachmentID;
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
