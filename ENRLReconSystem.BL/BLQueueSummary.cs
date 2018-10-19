using ENRLReconSystem.DAL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.BL
{
    public class BLQueueSummary
    {
        private ExceptionTypes retValue;
        public ExceptionTypes GetQueueSummary(DateTime dtpStartDate, DateTime dtpEndDate, long lBusinessSegmentLkup, long? lDiscrepancyCategory, out QueueSummary objQueueSummary, out string strErrorMessage)
        {
            retValue = new ExceptionTypes();
            DALQueueSummary objDALQueueSummary = new DALQueueSummary();
            return retValue = objDALQueueSummary.GetQueueSummary(dtpStartDate, dtpEndDate, lBusinessSegmentLkup, lDiscrepancyCategory, out objQueueSummary, out strErrorMessage);
        }

        public ExceptionTypes GetGMURecord(DateTime dtpStartDate, DateTime dtpEndDate, long lBusinessSegmentLkup, long lQueueLkup, long? lQueueIdToSkip, long lLoginUserId, bool isRestrictedUser, out DOGEN_Queue objDOGEN_Queue, out string strErrorMessage)
        {
            retValue = new ExceptionTypes();
            DALQueueSummary objDALQueueSummary = new DALQueueSummary();
            return retValue = objDALQueueSummary.GetGMURecord(dtpStartDate, dtpEndDate, lBusinessSegmentLkup, lQueueLkup, lQueueIdToSkip, lLoginUserId, isRestrictedUser, out objDOGEN_Queue, out strErrorMessage);
        }

        public ExceptionTypes GetPendedRecords(long lPendedByRef, long lBusinessSegmentLkup, long lWorkBasketLkup, long? lDiscrepancyCategoryLkup, out List<DOGEN_Queue> lstDOGEN_Queue, out string strErrorMessage)
        {
            retValue = new ExceptionTypes();
            DALQueueSummary objDALQueueSummary = new DALQueueSummary();
            return retValue = objDALQueueSummary.GetPendedRecords(lPendedByRef, lBusinessSegmentLkup, lWorkBasketLkup, lDiscrepancyCategoryLkup, out lstDOGEN_Queue, out strErrorMessage);
        }

        public ExceptionTypes GetMostRecentItems(long? TimeZone,long aDM_UserMasterId,long workBasketLkup,long businessSegment,out  List<MostRecentItem> lstMostRecentItems, out string errorMsg)
        {
            retValue = new ExceptionTypes();
            DALQueueSummary objDALQueueSummary = new DALQueueSummary();
            return retValue = objDALQueueSummary.GetMostRecentItems(TimeZone, aDM_UserMasterId, workBasketLkup, businessSegment,out  lstMostRecentItems, out errorMsg);
        }
    }
}
