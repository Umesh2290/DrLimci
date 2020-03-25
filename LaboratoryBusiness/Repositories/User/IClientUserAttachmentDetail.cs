using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IClientUserAttachmentDetailRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail> GetAll();
        LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail GetByID(int AdminDetailID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail admindetail);
        void Update(LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail admindetail);
        void Delete(int AdminDetailID);
        void Save();
    }
}
