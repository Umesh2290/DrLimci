﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboratorySystem.ViewModels
{
    public class TestInvestigation
    {
        public int InvestigationId { get; set; }
        public string InvestigationName { get; set; }
        public string InvestigationValues { get; set; }
        public string InvestigationRange { get; set; }
        public string InvestigationResult { get; set; }
    }
}