using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface ISectionRestrictionRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_SectionRestriction> GetAll();
        LaboratoryBusiness.POCO.User.Cl_SectionRestriction GetByID(int SectionRestrictionID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_SectionRestriction sectionrestriction);
        void Update(LaboratoryBusiness.POCO.User.Cl_SectionRestriction sectionrestriction);
        void Delete(int SectionRestrictionID);
        void Save();
    }
}
