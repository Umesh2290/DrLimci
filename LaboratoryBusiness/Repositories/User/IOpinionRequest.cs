using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{

    public interface IOpinionRequestRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_OpinionRequest> GetAll();
        LaboratoryBusiness.POCO.User.Cl_OpinionRequest GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_OpinionRequest clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_OpinionRequest clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
