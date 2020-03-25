using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
  

    public interface IExtraWorkAttachmentRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment> GetAll();
        LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }


}
