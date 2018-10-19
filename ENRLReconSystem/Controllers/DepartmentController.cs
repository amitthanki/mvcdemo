using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using ENRLReconSystem.Models;
using System.Reflection;
namespace ENRLReconSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private UIUserLogin currentUser;
       
        private BLReports _objBLReports = new BLReports();

        public DepartmentController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }
        // GET: Department
        [HttpGet]
        public ActionResult Search()
        {           
            return View(PGetDepartment());
        }
        
        public ActionResult Search(string strDepName = "", bool isActive =true)
        {            
            try
            {               
                return PartialView("_SearchResults", PGetDepartment(strDepName, isActive));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Department, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }
        // Check Duplicate Department and Business Segment.
        [HttpPost]
        public JsonResult CheckDuplicateDep(long BusinessSegment,long DepName)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            var data = "";
            BLDepartment objBLDepartment = new BLDepartment();
            List<DOCMN_Department> lstDOCMN_Department;
          
                DOCMN_Department objDOCMN_Department = new DOCMN_Department();
                objDOCMN_Department.BusinessSegmentLkup = BusinessSegment;
                objDOCMN_Department.DepartmentLkup = DepName;
               
                string errorMessage = string.Empty;
                ExceptionTypes result = objBLDepartment.CheckDuplicateDep(TimeZone, objDOCMN_Department, out lstDOCMN_Department, out errorMessage);
              
                if (result != (long)ExceptionTypes.Success)
                {
                //Log error
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Department, (long)ExceptionTypes.Uncategorized, errorMessage.ToString(), errorMessage.ToString());
                }
                else if (result == ExceptionTypes.ZeroRecords)
                {
                lstDOCMN_Department.Add(objDOCMN_Department);
                lstDOCMN_Department = new List<DOCMN_Department>();
                }
                  
                    int i = lstDOCMN_Department.Count;
                    if (i > 0)
                    {
                        data = "Department Name already exists.";
                    }
            return Json(new { Data = data }, JsonRequestBehavior.AllowGet);          
         
        }
        [HttpGet]
        public ActionResult Add(long DepId = 0, bool isActive = true)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            // Bind Dropdown List for Business Segment and Department.
            string errorMessage = string.Empty;         
            List<DOCMN_LookupMaster> lstBusinessSegment;
            List<DOCMN_LookupMaster> lstDepartment;
            List<DOCMN_LookupMaster> lstTimezone;
            BLDepartment objBLDepartment = new BLDepartment();
            List<DOCMN_Department> lstDOCMN_Department = new List<DOCMN_Department>();
            DOCMN_Department objDOCMN_Department = new DOCMN_Department();   
            
            lstBusinessSegment = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.BusinessSegment);
                      
            lstDepartment = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Department);

            lstTimezone = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Timezone);
            objDOCMN_Department.IsActive = true;
            if (DepId != 0)
            {
                // Fetch Record By Department ID.                       
                objDOCMN_Department.CMN_DepartmentId = DepId;
                objDOCMN_Department.IsActive = isActive;
                ExceptionTypes result = objBLDepartment.SearchDepartmentById(TimeZone,objDOCMN_Department, out lstDOCMN_Department, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    //Log Error
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Department, (long)ExceptionTypes.Uncategorized, errorMessage.ToString(), errorMessage.ToString());

                }
                if (lstDOCMN_Department.Count > 0)
                {
                    objDOCMN_Department = lstDOCMN_Department.FirstOrDefault();                    
                }                 
            }
            else
            {
                 objDOCMN_Department.EffectiveDate = DateTime.UtcNow;
            }
            objDOCMN_Department.lstBusinessSegment = lstBusinessSegment;
            objDOCMN_Department.lstCMN_Department = lstDepartment;
            objDOCMN_Department.lstTimeZone = lstTimezone;           
            
            return View("Add", objDOCMN_Department);
        }
        [HttpPost]
        public ActionResult Add(DOCMN_Department Department)
        {
            string errorMessage = string.Empty;
            string returnMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            try
            {
                long loginUserId = currentUser.ADM_UserMasterId;               
                BLDepartment objBLDepartment = new BLDepartment();
                DOCMN_Department objDOCMN_Department = new DOCMN_Department();
                objDOCMN_Department.CMN_DepartmentId = Department.CMN_DepartmentId;
                if (objDOCMN_Department.CMN_DepartmentId > 0) // Edit Mode
                {
                    BLCommon objCommon = new BLCommon();
                    if (!objCommon.ValidateLockBeforeSave(loginUserId, (long)ScreenType.Department, objDOCMN_Department.CMN_DepartmentId))
                    {
                        errorMessage = "Record not locked, please reload the page.";
                        result = ExceptionTypes.UnknownError;
                        return Json(new { ID = result, Message = errorMessage });
                    }

                    objDOCMN_Department.CreatedByRef = loginUserId;
                    returnMessage = "Record updated successfully.";
                }
                else
                {
                    objDOCMN_Department.CreatedByRef = loginUserId;  //Add Mode
                    returnMessage = "Record saved successfully.";
                }
                objDOCMN_Department.ERSDepartmentName = Department.ERSDepartmentName;
                objDOCMN_Department.BusinessSegmentLkup = Department.BusinessSegmentLkup;
                objDOCMN_Department.DepartmentLkup = Department.DepartmentLkup;
                objDOCMN_Department.EffectiveDate = Department.EffectiveDate;
                objDOCMN_Department.InactivationDate = Department.InactivationDate;
                objDOCMN_Department.IsActive = Department.IsActive;
                objDOCMN_Department.UTCCreatedOn = Department.UTCCreatedOn;
                result = objBLDepartment.SaveDepartment(objDOCMN_Department, out errorMessage);

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Department, (long)ExceptionTypes.Uncategorized, errorMessage.ToString(), errorMessage.ToString());
                    return Json(new { ID = result, Message = "An error occured while updating DB." });
                }

                return Json(new { ID = result, Message = returnMessage });
            }            
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Department, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return Json(new { ID = result, Message = "An error occured while performing Save action." });
            }

        }
        public ActionResult Edit()
        {
            return View();
        }

        // Get Department In Search Page.
        private List<DOCMN_Department> PGetDepartment(string strDepName = "", bool isActive = true)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            BLDepartment objBLDepartment = new BLDepartment();
            List<DOCMN_Department> lstDOCMN_Department;
            try
            {
                DOCMN_Department objDOCMN_Department = new DOCMN_Department();
                objDOCMN_Department.ERSDepartmentName = strDepName;
                if (isActive)
                {
                    objDOCMN_Department.IsActive = Convert.ToBoolean(isActive);
                }
                else
                {
                    objDOCMN_Department.IsActive = false;
                }
                string errorMessage = string.Empty;
                ExceptionTypes result = objBLDepartment.SearchDepartment(TimeZone,objDOCMN_Department, out lstDOCMN_Department, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    //Log error
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Department, (long)ExceptionTypes.Uncategorized, errorMessage.ToString(), errorMessage.ToString());
                }
                else if (result == ExceptionTypes.ZeroRecords)
                {
                    lstDOCMN_Department.Add(objDOCMN_Department);
                    lstDOCMN_Department = new List<DOCMN_Department>();
                }
                return lstDOCMN_Department;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Department, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                throw ex;
            }
        }

        // Get department report URL
        [HttpPost]
        public ActionResult GetDepartmentReportURL()
        {
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.DepartmentHistoryReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
            return Json(new { Data = urlData, JsonRequestBehavior.AllowGet });
        }
    }

}