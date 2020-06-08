using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
   public class TestRateRepository :LaboratoryBusiness.Repositories.User.ITestRate
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_TestRate testRate_entity = new Tbl_Cl_TestRate();
        private LaboratoryBusiness.POCO.User.Cl_TestRate testRate_poco = new POCO.User.Cl_TestRate();

        public TestRateRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public TestRateRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }
        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestRate> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_TestRate.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_TestRate
                           {
                               Cost = p.Cost,
                               TestName = p.TestName,
                               ID = p.ID



                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_TestRate GetByID(int TestReportTypeID)
        {
            var record = (from p in _context.Tbl_Cl_TestRate.AsEnumerable()
                          where p.ID == TestReportTypeID
                          select new LaboratoryBusiness.POCO.User.Cl_TestRate
                          {
                              Cost = p.Cost,
                              TestName = p.TestName,
                              ID = p.ID



                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_TestRate p)
        {
            Tbl_Cl_TestRate inp = new Tbl_Cl_TestRate()
            {

                Cost = p.Cost,
                TestName = p.TestName,
                ID = p.ID

            };
            _context.Tbl_Cl_TestRate.Add(inp);

            testRate_entity = inp;
            testRate_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_TestRate p)
        {
            var record = _context.Tbl_Cl_TestRate.Where(x => x.ID == p.ID).SingleOrDefault();
            if (record != null)
            {

                record.ID = p.ID;
                record.TestName = p.TestName;
                record.Cost = p.Cost;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int InvestigationID)
        {
            var record = _context.Tbl_Cl_TestRate.Where(x => x.ID == InvestigationID).SingleOrDefault();
            _context.Tbl_Cl_TestRate.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            testRate_poco.ID = testRate_entity.ID;
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

