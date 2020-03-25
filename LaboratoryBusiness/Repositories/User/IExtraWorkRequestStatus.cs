using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{

    public interface IExtraWorkRequestStatusRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequestStatus> GetAll();
        LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequestStatus GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequestStatus clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequestStatus clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
