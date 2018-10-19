using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_QueueWorkFlowCorrelation
    {
        //Constructor
        public DOGEN_QueueWorkFlowCorrelation()
        {

        }


        #region public properties
        public long GEN_QueueWorkFlowCorrelationId { get; set; }
        public long GEN_QueueRef { get; set; }
        public long RoleLkup { get; set; }
        public long WorkBasketLkup { get; set; }
        public long DiscripancyCategoryLkup { get; set; }
        public long? PreviousActionLkup { get; set; }
        public long? PreviousWorkQueuesLkup { get; set; }
        public long? PreviousStatusLkup { get; set; }
        public long? CurrentActionLkup { get; set; }
        public long? CurrentWorkQueuesLkup { get; set; }
        public long? CurrentStatusLkup { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public string Role { get; set; }
        public string WorkBasket { get; set; }
        public string DiscripancyCategory { get; set; }
        public string PreviousAction { get; set; }
        public string PreviousWorkQueues { get; set; }
        public string PreviousStatus { get; set; }
        public string CurrentAction { get; set; }
        public string CurrentWorkQueues { get; set; }
        public string CurrentStatus { get; set; }
        public string CreatedBy { get; set; }



        #endregion

    }
}
