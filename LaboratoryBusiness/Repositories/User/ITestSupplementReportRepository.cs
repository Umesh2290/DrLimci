using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface ITestSupplementReportRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestSupplementReport> GetAll();
        LaboratoryBusiness.POCO.User.Cl_TestSupplementReport GetByID(int SupplementReportID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_TestSupplementReport supplementreport);
        void Update(LaboratoryBusiness.POCO.User.Cl_TestSupplementReport supplementreport);
        void Delete(int SupplementReportID);
        void Save();
    }
}
