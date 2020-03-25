using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class TestAttachmentTypeRepository : LaboratoryBusiness.Repositories.User.ITestAttachmentTypeRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_TestAttachmentType TestAttachment_entity = new Tbl_Cl_TestAttachmentType();
        private LaboratoryBusiness.POCO.User.Cl_TestAttachmentType TestAttachment_poco = new POCO.User.Cl_TestAttachmentType();

        public TestAttachmentTypeRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public TestAttachmentTypeRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestAttachmentType> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_TestAttachmentType.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_TestAttachmentType
                           {

                               Description=p.Description,
                               Name=p.Name,
                               TestAttachmentTypeID=p.TestAttachmentTypeID



                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_TestAttachmentType GetByID(int TestAttachmentTypeID)
        {
            var record = (from p in _context.Tbl_Cl_TestAttachmentType.AsEnumerable()
                          where p.TestAttachmentTypeID == TestAttachmentTypeID
                          select new LaboratoryBusiness.POCO.User.Cl_TestAttachmentType
                          {

                              Description = p.Description,
                              Name = p.Name,
                              TestAttachmentTypeID = p.TestAttachmentTypeID




                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_TestAttachmentType p)
        {
            Tbl_Cl_TestAttachmentType inp = new Tbl_Cl_TestAttachmentType()
            {
                Description = p.Description,
                Name = p.Name,
                TestAttachmentTypeID = p.TestAttachmentTypeID


            };
            _context.Tbl_Cl_TestAttachmentType.Add(inp);

            TestAttachment_entity = inp;
            TestAttachment_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_TestAttachmentType p)
        {
            var record = _context.Tbl_Cl_TestAttachmentType.Where(x => x.TestAttachmentTypeID == p.TestAttachmentTypeID).SingleOrDefault();
            if (record != null)
            {
                    record.Description = p.Description;
                              record.Name = p.Name;
                              record.TestAttachmentTypeID = p.TestAttachmentTypeID;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int TestAttachmentID)
        {
            var record = _context.Tbl_Cl_TestAttachmentType.Where(x => x.TestAttachmentTypeID == TestAttachmentID).SingleOrDefault();
            _context.Tbl_Cl_TestAttachmentType.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            TestAttachment_poco.TestAttachmentTypeID = TestAttachment_entity.TestAttachmentTypeID;
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
