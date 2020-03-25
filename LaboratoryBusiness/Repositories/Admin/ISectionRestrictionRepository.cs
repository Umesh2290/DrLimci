using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface ISectionRestrictionRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.SectionRestriction> GetAll();
        LaboratoryBusiness.POCO.Admin.SectionRestriction GetByID(int SectionRestrictionID);
        void Insert(LaboratoryBusiness.POCO.Admin.SectionRestriction sectionrestriction);
        void Update(LaboratoryBusiness.POCO.Admin.SectionRestriction sectionrestriction);
        void Delete(int SectionRestriction);
        void Save();
    }
}
