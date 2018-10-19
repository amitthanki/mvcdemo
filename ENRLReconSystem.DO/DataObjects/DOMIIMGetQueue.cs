using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO.DataObjects
{
    [Serializable]
    public class DOMIIMGetQueue
    {

        public long CaseId { get; set; }
        public string CreationDate { get; set; }
        public string Status { get; set; }
        public long StatusRef { get; set; }
        public string CaseType { get; set; }
        public long CaseTypeRef { get; set; }
        public string CaseCategoryType { get; set; }
        public long CaseCategoryTypeRef { get; set; }
    }

    [Serializable]
    public class DOMIIMOOACommentUpdate
        {
        public int  ERSCaseId { get; set; }
        public string MIIMReferenceId { get; set; }
        public string Comments { get; set; }

    }

    [Serializable]
    public class DOServiceProcessDetails
    {

        public string ServiceName { get; set; }
        public string ClientMachineName { get; set; }
        public long ServiceRequestedBy { get; set; }
        public string ServiceRequestorName { get; set; }
    }
}
