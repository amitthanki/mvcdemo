using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_GPSData
    {
        //Constructor
        public DOGEN_GPSData()
        {

        }


        #region public properties

        public string HICN { get; set; }
        public string HouseholdId { get; set; }
        public string MBIId { get; set; }
        public long? IndividualId { get; set; }
        public string MemberId { get; set; }
        public string ContractNumber { get; set; }
        public string PBP { get; set; }
        public string LOB { get; set; }
        public DateTime? DOB { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string County { get; set; }
        public string Gender { get; set; }
        public string SCCCode { get; set; }
        public DateTime? PlanEffectiveDate { get; set; }
        public DateTime? PlanTermDate { get; set; }
        public string LOBDescription { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string InvalidAddress { get; set; }
        public string ZipCode4 { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public DateTime? SCCEffectiveDate { get; set; }
        public DateTime? SCCEndDate { get; set; }
        public string OOAIndicator { get; set; }
        public DateTime? GPSOOADisenrollmentDate { get; set; }
        public int PDPAutoEntrolleeIndicator { get; set; }
        public string ApplicationApprovedStatus { get; set; }
        public DateTime? EmployerEffectiveDate { get; set; }
        public DateTime? EmployerCloseDate { get; set; }
        public string EmployerId { get; set; }

        #endregion
    }
}
