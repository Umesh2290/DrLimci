using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IClientUserRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_ClientUser> GetAll();
        LaboratoryBusiness.POCO.User.Cl_ClientUser GetByID(int ClientUserID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_ClientUser clientuser);
        void Update(LaboratoryBusiness.POCO.User.Cl_ClientUser clientuser);
        void Delete(int ClientUserID);
        void Save();
        bool Unblock(int ClientUserID, bool isAutoUnblock, Repositories.User.IIncorrectPasswordAttemptRepository increpo, Repositories.User.IClientUserRepository clurepo
            ,Repositories.User.IClientUserTypeRepository cltyperepo,Repositories.User.IAdminDetailRepository addetailrepo);
    }
}
