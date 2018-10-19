using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ENRLReconSystem.Controllers
{
    [ERSAuthenticationAttribute(Roles = "User")]
    public class HomeController : Controller
    {
        BLQueueSummary _objBLQueueSummary = new BLQueueSummary();
        private UIUserLogin currentUser;
        private BLReports _objBLReports = new BLReports();
        private List<long> objpendingQueues = Enum.GetValues(typeof(PendingQueues)).Cast<long>().ToList();
        private List<long> objOSTHoldingQueues = Enum.GetValues(typeof(OSTHoldingQueues)).Cast<long>().ToList();
        public HomeController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            }
        }
       
        public ActionResult Home()
        {
            
            QueueSummary objQueueSummary ;
            DateTime dtpStartDate = DateTime.UtcNow.AddDays(-90);
            DateTime dtpEndDate = DateTime.UtcNow;
            try
            {
                objQueueSummary = GetQueueSummary(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup, null);
                objQueueSummary.lstDOADM_AlertDetails = GetAlerts(currentUser.ADM_UserMasterId);
                objQueueSummary.lstDOADM_ResourceDetails = GetResources();
                objQueueSummary.BusinessSegment = currentUser.BusinessSegmentLkup;
                return View(objQueueSummary);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Home, (long)ExceptionTypes.Uncategorized, ex.Message.NullToString(), ex.Message.NullToString());
                return RedirectToAction("Maintenance", "Error",new { Error= ex.ToString() } );
            }
        }
        public ActionResult GetPendRecords()
        {
            List<DOGEN_Queue> lstDOGEN_Queue = new List<DOGEN_Queue>();
            try
            {
                lstDOGEN_Queue = GetPendedRecords(currentUser.ADM_UserMasterId, (long)currentUser.BusinessSegmentLkup, (long)currentUser.WorkBasketLkup);
                return PartialView("_GetPendingWorkItem", lstDOGEN_Queue);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Home, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), ex.Message.ToString());
                return PartialView("");
            }
        }
        public ActionResult GetMostRecentRecord()
        {
            List<MostRecentItem> lstMostRecentItem = new List<MostRecentItem>();
            try
            {
                lstMostRecentItem = GetMostRecentItems(currentUser.ADM_UserMasterId, currentUser.WorkBasketLkup.ToInt64(), currentUser.BusinessSegmentLkup.ToInt64());
                return PartialView("_MostRecentCases", lstMostRecentItem);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Home, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), ex.Message.ToString());
                return PartialView("");
            }
        }



        [HttpPost]
        public ActionResult FilterQueueSummary(DateTime? dtpStartDate , DateTime? dtpEndDate)
        {
            QueueSummary objQueueSummary;
            if(!dtpStartDate.HasValue)
                dtpStartDate = DateTime.Now.AddDays(-90);
            if(!dtpEndDate.HasValue)
                dtpEndDate = DateTime.Now;
            try
            {
                objQueueSummary = GetQueueSummary(dtpStartDate.Value, dtpEndDate.Value, (long)currentUser.BusinessSegmentLkup, null);
                return PartialView("_QueueSummary", objQueueSummary);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Home, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }
        [HttpPost]
        public ActionResult FilterPendedRecords(DateTime? dtpStartDate, DateTime? dtpEndDate)
        {
            List<DOGEN_Queue> lstDOGEN_Queue = new List<DOGEN_Queue>();
            string strErrorMessage;
            try
            {
                lstDOGEN_Queue = GetPendedRecords(currentUser.ADM_UserMasterId, (long)currentUser.BusinessSegmentLkup, (long)currentUser.WorkBasketLkup);
                return PartialView("_GetPendingWorkItem", lstDOGEN_Queue);
            }
            catch (Exception ex)
            {
                //log error
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Home, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                strErrorMessage = ex.Message;
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        private List<DOADM_AlertDetails> GetAlerts(long lUserId)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            ExceptionTypes result;
            BLAlerts objBLAlerts = new BLAlerts();
            DOADM_AlertDetails objDOADM_AlertDetails = new DOADM_AlertDetails();
            objDOADM_AlertDetails.LoginUserId = lUserId;//CreatedByRef is mapped to @loginID SQL Parameter
            objDOADM_AlertDetails.IsActive = true;
            objDOADM_AlertDetails.ConsiderDates = true;
            result = objBLAlerts.SearchAlerts(TimeZone,objDOADM_AlertDetails, out List<DOADM_AlertDetails> lstDOADM_AlertDetails,out string strErrorMessage);
            lstDOADM_AlertDetails = lstDOADM_AlertDetails.OrderByDescending(x => x.AlertCriticalityLkup).ToList();
            //check result for DB action
            if (result != (long)ExceptionTypes.Success)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Home, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
            }
            return lstDOADM_AlertDetails;
        }

        private List<DOADM_ResourceDetails> GetResources()
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            ExceptionTypes result;
            List<DOADM_ResourceDetails> lstDOADM_ResourceDetails;
            BLResources objBLResources = new BLResources();
            DOADM_ResourceDetails objDOADM_ResourceDetails = new DOADM_ResourceDetails();
            objDOADM_ResourceDetails.IsActive = true;
            objDOADM_ResourceDetails.ConsiderDates = true;
            result = objBLResources.SearchResources(TimeZone,objDOADM_ResourceDetails, out lstDOADM_ResourceDetails, out string strErrorMessage);
            //check result for DB action
            if (result != (long)ExceptionTypes.Success)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Home, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
            }
            return lstDOADM_ResourceDetails;
        }

        private QueueSummary GetQueueSummary( DateTime dtpStartDate,  DateTime dtpEndDate, long lBusinessSegmentLkup, long? lDiscrepancyCategoryLkup)
        {
            ExceptionTypes result = _objBLQueueSummary.GetQueueSummary(dtpStartDate, dtpEndDate, lBusinessSegmentLkup, lDiscrepancyCategoryLkup, out QueueSummary objQueueSummary, out string strErrorMessage);
            objQueueSummary.StartDate = dtpStartDate;
            objQueueSummary.EndDate = dtpEndDate;
            objQueueSummary.lstUserAccessQueueLkups = currentUser.UserQueueList.Select(item => item.QueueLkp).Distinct().ToList();//to set queue link access on home page
            SetSegmentVisibility(ref objQueueSummary);
            return objQueueSummary;
        }

        private List<DOGEN_Queue> GetPendedRecords(long lPendedByRef, long lBusinessSegmentLkup, long lWorkBasket)
        {
            ExceptionTypes result = _objBLQueueSummary.GetPendedRecords(lPendedByRef, lBusinessSegmentLkup, lWorkBasket,null,out List<DOGEN_Queue> lstDOGEN_Queue, out string strErrorMessage);
            //check result for DB action
            if (result != ExceptionTypes.Success)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Home, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
            }
            return lstDOGEN_Queue;
        }

        [HttpPost]
        public ActionResult GetHomeReportURL()
        {
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.HomePageReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
            return Json(new { Data = urlData, JsonRequestBehavior.AllowGet });
        }

        private List<MostRecentItem> GetMostRecentItems(long aDM_UserMasterId,long workBasketlkup,long bussinessSegment)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            List<MostRecentItem> lstMostRecentItem = new List<MostRecentItem>();
            ExceptionTypes result;
            string errorMsg = string.Empty;
            try
            {
                result = _objBLQueueSummary.GetMostRecentItems(TimeZone,aDM_UserMasterId, workBasketlkup, bussinessSegment, out lstMostRecentItem, out errorMsg);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Home, (long)ExceptionTypes.Uncategorized, errorMsg, errorMsg);
                }
                if (lstMostRecentItem.Count > 0)
                {
                    EditRecordVisibility(ref lstMostRecentItem);
                }
                return lstMostRecentItem;

            }
            catch (Exception)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Home, (long)ExceptionTypes.Uncategorized, errorMsg, errorMsg);
                throw;
            }
        }

        private void SetSegmentVisibility(ref QueueSummary objQueueSummary)
        {
            objQueueSummary.ShowOSTQueueSummary = (currentUser.RoleLkup == (long)RoleLkup.Admin || currentUser.RoleLkup == (long)RoleLkup.Manager || currentUser.WorkBasketLkup == (long)WorkBasket.OST) && currentUser.ADM_UserPreference.ShowOSTSummary;
            objQueueSummary.ShowEligQueueSummary = (currentUser.RoleLkup == (long)RoleLkup.Admin || currentUser.RoleLkup == (long)RoleLkup.Manager || currentUser.WorkBasketLkup == (long)WorkBasket.GPSvsMMR) && currentUser.ADM_UserPreference.ShowEligibilitySummary;
            objQueueSummary.ShowRPRQueueSummary = (currentUser.RoleLkup == (long)RoleLkup.Admin || currentUser.RoleLkup == (long)RoleLkup.Manager || currentUser.WorkBasketLkup == (long)WorkBasket.RPR) && currentUser.ADM_UserPreference.ShowRPRSummary;
            objQueueSummary.ShowAlerts = currentUser.ADM_UserPreference.ShowAlerts;
            objQueueSummary.ShowResources = currentUser.ADM_UserPreference.ShowResources;
        }



        /// <summary>
        /// EditRecordVisibility
        /// </summary>
        /// <param name="lstSearchResults"></param>
        private void EditRecordVisibility(ref List<MostRecentItem> lstMostRecentItem)
        {
            foreach (var item in lstMostRecentItem)
            {
                item.EncryptedCaseID = URLEncoderDecoder.Encode(item.Gen_QueueId.NullToString());
                if (CheckEditButtonVisibility(item.QueueProgressTypeLkup, item.MostRecentWorkQueueLkup, item.AssignedToRef,item.PendedByRef, item.OOALetterStatusLkup, item.CMSTransactionStatusLkup))
                    item.EditActionVisibility = true;

            }
        }
        /// <summary>
        /// EditButtonVisibility
        /// </summary>
        /// <param name="QueueProgressTypeLkup"></param>
        /// <param name="MostRecentQueueLkup"></param>
        /// <param name="assignToRef"></param>
        /// <param name="OOALetterStatusLkup"></param>
        /// <param name="CMSTransactionStatusLkup"></param>
        /// <returns></returns>
        public bool CheckEditButtonVisibility(long? QueueProgressTypeLkup, long? MostRecentQueueLkup, long? assignToRef, long? PendedByRef,long? OOALetterStatusLkup, long? CMSTransactionStatusLkup)
        {
            try
            {
                if (QueueProgressTypeLkup == QueueProgressType.Processing.ToInt64() && !(objpendingQueues.Contains(MostRecentQueueLkup.ToInt64()))
                                                                                    && !(OOALetterStatusLkup.ToLong() == 53001 && MostRecentQueueLkup.ToInt64() == 10007)
                                                                                    && CMSTransactionStatusLkup.ToLong() != 55001
                                                                                    && (assignToRef.IsNull() || assignToRef == currentUser.ADM_UserMasterId))
                    return true;
                else if (QueueProgressTypeLkup == QueueProgressType.Processing.ToInt64() && (objpendingQueues.Contains(MostRecentQueueLkup.ToInt64()) && PendedByRef == currentUser.ADM_UserMasterId))//Queue in Proceesing and pending pending
                    return true;
                else if (QueueProgressTypeLkup == QueueProgressType.Holding.ToInt64() && objOSTHoldingQueues.Contains(MostRecentQueueLkup.ToInt64()))
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}