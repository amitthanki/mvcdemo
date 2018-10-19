using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOMQTRRWorkQueueItems
    {

        public DOMQTRRWorkQueueItems()
        {

        }

        public DOMQTRRWorkQueueItems(XDocument xmlDoucument)
        {
            XElement element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("IndividualID")).FirstOrDefault();
            if (element != null)
                IndividualID = element.Value;
            element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("MedicareClaimNumber")).FirstOrDefault();
            if (element != null)
                HICN = element.Value;
            element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("AccountID")).FirstOrDefault();
            if (element != null)
                HouseHoldID = element.Value;
            element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("ContractNo")).FirstOrDefault();
            if (element != null)
                Contract = element.Value;
            element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("PBPNo")).FirstOrDefault();
            if (element != null)
                PBP = element.Value;
            element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("TransactionReplyCode")).FirstOrDefault();
            if (element != null)
                TRC = element.Value;
            element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("TransactionTypeCode")).FirstOrDefault();
            if (element != null)
                TRCTypeCode = element.Value;
            element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("TrrRecordID")).FirstOrDefault();
            if (element != null)
                TrrRecordID = element.Value;
            element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("CaseNumber")).FirstOrDefault();
            if (element != null)
                StrERSCaseNumber = element.Value;
            element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("ReasonDesc")).FirstOrDefault();
            if (element != null)
                ReasonDescription = element.Value;
            element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("EffectiveDate")).FirstOrDefault();
            if (element != null)
                TimelineEffectiveDate = Convert.ToDateTime(element.Value);

            if(String.IsNullOrEmpty(HICN))
            {
                element = xmlDoucument.Descendants().Where(x => x.Name.LocalName.Contains("CustomField1")).FirstOrDefault();
                if (element != null)
                    HICN =  element.Value.Split(':').Length > 1 ? element.Value.Split(':')[1].Trim() : "" ; //HICN: 8D53VC7QU09;
            }

        }

        #region MQ Message data properties
        public string IndividualID { get; set; }
        public string HICN { get; set; }
        public string HouseHoldID { get; set; }
        public string Contract { get; set; }
        public string PBP { get; set; }
        public string TRC { get; set; }
        public string TRCTypeCode { get; set; }
        public string TrrRecordID { get; set; }
        public long ERSCaseNumber { get; set; }
        public string StrERSCaseNumber { get; set; }
        public string ReasonDescription { get; set; }
        public DateTime? TimelineEffectiveDate { get; set; }
        public long CMN_BackgroundProcessMasterRef { get; set; }
        public long MQMessagesRecievedRef { get; set; }
        #endregion

        #region Services and Gen_Queue data Properties
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string WQTrackingNumber { get; set; }
        public string MemberID { get; set; }
        public int DisenrollementPeriod { get; set; }
        public string SCCCode { get; set; }
        public DateTime? TRRFileReceiptDate { get; set; }
        public DateTime? PlanTerminationDate { get; set; }
        public DateTime? GPSProposedEffectiveDate { get; set; }
        public bool GPSPDPAutoEnroleeIndicator { get; set; }
        public string GPSApplicationStatus { get; set; }
        public string GPSContract { get; set; }
        public string GPSPBP { get; set; }
        public long MQSourceTypeLkup { get; set; }
        public string LOB { get; set; }
        public long MQTRRWorkQueueItemId { get; set; }
        public bool IsRestricted { get; set; }
        public string EmployerId { get; set; }
        public string StateAbbreviation { get; set; }
        public bool IsNationalEmployee { get; set; }
        #endregion
    }
}
