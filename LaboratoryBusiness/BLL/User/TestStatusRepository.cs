using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class TestStatusRepositories : LaboratoryBusiness.Repositories.User.ITestStatusRepositories
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_TestStatus TestAttachment_entity = new Tbl_Cl_TestStatus();
        private LaboratoryBusiness.POCO.User.Cl_TestStatus TestAttachment_poco = new POCO.User.Cl_TestStatus();

        public TestStatusRepositories()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public TestStatusRepositories(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestStatus> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_TestStatus.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_TestStatus
                           {
                               Description=p.Description,
                               StatusName=p.StatusName,
                               TestStatusID=p.TestStatusID



                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_TestStatus GetByID(int TestStatusID)
        {
            var record = (from p in _context.Tbl_Cl_TestStatus.AsEnumerable()
                          where p.TestStatusID == TestStatusID
                          select new LaboratoryBusiness.POCO.User.Cl_TestStatus
                          {
                              Description = p.Description,
                              StatusName = p.StatusName,
                              TestStatusID = p.TestStatusID




                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_TestStatus p)
        {
            Tbl_Cl_TestStatus inp = new Tbl_Cl_TestStatus()
            {


                Description = p.Description,
                StatusName = p.StatusName,
                TestStatusID = p.TestStatusID

            };
            _context.Tbl_Cl_TestStatus.Add(inp);

            TestAttachment_entity = inp;
            TestAttachment_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_TestStatus p)
        {
            var record = _context.Tbl_Cl_TestStatus.Where(x => x.TestStatusID == p.TestStatusID).SingleOrDefault();
            if (record != null)
            {

                record.Description = p.Description;
                record.StatusName = p.StatusName;
                record.TestStatusID = p.TestStatusID;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int TestStatusID)
        {
            var record = _context.Tbl_Cl_TestStatus.Where(x => x.TestStatusID == TestStatusID).SingleOrDefault();
            _context.Tbl_Cl_TestStatus.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            TestAttachment_poco.TestStatusID = TestAttachment_entity.TestStatusID;
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
