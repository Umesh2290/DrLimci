using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class TestReportTypeRepository : LaboratoryBusiness.Repositories.User.ITestReportTypeRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_TestReportType TestAttachment_entity = new Tbl_Cl_TestReportType();
        private LaboratoryBusiness.POCO.User.Cl_TestReportType TestAttachment_poco = new POCO.User.Cl_TestReportType();

        public TestReportTypeRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public TestReportTypeRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestReportType> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_TestReportType.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_TestReportType
                           {
                             Description=p.Description,
                             Name=p.Name,
                             TestReportTypeID=p.TestReportTypeID



                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_TestReportType GetByID(int TestReportTypeID)
        {
            var record = (from p in _context.Tbl_Cl_TestReportType.AsEnumerable()
                          where p.TestReportTypeID == TestReportTypeID
                          select new LaboratoryBusiness.POCO.User.Cl_TestReportType
                          {
                              Description = p.Description,
                              Name = p.Name,
                              TestReportTypeID = p.TestReportTypeID



                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_TestReportType p)
        {
            Tbl_Cl_TestReportType inp = new Tbl_Cl_TestReportType()
            {

                Description = p.Description,
                Name = p.Name,
                TestReportTypeID = p.TestReportTypeID

            };
            _context.Tbl_Cl_TestReportType.Add(inp);

            TestAttachment_entity = inp;
            TestAttachment_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_TestReportType p)
        {
            var record = _context.Tbl_Cl_TestReportType.Where(x => x.TestReportTypeID == p.TestReportTypeID).SingleOrDefault();
            if (record != null)
            {

                record.Description = p.Description;
                record.Name = p.Name;
                record.TestReportTypeID = p.TestReportTypeID;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int InvestigationID)
        {
            var record = _context.Tbl_Cl_TestReportType.Where(x => x.TestReportTypeID == InvestigationID).SingleOrDefault();
            _context.Tbl_Cl_TestReportType.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            TestAttachment_poco.TestReportTypeID = TestAttachment_entity.TestReportTypeID;
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
