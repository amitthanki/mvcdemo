using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENRLReconSystem.WebAPI.Models
{
    public class MacroModel
    {
        public int MyProperty { get; set; }
    }
 
    public class MacroPostDataModels
    {
        public long? ERSCaseId  { get; set; }
        public string HouseholdID  { get; set; }
        public string Status  { get; set; }
        public string ReasonForFail  { get; set; }
    }
    public class OutputModel
    {

    }
    }