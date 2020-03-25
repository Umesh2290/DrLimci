using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IIncorrectPasswordAttemptRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.IncorrectPasswordAttempt> GetAll();
        LaboratoryBusiness.POCO.Admin.IncorrectPasswordAttempt GetByID(int IncorrectPasswordID);
        void Insert(LaboratoryBusiness.POCO.Admin.IncorrectPasswordAttempt menuassignment);
        void Update(LaboratoryBusiness.POCO.Admin.IncorrectPasswordAttempt menuassignment);
        void Delete(int IncorrectPasswordID);
        void Save();
    }
}
