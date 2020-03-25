using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{

    public interface IPatientDetailRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_PatientDetail> GetAll();
        LaboratoryBusiness.POCO.User.Cl_PatientDetail GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_PatientDetail clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_PatientDetail clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
