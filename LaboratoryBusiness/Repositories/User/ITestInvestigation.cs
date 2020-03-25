using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface ITestInvestigationRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestInvestigation> GetAll();
        LaboratoryBusiness.POCO.User.Cl_TestInvestigation GetByID(int InvestigationID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_TestInvestigation TestInvestigation);
        void Update(LaboratoryBusiness.POCO.User.Cl_TestInvestigation TestInvestigation);
        void Delete(int InvestigationID);
        void Save();
    }

}
