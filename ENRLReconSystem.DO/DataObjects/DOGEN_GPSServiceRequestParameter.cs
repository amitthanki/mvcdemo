using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_GPSServiceRequestParameter
    {
        //Constructor
        public DOGEN_GPSServiceRequestParameter()
        {

        }
        public long? ActionLkup { get; set; }
        public string ERSCaseId { get; set; }
        public string TransactionId { get; set; }
        public string AccountId { get; set; }
        public string EmployerId { get; set; }
        public string IndividualId { get; set; }
        public string EmployerAccountId { get; set; }
        public string MedicareClaimNumber { get; set; }
        public string MemberNumber { get; set; }
        public string ApplicationDate { get; set; }
        public string BirthDate { get; set; }
        public string CaseNumber { get; set; }
        public string ContractNumber { get; set; }
        public string EffectiveEndDate { get; set; }
        public string EffectiveStartDate { get; set; }
        public string ElectionType { get; set; }
        public string PbpNo { get; set; }
        public string TransactionCode { get; set; }
        public string HouseholdId { get; set; }
        public string OutOfAreaDisenrollmentDate { get; set; }
        public string SendFulfillmentInd { get; set; }
        public string OutOfAreaOptionRequest { get; set; }
        public string LastName { get; set; }

        public long LoggedInUserId { get; set; }

      

    }
}
