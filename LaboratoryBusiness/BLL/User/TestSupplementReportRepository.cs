using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryBusiness.DAL.Client;

namespace LaboratoryBusiness.BLL.User
{
    public class TestSupplementReportRepository : LaboratoryBusiness.Repositories.User.ITestSupplementReportRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_TestSupplementReport TestAttachment_entity = new Tbl_Cl_TestSupplementReport();
        private LaboratoryBusiness.POCO.User.Cl_TestSupplementReport TestAttachment_poco = new POCO.User.Cl_TestSupplementReport();

        public TestSupplementReportRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public TestSupplementReportRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestSupplementReport> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_TestSupplementReport.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_TestSupplementReport
                           {

                               ClinicalDetails=p.ClinicalDetails,
                               SupplementReportConclusion = p.SupplementReportConclusion,
                               CreatedBy=p.CreatedBy,
                               CreatedDate=p.CreatedDate,
                               Macroscopy=p.Macroscopy,
                               Microscopy=p.Microscopy,
                               Report=p.Report,
                               SampleDescription=p.SampleDescription,
                               SnomedCoding=p.SnomedCoding,
                               SpecimenDetails=p.SpecimenDetails,
                               TestSupplementReportID = p.TestSupplementReportID,
                               TestID=p.TestID,
                               TestReportTypeID=p.TestReportTypeID,
                               UpdatedBy=p.UpdatedBy,
                               UpdatedDate=p.UpdatedDate




                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_TestSupplementReport GetByID(int TestSupplementReportID)
        {
            var record = (from p in _context.Tbl_Cl_TestSupplementReport.AsEnumerable()
                          where p.TestSupplementReportID == TestSupplementReportID
                          select new LaboratoryBusiness.POCO.User.Cl_TestSupplementReport
                          {

                              ClinicalDetails = p.ClinicalDetails,
                              SupplementReportConclusion = p.SupplementReportConclusion,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              Macroscopy = p.Macroscopy,
                              Microscopy = p.Microscopy,
                              Report = p.Report,
                              SampleDescription = p.SampleDescription,
                              SnomedCoding = p.SnomedCoding,
                              SpecimenDetails = p.SpecimenDetails,
                              TestSupplementReportID = p.TestSupplementReportID,
                              TestID = p.TestID,
                              TestReportTypeID = p.TestReportTypeID,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate






                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_TestSupplementReport p)
        {
            Tbl_Cl_TestSupplementReport inp = new Tbl_Cl_TestSupplementReport()
            {
                ClinicalDetails = p.ClinicalDetails,
                SupplementReportConclusion = p.SupplementReportConclusion,
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                Macroscopy = p.Macroscopy,
                Microscopy = p.Microscopy,
                Report = p.Report,
                SampleDescription = p.SampleDescription,
                SnomedCoding = p.SnomedCoding,
                SpecimenDetails = p.SpecimenDetails,
                TestSupplementReportID = p.TestSupplementReportID,
                TestID = p.TestID,
                TestReportTypeID = p.TestReportTypeID,
                UpdatedBy = p.UpdatedBy,
                UpdatedDate = p.UpdatedDate


            };
            _context.Tbl_Cl_TestSupplementReport.Add(inp);

            TestAttachment_entity = inp;
            TestAttachment_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_TestSupplementReport p)
        {
            var record = _context.Tbl_Cl_TestSupplementReport.Where(x => x.TestSupplementReportID == p.TestSupplementReportID).SingleOrDefault();
            if (record != null)
            {
                   record.ClinicalDetails = p.ClinicalDetails;
                              record.SupplementReportConclusion = p.SupplementReportConclusion;
                              record.CreatedBy = p.CreatedBy;
                              record.CreatedDate = p.CreatedDate;
                              record.Macroscopy = p.Macroscopy;
                              record.Microscopy = p.Microscopy;
                              record.Report = p.Report;
                              record.SampleDescription = p.SampleDescription;
                              record.SnomedCoding = p.SnomedCoding;
                              record.SpecimenDetails = p.SpecimenDetails;
                              record.TestSupplementReportID = p.TestSupplementReportID;
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

        public void Delete(int TestSupplementReportID)
        {
            var record = _context.Tbl_Cl_TestSupplementReport.Where(x => x.TestSupplementReportID == TestSupplementReportID).SingleOrDefault();
            _context.Tbl_Cl_TestSupplementReport.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            TestAttachment_poco.TestSupplementReportID = TestAttachment_entity.TestSupplementReportID;
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
