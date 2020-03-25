using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class TestInvestigationRepository : LaboratoryBusiness.Repositories.User.ITestInvestigationRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_TestInvestigation TestAttachment_entity = new Tbl_Cl_TestInvestigation();
        private LaboratoryBusiness.POCO.User.Cl_TestInvestigation TestAttachment_poco = new POCO.User.Cl_TestInvestigation();

        public TestInvestigationRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public TestInvestigationRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestInvestigation> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_TestInvestigation.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_TestInvestigation
                           {
CreatedBy=p.CreatedBy,
CreatedDate=p.CreatedDate,
ExtraWorkID=p.ExtraWorkID,
InvestigationID=p.InvestigationID,
InvestigationName=p.InvestigationName,
InvestigationResult=p.InvestigationResult,
Range=p.Range,
TestID=p.TestID,
UpdatedBy=p.UpdatedBy,
UpdatedDate=p.UpdatedDate,
Value=p.Value



                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_TestInvestigation GetByID(int InvestigationID)
        {
            var record = (from p in _context.Tbl_Cl_TestInvestigation.AsEnumerable()
                          where p.InvestigationID == InvestigationID
                          select new LaboratoryBusiness.POCO.User.Cl_TestInvestigation
                          {
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              ExtraWorkID = p.ExtraWorkID,
                              InvestigationID = p.InvestigationID,
                              InvestigationName = p.InvestigationName,
                              InvestigationResult = p.InvestigationResult,
                              Range = p.Range,
                              TestID = p.TestID,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate,
                              Value = p.Value





                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_TestInvestigation p)
        {
            Tbl_Cl_TestInvestigation inp = new Tbl_Cl_TestInvestigation()
            {
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                ExtraWorkID = p.ExtraWorkID,
                InvestigationID = p.InvestigationID,
                InvestigationName = p.InvestigationName,
                InvestigationResult = p.InvestigationResult,
                Range = p.Range,
                TestID = p.TestID,
                UpdatedBy = p.UpdatedBy,
                UpdatedDate = p.UpdatedDate,
                Value = p.Value


            };
            _context.Tbl_Cl_TestInvestigation.Add(inp);

            TestAttachment_entity = inp;
            TestAttachment_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_TestInvestigation p)
        {
            var record = _context.Tbl_Cl_TestInvestigation.Where(x => x.InvestigationID == p.InvestigationID).SingleOrDefault();
            if (record != null)
            {
                  record.CreatedBy = p.CreatedBy;
                              record.CreatedDate = p.CreatedDate;
                              record.ExtraWorkID = p.ExtraWorkID;
                              record.InvestigationID = p.InvestigationID;
                              record.InvestigationName = p.InvestigationName;
                              record.InvestigationResult = p.InvestigationResult;
                              record.Range = p.Range;
                              record.TestID = p.TestID;
                              record.UpdatedBy = p.UpdatedBy;
                             record.UpdatedDate = p.UpdatedDate;
                             record.Value = p.Value;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int InvestigationID)
        {
            var record = _context.Tbl_Cl_TestInvestigation.Where(x => x.InvestigationID == InvestigationID).SingleOrDefault();
            _context.Tbl_Cl_TestInvestigation.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            TestAttachment_poco.InvestigationID = TestAttachment_entity.InvestigationID;
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
