using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IHospitalDetail
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_HospitalDetail> GetAll();
        LaboratoryBusiness.POCO.User.Cl_HospitalDetail GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_HospitalDetail clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_HospitalDetail clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
