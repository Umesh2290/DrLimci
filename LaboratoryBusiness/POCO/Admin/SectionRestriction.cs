﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.Admin
{
    public class SectionRestriction
    {
        public int SectionID { get; set; }
        public string SectionSelector { get; set; }
        public Nullable<int> MenuID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
