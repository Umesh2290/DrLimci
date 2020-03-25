using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{

    public interface IClientUserDetailRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_ClientUserDetail> GetAll();
        LaboratoryBusiness.POCO.User.Cl_ClientUserDetail GetByID(int AdminDetailID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_ClientUserDetail admindetail);
        void Update(LaboratoryBusiness.POCO.User.Cl_ClientUserDetail admindetail);
        void Delete(int AdminDetailID);
        void Save();
    }
}
