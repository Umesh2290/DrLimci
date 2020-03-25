using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
 
    public interface IExtraWorkRequestReposotory
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest> GetAll();
        LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }

}
