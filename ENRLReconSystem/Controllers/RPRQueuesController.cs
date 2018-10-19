﻿using ENRLReconSystem.BL;
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
    [Common.Filter(WorkBasketLkup = (long)WorkBasket.RPR)]
    public class RPRQueuesController : Controller
    {
        private UIUserLogin currentUser;
        public RPRQueuesController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            }
        }
        public ActionResult RPRGetQueue()
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            QueueSummary objQueueSummary;
            DateTime dtpStartDate = DateTime.UtcNow.AddDays(-90);
            DateTime dtpEndDate = DateTime.UtcNow;
            string strErrorMessage;
            try
            {
                ExceptionTypes result = objBLQueueSummary.GetQueueSummary(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup, (long)DiscripancyCategory.RPR, out objQueueSummary, out strErrorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRGetQueue, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
                }
                objQueueSummary.lstUserAccessQueueLkups = currentUser.UserQueueList.Select(item => item.QueueLkp).Distinct().ToList();
                objQueueSummary.StartDate = dtpStartDate;
                objQueueSummary.EndDate = dtpEndDate;
                objQueueSummary.BusinessSegment = currentUser.BusinessSegmentLkup;
                ViewBag.ReportUrl = GetReportUrl();
                return View(objQueueSummary);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRGetQueue, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        [HttpPost]
        public ActionResult RPRGetQueue(DateTime dtpStartDate, DateTime dtpEndDate)
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            QueueSummary objQueueSummary;
            string strErrorMessage;
            try
            {
                ExceptionTypes result = objBLQueueSummary.GetQueueSummary(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup, (long)DiscripancyCategory.RPR, out objQueueSummary, out strErrorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRGetQueue, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
                }
                objQueueSummary.StartDate = dtpStartDate;
                objQueueSummary.EndDate = dtpEndDate;
                objQueueSummary.lstUserAccessQueueLkups = currentUser.UserQueueList.Select(item => item.QueueLkp).Distinct().ToList();
                return PartialView("_GetRPRQueueCount", objQueueSummary);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRGetQueue, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        public ActionResult GMURecord(DateTime dtpStartDate, DateTime dtpEndDate, long queueLkup, long? queueIdToSkip)
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            BLCommon objCommon = new BLCommon();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            string strErrorMessage = string.Empty;
            bool isForcedLock = false;
            try
            {
                if (queueIdToSkip > 0)
                {
                    ExceptionTypes exceptionResult = objCommon.UnlockRecord((long)ScreenType.Queue, (long)queueIdToSkip);
                    if (exceptionResult != ExceptionTypes.Success)
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRGetQueue, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
                        return RedirectToAction("Maintenance", "Error", new { Error = strErrorMessage });
                    }
                }

                int lockCount = 1;
                do
                {
                    ExceptionTypes dbresult = objBLQueueSummary.GetGMURecord(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup, queueLkup, queueIdToSkip, currentUser.ADM_UserMasterId, currentUser.IsRestrictedUser, out objDOGEN_Queue, out strErrorMessage);
                    if (dbresult == ExceptionTypes.Success)
                    {
                        //Locking  the record.
                        if (objDOGEN_Queue.LockedByRef == currentUser.ADM_UserMasterId)
                            isForcedLock = true;
                        UIRecordsLock objRecordsLocked;
                        ExceptionTypes exceptionResult = objCommon.GetLockedRecordOrLockRecord(currentUser.ADM_UserMasterId, (long)ScreenType.Queue, (long)objDOGEN_Queue.GEN_QueueId, false, out objRecordsLocked);

                        if (exceptionResult == (long)ExceptionTypes.Success && objRecordsLocked != null)
                        {
                            if (objRecordsLocked.CreatedByRef != currentUser.ADM_UserMasterId)
                            {
                                lockCount = lockCount + 1;
                                continue;
                            }

                            lockCount = 11;
                        }
                        else
                        {
                            lockCount = lockCount + 1;
                        }
                    }
                    else
                    {
                        lockCount = lockCount + 1;
                    }
                } while (lockCount <= 10);

                return PartialView("_GetRPRWorkItem", objDOGEN_Queue);
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRGetQueue, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        public ActionResult GetPendedRecords()
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            List<DOGEN_Queue> lstDOGEN_Queue = new List<DOGEN_Queue>();
            string strErrorMessage;           
            try
            {
                ExceptionTypes result = objBLQueueSummary.GetPendedRecords(currentUser.ADM_UserMasterId, (long)currentUser.BusinessSegmentLkup, (long)currentUser.WorkBasketLkup, (long)DiscripancyCategory.RPR, out lstDOGEN_Queue, out strErrorMessage);
                if (result != ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRGetQueue, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
                }
                return PartialView("_GetRPRPendingWorkItem", lstDOGEN_Queue);
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRGetQueue, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }
        private string GetReportUrl()
        {
            BLReports _objBLReports = new BLReports();
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.GetQueueReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault() != null ? lstDORPT_ReportsMaster.FirstOrDefault().ReportURL : null;
            return urlData;
        }
    }
}