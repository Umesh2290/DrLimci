using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class TestRepository : LaboratoryBusiness.Repositories.User.ITestRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_Test TestAttachment_entity = new Tbl_Cl_Test();
        private LaboratoryBusiness.POCO.User.Cl_Test TestAttachment_poco = new POCO.User.Cl_Test();

        public TestRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public TestRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_Test> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_Test.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_Test
                           {
                               AnalysisBy = p.AnalysisBy,
                               AnalysisDate = p.AnalysisDate,
                               ComplaintHistory = p.ComplaintHistory,
                               ConclusionBy = p.ConclusionBy,
                               ConclusionDate = p.ConclusionDate,
                               Description = p.Description,
                               IsPublish = p.IsPublish,
                               IsSampleCollected = p.IsSampleCollected,
                               IsSampleRequired = p.IsSampleRequired,
                               PatientUserID = p.PatientUserID,
                               PdfLink = p.PdfLink,
                               SampleCode = p.SampleCode,
                               SampleCollectionBy = p.SampleCollectionBy,
                               SampleCollectionDate = p.SampleCollectionDate,
                               SampleLabel = p.SampleLabel,
                               SampleType = p.SampleType,
                               TestCreatedBy = p.TestCreatedBy,
                               TestCreatedDate = p.TestCreatedDate,
                               TestID = p.TestID,
                               TestName = p.TestName,
                               TestStatusID = p.TestStatusID,
                               Cost = p.Cost,
                               IsInvoiceGenerated = p.IsInvoiceGenerated,
                               AurthorizeDate = p.AurthorizeDate,
                               ExternalConsultantBy=p.ExternalConsultantBy
                              


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_Test GetByID(int TestID)
        {
            var record = (from p in _context.Tbl_Cl_Test.AsEnumerable()
                          where p.TestID == TestID
                          select new LaboratoryBusiness.POCO.User.Cl_Test
                          {
                              AnalysisBy = p.AnalysisBy,
                              AnalysisDate = p.AnalysisDate,
                              ComplaintHistory = p.ComplaintHistory,
                              ConclusionBy = p.ConclusionBy,
                              ConclusionDate = p.ConclusionDate,
                              Description = p.Description,
                              IsPublish = p.IsPublish,
                              IsSampleCollected = p.IsSampleCollected,
                              IsSampleRequired = p.IsSampleRequired,
                              PatientUserID = p.PatientUserID,
                              PdfLink = p.PdfLink,
                              SampleCode = p.SampleCode,
                              SampleCollectionBy = p.SampleCollectionBy,
                              SampleCollectionDate = p.SampleCollectionDate,
                              SampleLabel = p.SampleLabel,
                              SampleType = p.SampleType,
                              TestCreatedBy = p.TestCreatedBy,
                              TestCreatedDate = p.TestCreatedDate,
                              TestID = p.TestID,
                              TestName = p.TestName,
                              TestStatusID = p.TestStatusID,
                              Cost = p.Cost,
                              IsInvoiceGenerated = p.IsInvoiceGenerated,
                              AurthorizeDate = p.AurthorizeDate,
                              ExternalConsultantBy = p.ExternalConsultantBy

                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_Test p)
        {
            Tbl_Cl_Test inp = new Tbl_Cl_Test()
            {

                AnalysisBy = p.AnalysisBy,
                AnalysisDate = p.AnalysisDate,
                ComplaintHistory = p.ComplaintHistory,
                ConclusionBy = p.ConclusionBy,
                ConclusionDate = p.ConclusionDate,
                Description = p.Description,
                IsPublish = p.IsPublish,
                IsSampleCollected = p.IsSampleCollected,
                IsSampleRequired = p.IsSampleRequired,
                PatientUserID = p.PatientUserID,
                PdfLink = p.PdfLink,
                SampleCode = p.SampleCode,
                SampleCollectionBy = p.SampleCollectionBy,
                SampleCollectionDate = p.SampleCollectionDate,
                SampleLabel = p.SampleLabel,
                SampleType = p.SampleType,
                TestCreatedBy = p.TestCreatedBy,
                TestCreatedDate = p.TestCreatedDate,
                TestID = p.TestID,
                TestName = p.TestName,
                TestStatusID = p.TestStatusID,
                Cost = p.Cost,
                IsInvoiceGenerated=p.IsInvoiceGenerated,
                AurthorizeDate = p.AurthorizeDate,
                ExternalConsultantBy = p.ExternalConsultantBy


            };
            _context.Tbl_Cl_Test.Add(inp);

            TestAttachment_entity = inp;
            TestAttachment_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_Test p)
        {
            var record = _context.Tbl_Cl_Test.Where(x => x.TestID == p.TestID).SingleOrDefault();
            if (record != null)
            {
       record.AnalysisBy = p.AnalysisBy;
                 record.AnalysisDate = p.AnalysisDate;
                 record.ComplaintHistory = p.ComplaintHistory;
                 record.ConclusionBy = p.ConclusionBy;
                 record.ConclusionDate = p.ConclusionDate;
                 record.Description = p.Description;
                 record.IsPublish = p.IsPublish;
                 record.IsSampleCollected = p.IsSampleCollected;
                 record.IsSampleRequired = p.IsSampleRequired;
                 record.PatientUserID = p.PatientUserID;
                 record.PdfLink = p.PdfLink;
                 record.SampleCode = p.SampleCode;
                 record.SampleCollectionBy = p.SampleCollectionBy;
                 record.SampleCollectionDate = p.SampleCollectionDate;
                 record.SampleLabel = p.SampleLabel;
                 record.SampleType = p.SampleType;
                 record.TestCreatedBy = p.TestCreatedBy;
                 record.TestCreatedDate = p.TestCreatedDate;
                 record.TestID = p.TestID;
                 record.TestName = p.TestName;
                 record.TestStatusID = p.TestStatusID;
                record.Cost = p.Cost;
                record.IsInvoiceGenerated = p.IsInvoiceGenerated;
                record.AurthorizeDate = p.AurthorizeDate;
                record.ExternalConsultantBy = p.ExternalConsultantBy;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int TestID)
        {
            var record = _context.Tbl_Cl_Test.Where(x => x.TestID == TestID).SingleOrDefault();
            _context.Tbl_Cl_Test.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            TestAttachment_poco.TestID = TestAttachment_entity.TestID;
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
