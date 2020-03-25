using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IForgetPasswordRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_ForgetPassword> GetAll();
        LaboratoryBusiness.POCO.User.Cl_ForgetPassword GetByID(int ForgetPasswordID);
        LaboratoryBusiness.POCO.User.Cl_ForgetPassword GetByGUID(string guid);
        void Insert(LaboratoryBusiness.POCO.User.Cl_ForgetPassword forgetpassword);
        void Update(LaboratoryBusiness.POCO.User.Cl_ForgetPassword forgetpassword);
        void Delete(int ForgetPasswordID);
        void Save();
    }
}
