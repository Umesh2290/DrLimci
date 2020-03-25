using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface ITestReportTypeRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestReportType> GetAll();
        LaboratoryBusiness.POCO.User.Cl_TestReportType GetByID(int TestReportTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_TestReportType TestInvestigation);
        void Update(LaboratoryBusiness.POCO.User.Cl_TestReportType TestInvestigation);
        void Delete(int TestReportTypeID);
        void Save();
    }
}
