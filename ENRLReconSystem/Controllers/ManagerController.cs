using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Data;

namespace ENRLReconSystem.Controllers
{
    [ERSRoleAuthorize(ERSAuthenticationRoles.MgrEligUser, ERSAuthenticationRoles.MgrOSTUser, ERSAuthenticationRoles.MgrRPRUser, ERSAuthenticationRoles.AdmEligUser, ERSAuthenticationRoles.AdmOSTUser, ERSAuthenticationRoles.AdmRPRUser)]
    [ValidateInput(false)]
    public class ManagerController : Controller
    {
        string errorMessage = string.Empty;
        BLCommon _objBLCommon = new BLCommon();
        private UIUserLogin currentUser;
        public ManagerController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }

        [HttpGet]
        public ActionResult Unlock()
        {
            try
            {
                ViewBag.SearchPage = "Unlock Search";
                LoadDropDownData();
                ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;
                UISearch objUISearch = new UISearch();
                objUISearch.SearchCriteria = new SearchCriteria();
                objUISearch.SearchCriteria.WorkBasketLkup = (long)currentUser.WorkBasketLkup;
                objUISearch.UnlockSearchPanel = new List<UnlockSearchResults>();
                ViewBag.pageName = "Unlock Search";
                return View(objUISearch);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Unlock, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }
        [HttpPost]
        public ActionResult SearchUnlockRecords(SearchCriteria objSearchCriteria)
        {
            LoadDropDownData();
            UISearch objUISearch = new UISearch();
            ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;
            try
            {
                objSearchCriteria.IsUnlock = true;
                if (objSearchCriteria.WorkItemId == null &&
                    objSearchCriteria.DiscrepancyStartDate == DateTime.MinValue &&
                    objSearchCriteria.DiscrepancyEndDate == DateTime.MinValue &&
                    objSearchCriteria.CurrentHICN == null &&
                    objSearchCriteria.DiscrepancyCategoryLkup == null &&
                    objSearchCriteria.DiscrepancyTypeLkup == null
                    )
                {
                    objUISearch.SearchCriteria = new SearchCriteria();
                    objUISearch.SearchPanel = new List<SearchResults>();
                    ViewBag.Error = "To perform a search, you must enter one of the following:\n - Work ItemID\n - DiscrepancyStartDate\n - DiscrepancyEndDate\n - CurrentHICN \n - DiscrepancyCategory \n - DiscrepancyType";
                }
                else
                {
                    List<UnlockSearchResults> lstSearchResults;
                    objSearchCriteria.BusinessSegmentLkup = currentUser.BusinessSegmentLkup;
                    objSearchCriteria.WorkBasketLkup = currentUser.WorkBasketLkup;
                    objSearchCriteria.DiscrepancyStartDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyStartDate);
                    objSearchCriteria.DiscrepancyEndDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyEndDate);
                    objSearchCriteria.DOBId = ConvertDateToLong(objSearchCriteria.DOB);
                    objSearchCriteria.FirstLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailStartDate);
                    objSearchCriteria.FirstLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailEndDate);
                    objSearchCriteria.SecondLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailStartDate);
                    objSearchCriteria.SecondLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailEndDate);
                    objSearchCriteria.ComplianceStartDateId = ConvertDateToLong(objSearchCriteria.ComplianceStartDate);
                    objSearchCriteria.ComplianceEndDateId = ConvertDateToLong(objSearchCriteria.ComplianceEndDate);
                    objSearchCriteria.CaseCreationStartDateId = ConvertDateToLong(objSearchCriteria.CaseCreationStartDate);
                    objSearchCriteria.CaseCreationEndDateId = ConvertDateToLong(objSearchCriteria.CaseCreationEndDate);
                    objSearchCriteria.LastUpdatedStartDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedStartDate);
                    objSearchCriteria.LastUpdatedEndDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedEndDate);
                    objSearchCriteria.MemberResponseVerificationStartDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationStartDate);
                    objSearchCriteria.MemberResponseVerificationEndDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationEndDate);
                    objSearchCriteria.RequestedEffectiveStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
                    objSearchCriteria.RequestedEffectiveEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
                    objSearchCriteria.PotentionSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
                    objSearchCriteria.PotentionSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
                    objSearchCriteria.AdjustedCreateStartDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateStartDate);
                    objSearchCriteria.AdjustedCreateEndDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateEndDate);
                    objSearchCriteria.RPCSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionStartDate);
                    objSearchCriteria.RPCSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionEndDate);
                    objSearchCriteria.CMSAccountManagerApprovalStartDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalStartDate);
                    objSearchCriteria.CMSAccountManagerApprovalEndDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalEndDate);
                    objSearchCriteria.FDRReceivedStartDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedStartDate);
                    objSearchCriteria.FDRReceivedEndDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedEndDate);
                    objSearchCriteria.PeerAuditCompletionStartDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionStartDate);
                    objSearchCriteria.PeerAuditCompletionEndDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionEndDate);
                    objSearchCriteria.DisenrollmentFromDateId = ConvertDateToLong(objSearchCriteria.DisenrollmentFromDate);
                    objSearchCriteria.DisenrollmentToDateId = ConvertDateToLong(objSearchCriteria.DisenrollmentToDate);
                    objSearchCriteria.IsRestricted = currentUser.IsRestrictedUser;
                    long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                    BLCommon objBLCommon = new BLCommon();
                    ExceptionTypes result = objBLCommon.SearchRecordsToUnlock(TimeZone, (long)currentUser.WorkBasketLkup, currentUser.ADM_UserMasterId, objSearchCriteria, out lstSearchResults);
                    if (result != (long)ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Unlock, (long)ExceptionTypes.Uncategorized, string.Empty, "Error occured while fetching search records");
                        return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:Error occured while fetching search records." });
                    }

                    objUISearch.SearchCriteria = objSearchCriteria;
                    if (lstSearchResults != null && lstSearchResults.Count > 0)
                        objUISearch.UnlockSearchPanel = lstSearchResults;
                    else
                        objUISearch.UnlockSearchPanel = new List<UnlockSearchResults>();
                }

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Unlock, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
            }
            return PartialView("~/Views/Manager/_UnlockSearchResults.cshtml", objUISearch.UnlockSearchPanel);
        }

        [HttpGet]
        public ActionResult ReAssign()
        {
            try
            {
                ViewBag.SearchPage = "Reassign Search";
                ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;
                LoadDropDownData();
                UISearch objUISearch = new UISearch();
                objUISearch.SearchCriteria = new SearchCriteria();
                objUISearch.SearchCriteria.WorkBasketLkup = (long)currentUser.WorkBasketLkup;
                objUISearch.UnlockSearchPanel = new List<UnlockSearchResults>();
                ViewBag.pageName = "Reassign Search";
                return View(objUISearch);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Reassign, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        [HttpPost]
        public ActionResult ReAssign(SearchCriteria objSearchCriteria)
        {
            LoadDropDownData();
            UISearch objUISearch = new UISearch();

            try
            {
                ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;
                objSearchCriteria.IsReAssign = true;
                if (objSearchCriteria.WorkItemId == null &&
                    objSearchCriteria.DiscrepancyStartDate == DateTime.MinValue &&
                    objSearchCriteria.DiscrepancyEndDate == DateTime.MinValue &&
                    objSearchCriteria.CurrentHICN == null &&
                    objSearchCriteria.DiscrepancyCategoryLkup == null &&
                    objSearchCriteria.DiscrepancyTypeLkup == null
                    )
                {
                    objUISearch.SearchCriteria = new SearchCriteria();
                    objUISearch.SearchPanel = new List<SearchResults>();
                    ViewBag.Error = "To perform a search, you must enter one of the following:\n - Work ItemID\n - DiscrepancyStartDate\n - DiscrepancyEndDate\n - CurrentHICN \n - DiscrepancyCategory \n - DiscrepancyType";
                }
                else
                {
                    List<UnlockSearchResults> lstSearchResults;
                    objSearchCriteria.BusinessSegmentLkup = currentUser.BusinessSegmentLkup;
                    objSearchCriteria.WorkBasketLkup = currentUser.WorkBasketLkup;
                    objSearchCriteria.DiscrepancyStartDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyStartDate);
                    objSearchCriteria.DiscrepancyEndDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyEndDate);
                    objSearchCriteria.DOBId = ConvertDateToLong(objSearchCriteria.DOB);
                    objSearchCriteria.FirstLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailStartDate);
                    objSearchCriteria.FirstLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailEndDate);
                    objSearchCriteria.SecondLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailStartDate);
                    objSearchCriteria.SecondLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailEndDate);
                    objSearchCriteria.ComplianceStartDateId = ConvertDateToLong(objSearchCriteria.ComplianceStartDate);
                    objSearchCriteria.ComplianceEndDateId = ConvertDateToLong(objSearchCriteria.ComplianceEndDate);
                    objSearchCriteria.CaseCreationStartDateId = ConvertDateToLong(objSearchCriteria.CaseCreationStartDate);
                    objSearchCriteria.CaseCreationEndDateId = ConvertDateToLong(objSearchCriteria.CaseCreationEndDate);
                    objSearchCriteria.LastUpdatedStartDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedStartDate);
                    objSearchCriteria.LastUpdatedEndDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedEndDate);
                    objSearchCriteria.MemberResponseVerificationStartDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationStartDate);
                    objSearchCriteria.MemberResponseVerificationEndDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationEndDate);
                    objSearchCriteria.RequestedEffectiveStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
                    objSearchCriteria.RequestedEffectiveEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
                    objSearchCriteria.PotentionSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
                    objSearchCriteria.PotentionSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
                    objSearchCriteria.AdjustedCreateStartDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateStartDate);
                    objSearchCriteria.AdjustedCreateEndDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateEndDate);
                    objSearchCriteria.RPCSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionStartDate);
                    objSearchCriteria.RPCSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionEndDate);
                    objSearchCriteria.CMSAccountManagerApprovalStartDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalStartDate);
                    objSearchCriteria.CMSAccountManagerApprovalEndDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalEndDate);
                    objSearchCriteria.FDRReceivedStartDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedStartDate);
                    objSearchCriteria.FDRReceivedEndDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedEndDate);
                    objSearchCriteria.PeerAuditCompletionStartDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionStartDate);
                    objSearchCriteria.PeerAuditCompletionEndDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionEndDate);
                    objSearchCriteria.DisenrollmentFromDateId = ConvertDateToLong(objSearchCriteria.DisenrollmentFromDate);
                    objSearchCriteria.DisenrollmentToDateId = ConvertDateToLong(objSearchCriteria.DisenrollmentToDate);
                    objSearchCriteria.IsRestricted = currentUser.IsRestrictedUser;
                    long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                    BLCommon objBLCommon = new BLCommon();
                    ExceptionTypes result = objBLCommon.SearchRecordsToReassign(TimeZone, (long)currentUser.WorkBasketLkup, currentUser.ADM_UserMasterId, objSearchCriteria, out lstSearchResults);
                    if (result != (long)ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Reassign, (long)ExceptionTypes.Uncategorized, string.Empty, "Error occured while fetching search records");
                        return null;
                    }

                    objUISearch.SearchCriteria = objSearchCriteria;
                    if (lstSearchResults != null && lstSearchResults.Count > 0)
                        objUISearch.UnlockSearchPanel = lstSearchResults;
                    else
                        objUISearch.UnlockSearchPanel = new List<UnlockSearchResults>();
                }

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Reassign, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
            }
            return PartialView("_ReAssignSearchResults", objUISearch.UnlockSearchPanel);
        }
        /// <summary>
        /// Get data for Initial load
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MassUpdate()
        {
            try
            {
                ViewBag.SearchPage = "Mass Update Search";
                ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;
                LoadDropDownData();
                UISearch objUISearch = new UISearch();
                objUISearch.SearchCriteria = new SearchCriteria();
                objUISearch.SearchPanel = new List<SearchResults>();
                objUISearch.SearchCriteria.WorkBasketLkup = (long)currentUser.WorkBasketLkup;
                ViewBag.pageName = "Mass Update Search";
                objUISearch.workBasketLkup = currentUser.WorkBasketLkup;
                return View(objUISearch);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }
        /// <summary>
        /// Mass Update Search Result
        /// </summary>
        /// <param name="objSearchCriteria"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MassUpdate(SearchCriteria objSearchCriteria)
        {
            LoadDropDownData();
            UISearch objUISearch = new UISearch();
            List<SearchResults> lstSearchResult = new List<SearchResults>();
            string totalCount = string.Empty;
            string errorMessage = string.Empty;

            try
            {
                ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;
                objSearchCriteria.IsMassUpdate = true;
                if (objSearchCriteria.WorkItemId == null &&
                    objSearchCriteria.DiscrepancyStartDate == DateTime.MinValue &&
                    objSearchCriteria.DiscrepancyEndDate == DateTime.MinValue &&
                    objSearchCriteria.CurrentHICN == null &&
                    objSearchCriteria.DiscrepancyCategoryLkup == null &&
                    objSearchCriteria.DiscrepancyTypeLkup == null
                    )
                {
                    objUISearch.SearchCriteria = new SearchCriteria();
                    objUISearch.SearchPanel = new List<SearchResults>();
                    ViewBag.Error = "To perform a search, you must enter one of the following:\n - Work ItemID\n - DiscrepancyStartDate\n - DiscrepancyEndDate\n - CurrentHICN \n - DiscrepancyCategory \n - DiscrepancyType";
                }
                else
                {

                    objSearchCriteria.BusinessSegmentLkup = currentUser.BusinessSegmentLkup;
                    objSearchCriteria.WorkBasketLkup = currentUser.WorkBasketLkup;
                    objSearchCriteria.DiscrepancyStartDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyStartDate);
                    objSearchCriteria.DiscrepancyEndDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyEndDate);
                    objSearchCriteria.DOBId = ConvertDateToLong(objSearchCriteria.DOB);
                    objSearchCriteria.FirstLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailStartDate);
                    objSearchCriteria.FirstLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailEndDate);
                    objSearchCriteria.SecondLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailStartDate);
                    objSearchCriteria.SecondLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailEndDate);
                    objSearchCriteria.ComplianceStartDateId = ConvertDateToLong(objSearchCriteria.ComplianceStartDate);
                    objSearchCriteria.ComplianceEndDateId = ConvertDateToLong(objSearchCriteria.ComplianceEndDate);
                    objSearchCriteria.CaseCreationStartDateId = ConvertDateToLong(objSearchCriteria.CaseCreationStartDate);
                    objSearchCriteria.CaseCreationEndDateId = ConvertDateToLong(objSearchCriteria.CaseCreationEndDate);
                    objSearchCriteria.LastUpdatedStartDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedStartDate);
                    objSearchCriteria.LastUpdatedEndDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedEndDate);
                    objSearchCriteria.MemberResponseVerificationStartDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationStartDate);
                    objSearchCriteria.MemberResponseVerificationEndDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationEndDate);
                    objSearchCriteria.RequestedEffectiveStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
                    objSearchCriteria.RequestedEffectiveEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
                    objSearchCriteria.PotentionSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
                    objSearchCriteria.PotentionSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
                    objSearchCriteria.AdjustedCreateStartDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateStartDate);
                    objSearchCriteria.AdjustedCreateEndDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateEndDate);
                    objSearchCriteria.RPCSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionStartDate);
                    objSearchCriteria.RPCSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionEndDate);
                    objSearchCriteria.CMSAccountManagerApprovalStartDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalStartDate);
                    objSearchCriteria.CMSAccountManagerApprovalEndDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalEndDate);
                    objSearchCriteria.FDRReceivedStartDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedStartDate);
                    objSearchCriteria.FDRReceivedEndDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedEndDate);
                    objSearchCriteria.PeerAuditCompletionStartDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionStartDate);
                    objSearchCriteria.PeerAuditCompletionEndDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionEndDate);
                    objSearchCriteria.DisenrollmentFromDateId = ConvertDateToLong(objSearchCriteria.DisenrollmentFromDate);
                    objSearchCriteria.DisenrollmentToDateId = ConvertDateToLong(objSearchCriteria.DisenrollmentToDate);
                    objSearchCriteria.IsRestricted = currentUser.IsRestrictedUser;
                    long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                    ExceptionTypes result = _objBLCommon.SearchRecords(TimeZone, (long)currentUser.WorkBasketLkup, currentUser.ADM_UserMasterId, ConstantTexts.DefaultCount, objSearchCriteria, out lstSearchResult, out totalCount, out errorMessage);
                    if (result != (long)ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                        return null;
                    }

                    objUISearch.SearchCriteria = objSearchCriteria;
                    objUISearch.SearchPanel = lstSearchResult;
                }

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
            }
            return PartialView("_MassUpdateSearchResultsPanel", objUISearch.SearchPanel);
        }

        /// <summary>
        /// Used to unlock the record.
        /// </summary>
        /// <param name="objDOGEN_ManageCases"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult UnlockRecord(string Gen_QueueIds, string CasesComments)
        {
            try
            {
                string errorMessage;
                ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;
                BLCommon objBLCommon = new BLCommon();
                ExceptionTypes result = objBLCommon.UnlockQueueRecord(Gen_QueueIds, currentUser.ADM_UserMasterId, (long)ActionLookup.Unlock, CasesComments, out errorMessage);
                if (result == (long)ExceptionTypes.Success)
                {
                    return new JsonResult() { Data = true };
                }
                else if (result == ExceptionTypes.UnknownError)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Unlock, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Unlock, (long)ExceptionTypes.Uncategorized, "Error while retriving users from Data base", "Error:" + ex.ToString());
            }
            return new JsonResult() { Data = false };
        }

        /// <summary>
        /// Used to reassign the record.
        /// </summary>
        /// <param name="id">List<long></long></param>
        /// <param name="data">DOGEN_ManageCases</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult BulkReassignRecord(UIBulkAssign objUIBulkAssign)
        {
            try
            {
                string errorMessage;
                ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;
                objUIBulkAssign.DOGEN_ManageCases.CurrentUserRef = currentUser.ADM_UserMasterId;
                objUIBulkAssign.DOGEN_ManageCases.IsActive = true;
                objUIBulkAssign.DOGEN_ManageCases.ActionPerformedLkup = (long)ActionLookup.ReAssign;
                BLCommon objBLCommon = new BLCommon();
                ExceptionTypes result = objBLCommon.BulkReassignQueueRecord(objUIBulkAssign, out errorMessage);
                if (result == (long)ExceptionTypes.Success)
                {
                    return new JsonResult() { Data = true };
                }
                else if (result == ExceptionTypes.UnknownError)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Reassign, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Reassign, (long)ExceptionTypes.Uncategorized, "Error while retriving users from Data base", "Error:" + ex.ToString());
            }
            return new JsonResult() { Data = false };
        }

        /// <summary>
        /// Used to verify user reassign permission
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckUserBulkReassignPermission()
        {
            bool canReAssign = false;
            try
            {
                if (currentUser.WorkBasketLkup == (long)WorkBasket.OST)
                {
                    canReAssign = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReassign, (long)DiscripancyCategory.OOA);
                    if (!canReAssign)
                        canReAssign = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReassign, (long)DiscripancyCategory.SCC);
                    if (!canReAssign)
                        canReAssign = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReassign, (long)DiscripancyCategory.TRR);
                }
                else if (currentUser.WorkBasketLkup == (long)WorkBasket.GPSvsMMR)
                {
                    canReAssign = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReassign, (long)DiscripancyCategory.Eligibility);
                    if (!canReAssign)
                        canReAssign = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReassign, (long)DiscripancyCategory.DOB);
                    if (!canReAssign)
                        canReAssign = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReassign, (long)DiscripancyCategory.Gender);
                }
                else if (currentUser.WorkBasketLkup == (long)WorkBasket.RPR)
                {
                    canReAssign = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReassign, (long)DiscripancyCategory.RPR);
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Reassign, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
            }
            return new JsonResult { Data = canReAssign };
        }

        /// <summary>
        /// Used to verify user MassUpdate permission
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckUserMassUpdatePermission(long DisCat)
        {
            bool CanMassUpdate = false;
            try
            {
                if (currentUser.WorkBasketLkup == (long)WorkBasket.OST)
                {
                    if (DisCat == (long)DiscripancyCategory.OOA)
                    {
                        CanMassUpdate = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanMassUpdate, (long)DiscripancyCategory.OOA);
                    }
                    else if (DisCat == (long)DiscripancyCategory.SCC)
                    {
                        CanMassUpdate = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanMassUpdate, (long)DiscripancyCategory.SCC);
                    }
                    else if (DisCat == (long)DiscripancyCategory.TRR)
                    {
                        CanMassUpdate = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanMassUpdate, (long)DiscripancyCategory.TRR);
                    }
                }
                else if (currentUser.WorkBasketLkup == (long)WorkBasket.GPSvsMMR)
                {
                    if (DisCat == (long)DiscripancyCategory.Eligibility)
                    {
                        CanMassUpdate = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanMassUpdate, (long)DiscripancyCategory.Eligibility);
                    }

                    else if (DisCat == (long)DiscripancyCategory.DOB)
                    {
                        CanMassUpdate = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanMassUpdate, (long)DiscripancyCategory.DOB);
                    }

                    else if (DisCat == (long)DiscripancyCategory.Gender)
                    {
                        CanMassUpdate = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanMassUpdate, (long)DiscripancyCategory.Gender);
                    }

                }
                else if (currentUser.WorkBasketLkup == (long)WorkBasket.RPR)
                {
                    if (DisCat == (long)DiscripancyCategory.RPR)
                    {
                        CanMassUpdate = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanMassUpdate, (long)DiscripancyCategory.RPR);
                    }
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, "Check User Mass Update Permission", ex.ToString());
            }
            return new JsonResult { Data = CanMassUpdate };
        }

        /// <summary>
        /// Used to verify user Unlock the record
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckUserUnlockPermission()
        {
            bool CanUnlock = false;
            try
            {
                if (currentUser.WorkBasketLkup == (long)WorkBasket.OST)
                {
                    CanUnlock = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanUnlock, (long)DiscripancyCategory.OOA);
                    if (!CanUnlock)
                        CanUnlock = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanUnlock, (long)DiscripancyCategory.SCC);
                    if (!CanUnlock)
                        CanUnlock = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanUnlock, (long)DiscripancyCategory.TRR);
                }
                else if (currentUser.WorkBasketLkup == (long)WorkBasket.GPSvsMMR)
                {
                    CanUnlock = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanUnlock, (long)DiscripancyCategory.Eligibility);
                    if (!CanUnlock)
                        CanUnlock = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanUnlock, (long)DiscripancyCategory.DOB);
                    if (!CanUnlock)
                        CanUnlock = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanUnlock, (long)DiscripancyCategory.Gender);
                }
                else if (currentUser.WorkBasketLkup == (long)WorkBasket.RPR)
                {
                    CanUnlock = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanUnlock, (long)DiscripancyCategory.RPR);
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, "Check User Mass Update Permission", ex.ToString());
            }
            return new JsonResult { Data = CanUnlock };
        }

        /// <summary>
        /// Used to get permitted user list to reassign records.
        /// </summary>
        /// <param name="discCatIds">List<long></long></param>
        public ActionResult ReassignUserList(string Gen_QueueIds)
        {
            try
            {
                List<DOADM_UserMaster> lstDOADM_UserMaster = new List<DOADM_UserMaster>();
                ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;
                string errorMessage = string.Empty;
                BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                ExceptionTypes result = objBLUserAdministration.ReassignUserList(TimeZone, Gen_QueueIds, out lstDOADM_UserMaster, out errorMessage);
                if (result != (long)ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Reassign, (long)ExceptionTypes.Uncategorized, "Error occured while fetching reassign user list", errorMessage);
                }
                //ViewBag.ReAssignedUserList = lstDOADM_UserMaster;
                ViewBag.BulkReAssignedUserList = lstDOADM_UserMaster;
                return PartialView("_BulkAssignUsers");
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Reassign, (long)ExceptionTypes.Uncategorized, ex.ToString(), "Error:" + ex.ToString());
            }
            return null;
        }


        public JsonResult LoadQueue(long categoryID)
        {

            List<DOCMN_LookupMasterCorrelations> lstQueue = new List<DOCMN_LookupMasterCorrelations>();
            try
            {
                lstQueue = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, categoryID)
                                       .Where(xx => xx.IsActive == true
                                       && xx.GroupingLookupMasterRef == QueueProgressType.Processing.ToInt64()
                                       || Enum.GetValues(typeof(RequiredQueueList)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();
                //removing pended Queues
                lstQueue = lstQueue.Where(xx => !Enum.GetValues(typeof(PendingQueues)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();
                return Json(lstQueue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, ex.ToString(), "Error:" + ex.ToString());
            }
            return null;
        }
        public ActionResult OSTActionUpdate(DOGEN_OSTActions objDOGEN_OSTActions)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = string.Empty;
            try
            {
                objDOGEN_OSTActions.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                objDOGEN_OSTActions.RoleLkup = currentUser.RoleLkup;
                objDOGEN_OSTActions.BusinessSegmentLkup = currentUser.BusinessSegmentLkup;
                objDOGEN_OSTActions.CommentsSourceSystemLkup = (long)SourceSystemLkup.ERS;
                returnMessage = "Record updated successfully.";

                result = _objBLCommon.OSTActionUpdate(objDOGEN_OSTActions, out errorMessage);

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = "An error occured." });
                }
                return Json(new { ID = result, Message = returnMessage });

            }
            catch (Exception ex)
            {

                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return Json(new { ID = result, Message = "An error occured." });
            }

        }
        public ActionResult EligibilityActionUpdate(DOGEN_EligibilityActions objDOGEN_EligibilityActions)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = string.Empty;
            try
            {
                objDOGEN_EligibilityActions.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                objDOGEN_EligibilityActions.LoginID = currentUser.ADM_UserMasterId;
                objDOGEN_EligibilityActions.BusinessSegmentLkup = currentUser.BusinessSegmentLkup;
                objDOGEN_EligibilityActions.RoleLKup = currentUser.RoleLkup;
                objDOGEN_EligibilityActions.CommentsSourceSystemLkup = (long)SourceSystemLkup.ERS;
                returnMessage = "Record updated successfully.";

                result = _objBLCommon.EligibilityActionUpdate(objDOGEN_EligibilityActions, out errorMessage);

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = "An error occured." });
                }
                return Json(new { ID = result, Message = returnMessage });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return Json(new { ID = result, Message = "An error occured." });
            }

        }
        public ActionResult RPRActionUpdate(DOGEN_RPRActions objDOGEN_RPRActions)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = string.Empty;
            try
            {
                objDOGEN_RPRActions.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                objDOGEN_RPRActions.BusinessSegmentLkup = currentUser.BusinessSegmentLkup;
                objDOGEN_RPRActions.LoginUserId = currentUser.ADM_UserMasterId;
                objDOGEN_RPRActions.RoleLkup = currentUser.RoleLkup;
                objDOGEN_RPRActions.CommentsSourceSystemLkup = (long)SourceSystemLkup.ERS;
                returnMessage = "Record updated successfully.";

                result = _objBLCommon.RPRActionUpdate(objDOGEN_RPRActions, out errorMessage);

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = "An error occured." });
                }
                return Json(new { ID = result, Message = returnMessage });

            }
            catch (Exception ex)
            {

                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return Json(new { ID = result, Message = "An error occured." });
            }

        }

        /// <summary>
        /// Used to convert date into long dateId value.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private long ConvertDateToLong(DateTime? date)
        {
            long retVal = 0;
            try
            {
                if (date != null && date != DateTime.MinValue)
                {
                    string strId = date.Value.ToString("yyyyMMdd");
                    if (long.TryParse(strId, out retVal) == false)
                    {
                        retVal = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        /// <summary>
        /// Used to load all drop down default data.
        /// </summary>
        private void LoadDropDownData()
        {
            BLUserAdministration objBLUserAdministration = new BLUserAdministration();
            DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
            objDOADM_UserMaster.IsActive = true;
            List<DOADM_UserMaster> lstUsers = new List<DOADM_UserMaster>();
            string errorMessage;
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            ExceptionTypes result = objBLUserAdministration.SearchUser(TimeZone, objDOADM_UserMaster, out lstUsers, out errorMessage);
            if (result != (long)ExceptionTypes.Success)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, "Error while retriving users from Data base", "Error while retriving users from Data base");
            }
            //Loading Disccategory as per Business Segment
            List<DOCMN_LookupMaster> lstDOCMN_LMDiscCat = new List<DOCMN_LookupMaster>();
            List<DOCMN_LookupMaster> lstDiscripancyCategory = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.DiscripancyCategory);
            var lookupCorrelations = CacheUtility.GetAllLookupMasterCorrelationFromCache(null, (long)currentUser.WorkBasketLkup).ToList();
            lookupCorrelations.Select(x => x.CMN_LookupMasterChildRef).Distinct().ToList().ForEach(CMN_LookupMasterChildRef =>
            {
                if (CMN_LookupMasterChildRef > 0)
                {
                    lstDOCMN_LMDiscCat.Add(lstDiscripancyCategory.Where(x => x.CMN_LookupMasterId.Equals(CMN_LookupMasterChildRef)).FirstOrDefault());
                }
            });
            ViewBag.DiscrepancyCategory = lstDOCMN_LMDiscCat;

            //Loading Discrepancy Type as per Discrepancy Category
            List<DOCMN_LookupMaster> lstDOCMN_LMDiscCatType = new List<DOCMN_LookupMaster>();
            List<DOCMN_LookupMaster> lstDiscripancyType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.DiscripancyType);
            lstDOCMN_LMDiscCat.Select(x => x.CMN_LookupMasterId).Distinct().ToList().ForEach(CMN_LookupMasterId =>
            {
                lookupCorrelations = CacheUtility.GetAllLookupMasterCorrelationFromCache(null, CMN_LookupMasterId).ToList();
                lookupCorrelations.Select(x => x.CMN_LookupMasterChildRef).Distinct().ToList().ForEach(CMN_LookupMasterChildRef =>
                {
                    if (CMN_LookupMasterChildRef > 0)
                    {
                        var item = lstDiscripancyType.Where(x => x.CMN_LookupMasterId.Equals(CMN_LookupMasterChildRef)).FirstOrDefault();
                        if (item != null && (lstDOCMN_LMDiscCatType.Contains(item) == false))
                            lstDOCMN_LMDiscCatType.Add(lstDiscripancyType.Where(x => x.CMN_LookupMasterId.Equals(CMN_LookupMasterChildRef)).FirstOrDefault());
                    }
                });
            });
            ViewBag.DiscrepancyType = lstDOCMN_LMDiscCatType;


            //Loading Queues as per Discrepancy Category
            List<DOCMN_LookupMasterCorrelations> lstDOCMN_LMTest = new List<DOCMN_LookupMasterCorrelations>();
            List<DOCMN_LookupMaster> lstQueue = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Queue);
            lstDOCMN_LMTest = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, 0).ToList();
            ViewBag.Queue = lstDOCMN_LMTest;

            ViewBag.DiscrepancySource = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.DiscrepancySource);
            ViewBag.ContractNumber = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
            ViewBag.PBP = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
            ViewBag.LOB = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);           
            ViewBag.RPRActionRequested = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RPRActionRequested);
            ViewBag.FDRStatus = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.FDRStatus);
            ViewBag.SubmissionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.SubmissionType);
            ViewBag.PendReason = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PendReason);
            ViewBag.Resolution = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Resolution);
            ViewBag.Status = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Status);
            ViewBag.Gender = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Gender);
            ViewBag.RPREGHPMember = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
            ViewBag.VerifiedRootCause = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RootCause);
            ViewBag.PerformcedTaskList = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Taskbeingperformedwhenthisdiscrepancywasidentified);
            ViewBag.lstTransactionReplyCode = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.TransactionReplyCode);
            ViewBag.lstPDPAutoEnrolleeInd = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
            ViewBag.VerifiedState = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.State);
            if (lstUsers != null && lstUsers.Count() > 0)
            {
                lstUsers = lstUsers.Where(x => x.ADM_UserMasterId > 1000).ToList();
                ViewBag.LastUpdatedOperator = lstUsers;
                ViewBag.AssignedTo = lstUsers;
                ViewBag.SupervisiorList = lstUsers.Where(x => x.IsManager == true).ToList();
                ViewBag.RPRRequestor = lstUsers;
                ViewBag.ReAssignedUserList = lstUsers;
                ViewBag.BulkReAssignedUserList = lstUsers;
            }
        }

        [HttpPost]
        public ActionResult LoadActionsBasedOnQueue(long lQueueLkup)
        {
            UIMassUpdateExtended objUIMassUpdateExtended = new UIMassUpdateExtended();
            try
            {
                objUIMassUpdateExtended.lstActions = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.QueueVsAction, (long)lQueueLkup);
                if (objUIMassUpdateExtended.lstActions.Count > 0)
                {
                    objUIMassUpdateExtended.lstActions = objUIMassUpdateExtended.lstActions.Where(xx => !Enum.GetValues(typeof(RemoveActionForMassUpdate)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef)).ToList();
                }
                return PartialView("_MassUpdateActions", objUIMassUpdateExtended);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return null;
            }
        }


    }
}