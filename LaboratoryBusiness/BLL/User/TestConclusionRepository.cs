using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class TestConclusionRepository : LaboratoryBusiness.Repositories.User.ITestConclusionRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_TestConclusion TestAttachment_entity = new Tbl_Cl_TestConclusion();
        private LaboratoryBusiness.POCO.User.Cl_TestConclusion TestAttachment_poco = new POCO.User.Cl_TestConclusion();

        public TestConclusionRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public TestConclusionRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestConclusion> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_TestConclusion.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_TestConclusion
                           {

                               ClinicalDetails=p.ClinicalDetails,
                               Conclusion=p.Conclusion,
                               CreatedBy=p.CreatedBy,
                               CreatedDate=p.CreatedDate,
                               Macroscopy=p.Macroscopy,
                               Microscopy=p.Microscopy,
                               Report=p.Report,
                               SampleDescription=p.SampleDescription,
                               SnomedCoding=p.SnomedCoding,
                               SpecimenDetails=p.SpecimenDetails,
                               TestConclusionID=p.TestConclusionID,
                               TestID=p.TestID,
                               TestReportTypeID=p.TestReportTypeID,
                               UpdatedBy=p.UpdatedBy,
                               UpdatedDate=p.UpdatedDate




                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_TestConclusion GetByID(int TestConclusionID)
        {
            var record = (from p in _context.Tbl_Cl_TestConclusion.AsEnumerable()
                          where p.TestConclusionID == TestConclusionID
                          select new LaboratoryBusiness.POCO.User.Cl_TestConclusion
                          {

                              ClinicalDetails = p.ClinicalDetails,
                              Conclusion = p.Conclusion,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              Macroscopy = p.Macroscopy,
                              Microscopy = p.Microscopy,
                              Report = p.Report,
                              SampleDescription = p.SampleDescription,
                              SnomedCoding = p.SnomedCoding,
                              SpecimenDetails = p.SpecimenDetails,
                              TestConclusionID = p.TestConclusionID,
                              TestID = p.TestID,
                              TestReportTypeID = p.TestReportTypeID,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate






                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_TestConclusion p)
        {
            Tbl_Cl_TestConclusion inp = new Tbl_Cl_TestConclusion()
            {
                ClinicalDetails = p.ClinicalDetails,
                Conclusion = p.Conclusion,
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                Macroscopy = p.Macroscopy,
                Microscopy = p.Microscopy,
                Report = p.Report,
                SampleDescription = p.SampleDescription,
                SnomedCoding = p.SnomedCoding,
                SpecimenDetails = p.SpecimenDetails,
                TestConclusionID = p.TestConclusionID,
                TestID = p.TestID,
                TestReportTypeID = p.TestReportTypeID,
                UpdatedBy = p.UpdatedBy,
                UpdatedDate = p.UpdatedDate


            };
            _context.Tbl_Cl_TestConclusion.Add(inp);

            TestAttachment_entity = inp;
            TestAttachment_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_TestConclusion p)
        {
            var record = _context.Tbl_Cl_TestConclusion.Where(x => x.TestConclusionID == p.TestConclusionID).SingleOrDefault();
            if (record != null)
            {
                   record.ClinicalDetails = p.ClinicalDetails;
                              record.Conclusion = p.Conclusion;
                              record.CreatedBy = p.CreatedBy;
                              record.CreatedDate = p.CreatedDate;
                              record.Macroscopy = p.Macroscopy;
                              record.Microscopy = p.Microscopy;
                              record.Report = p.Report;
                              record.SampleDescription = p.SampleDescription;
                              record.SnomedCoding = p.SnomedCoding;
                              record.SpecimenDetails = p.SpecimenDetails;
                              record.TestConclusionID = p.TestConclusionID;
                              record.TestID = p.TestID;
                              record.TestReportTypeID = p.TestReportTypeID;
                              record.UpdatedBy = p.UpdatedBy;
                              record.UpdatedDate = p.UpdatedDate;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int TestConclusionID)
        {
            var record = _context.Tbl_Cl_TestConclusion.Where(x => x.TestConclusionID == TestConclusionID).SingleOrDefault();
            _context.Tbl_Cl_TestConclusion.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            TestAttachment_poco.TestConclusionID = TestAttachment_entity.TestConclusionID;
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
