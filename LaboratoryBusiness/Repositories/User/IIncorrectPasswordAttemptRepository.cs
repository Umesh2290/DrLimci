using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IIncorrectPasswordAttemptRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_IncorrectPasswordAttempt> GetAll();
        LaboratoryBusiness.POCO.User.Cl_IncorrectPasswordAttempt GetByID(int IncorrectPasswordAttemptID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_IncorrectPasswordAttempt incorrectpasswordattempt);
        void Update(LaboratoryBusiness.POCO.User.Cl_IncorrectPasswordAttempt incorrectpasswordattempt);
        void Delete(int IncorrectPasswordAttemptID);
        void Save();
    }
}
