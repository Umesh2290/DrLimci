using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IAdminDetailRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_AdminDetail> GetAll();
        LaboratoryBusiness.POCO.User.Cl_AdminDetail GetByID(int AdminDetailID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_AdminDetail admindetail);
        void Update(LaboratoryBusiness.POCO.User.Cl_AdminDetail admindetail);
        void Delete(int AdminDetailID);
        void Save();
    }
}
