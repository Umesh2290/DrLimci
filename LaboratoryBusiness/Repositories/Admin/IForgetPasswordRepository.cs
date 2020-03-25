using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IForgetPasswordRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.ForgetPassword> GetAll();
        LaboratoryBusiness.POCO.Admin.ForgetPassword GetByID(int ForgetPasswordID);
        LaboratoryBusiness.POCO.Admin.ForgetPassword GetByGUID(string guid);
        void Insert(LaboratoryBusiness.POCO.Admin.ForgetPassword menuassignment);
        void Update(LaboratoryBusiness.POCO.Admin.ForgetPassword menuassignment);
        void Delete(int ForgetPasswordID);
        void Save();
    }
}
