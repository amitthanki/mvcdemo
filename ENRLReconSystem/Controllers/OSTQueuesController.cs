﻿using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Models;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ENRLReconSystem.Controllers
{
    [Common.Filter(WorkBasketLkup = (long)WorkBasket.OST)]
    public class OSTQueuesController : Controller
    {
        private UIUserLogin currentUser;
        public OSTQueuesController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }

        public ActionResult OOAGetQueue()
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            QueueSummary objQueueSummary;
            DateTime dtpStartDate = DateTime.UtcNow.AddDays(-90);
            DateTime dtpEndDate = DateTime.UtcNow;
            string strErrorMessage;
            try
            {
                objBLQueueSummary.GetQueueSummary(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup,(long)DiscripancyCategory.OOA, out objQueueSummary, out strErrorMessage);
                objQueueSummary.lstUserAccessQueueLkups = currentUser.UserQueueList.Select(item => item.QueueLkp).Distinct().ToList();
                objQueueSummary.StartDate = dtpStartDate;
                objQueueSummary.EndDate = dtpEndDate;
                objQueueSummary.BusinessSegment = currentUser.BusinessSegmentLkup;
                ViewBag.ReportUrl = GetReportUrl();
                return View(objQueueSummary);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        public ActionResult SCCGetQueue()
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            QueueSummary objQueueSummary = new QueueSummary();
            DateTime dtpStartDate = DateTime.UtcNow.AddDays(-90);
            DateTime dtpEndDate = DateTime.UtcNow;
            string strErrorMessage;
            try
            {
                objBLQueueSummary.GetQueueSummary(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup,(long)DiscripancyCategory.SCC, out objQueueSummary, out strErrorMessage);
                objQueueSummary.lstUserAccessQueueLkups = currentUser.UserQueueList.Select(item => item.QueueLkp).Distinct().ToList();
                objQueueSummary.StartDate = dtpStartDate;
                objQueueSummary.EndDate = dtpEndDate;
                objQueueSummary.BusinessSegment = currentUser.BusinessSegmentLkup;
                ViewBag.ReportUrl = GetReportUrl();
                return View(objQueueSummary);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

      

        public ActionResult TRRGetQueue()
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            QueueSummary objQueueSummary = new QueueSummary();
            DateTime dtpStartDate = DateTime.UtcNow.AddDays(-90);
            DateTime dtpEndDate = DateTime.UtcNow;
            string strErrorMessage;
            try
            {
                objBLQueueSummary.GetQueueSummary(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup,(long)DiscripancyCategory.TRR, out objQueueSummary, out strErrorMessage);
                objQueueSummary.lstUserAccessQueueLkups = currentUser.UserQueueList.Select(item => item.QueueLkp).Distinct().ToList();
                objQueueSummary.StartDate = dtpStartDate;
                objQueueSummary.EndDate = dtpEndDate;
                objQueueSummary.BusinessSegment = currentUser.BusinessSegmentLkup;
                ViewBag.ReportUrl = GetReportUrl();
                return View(objQueueSummary);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        [HttpPost]
        public ActionResult OOAGetQueue(DateTime dtpStartDate, DateTime dtpEndDate)
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            QueueSummary objQueueSummary;
            string strErrorMessage;
            try
            {
                objBLQueueSummary.GetQueueSummary(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup,(long)DiscripancyCategory.OOA, out objQueueSummary, out strErrorMessage);
                objQueueSummary.lstUserAccessQueueLkups = currentUser.UserQueueList.Select(item => item.QueueLkp).Distinct().ToList();
                objQueueSummary.StartDate = dtpStartDate;
                objQueueSummary.EndDate = dtpEndDate;

                ViewBag.ReportUrl = GetReportUrl();

                return PartialView("_GetOOAQueueCount", objQueueSummary);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
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

        [HttpPost]
        public ActionResult SCCGetQueue(DateTime dtpStartDate, DateTime dtpEndDate)
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            QueueSummary objQueueSummary;
            string strErrorMessage;
            try
            {
                objBLQueueSummary.GetQueueSummary(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup, (long)DiscripancyCategory.SCC, out objQueueSummary, out strErrorMessage);
                objQueueSummary.lstUserAccessQueueLkups = currentUser.UserQueueList.Select(item => item.QueueLkp).Distinct().ToList();
                objQueueSummary.StartDate = dtpStartDate;
                objQueueSummary.EndDate = dtpEndDate;
                ViewBag.ReportUrl = GetReportUrl();
                return PartialView("_GetSCCQueueCount", objQueueSummary);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        [HttpPost]
        public ActionResult TRRGetQueue(DateTime dtpStartDate, DateTime dtpEndDate)
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            QueueSummary objQueueSummary;
            string strErrorMessage;
            try
            {
                objBLQueueSummary.GetQueueSummary(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup, (long)DiscripancyCategory.TRR, out objQueueSummary, out strErrorMessage);
                objQueueSummary.lstUserAccessQueueLkups = currentUser.UserQueueList.Select(item => item.QueueLkp).Distinct().ToList();
                objQueueSummary.StartDate = dtpStartDate;
                objQueueSummary.EndDate = dtpEndDate;
                ViewBag.ReportUrl = GetReportUrl();
                return PartialView("_GetTRRQueueCount", objQueueSummary);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        public ActionResult GMURecord(DateTime dtpStartDate, DateTime dtpEndDate, long queueLkup, long? queueIdToSkip, string DiscrepancyCategory)
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
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
                        return RedirectToAction("Maintenance", "Error", new { Error = strErrorMessage });
                    }
                }

                int lockCount = 1;
                do
                {
                    ExceptionTypes dbresult = objBLQueueSummary.GetGMURecord(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup, queueLkup, queueIdToSkip, currentUser.ADM_UserMasterId, currentUser.IsRestrictedUser, out objDOGEN_Queue, out strErrorMessage);
                    if (dbresult == ExceptionTypes.Success)
                    {
                        if (objDOGEN_Queue.LockedByRef == currentUser.ADM_UserMasterId)
                            isForcedLock = true;
                        //Locking  the record.
                        UIRecordsLock objRecordsLocked;

                        ExceptionTypes exceptionResult = objCommon.GetLockedRecordOrLockRecord(currentUser.ADM_UserMasterId, (long)ScreenType.Queue, (long)objDOGEN_Queue.GEN_QueueId, isForcedLock, out objRecordsLocked);

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
                ViewBag.Name = DiscrepancyCategory;
                return PartialView("_GetOSTWorkItem", objDOGEN_Queue);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        public ActionResult GetPendedRecords(string DiscrepancyCategory)
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            List<DOGEN_Queue> lstDOGEN_Queue = new List<DOGEN_Queue>();
            string strErrorMessage;
            long lDiscrepancyCategoryLkup = 0;
            try
            {
                if (DiscrepancyCategory == DiscripancyCategory.OOA.ToString())
                {
                    ViewBag.Name = DiscripancyCategory.OOA.ToString();
                    lDiscrepancyCategoryLkup = (long)DiscripancyCategory.OOA;
                }
                if (DiscrepancyCategory == DiscripancyCategory.SCC.ToString())
                {
                    ViewBag.Name = DiscripancyCategory.SCC.ToString();
                    lDiscrepancyCategoryLkup = (long)DiscripancyCategory.SCC;
                }
                if (DiscrepancyCategory == DiscripancyCategory.TRR.ToString())
                {
                    ViewBag.Name = DiscripancyCategory.TRR.ToString();
                    lDiscrepancyCategoryLkup = (long)DiscripancyCategory.TRR;
                }
                ExceptionTypes result = objBLQueueSummary.GetPendedRecords(currentUser.ADM_UserMasterId, (long)currentUser.BusinessSegmentLkup, (long)currentUser.WorkBasketLkup,lDiscrepancyCategoryLkup, out lstDOGEN_Queue, out strErrorMessage);
                if (result != ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
                }
                ViewBag.Name = DiscrepancyCategory;
                return PartialView("_GetOSTPendingWorkItem", lstDOGEN_Queue);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }
    }
}