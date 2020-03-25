using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{

    public interface IOpinionRequestStatus
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_OpinionRequestStatus> GetAll();
        LaboratoryBusiness.POCO.User.Cl_OpinionRequestStatus GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_OpinionRequestStatus clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_OpinionRequestStatus clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
