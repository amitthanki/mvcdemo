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
    public class RecordsLockedController : Controller
    {
        private UIUserLogin currentUser;
        public RecordsLockedController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            }
        }
        [HttpPost]
        public JsonResult GetLockedRecordOrLockRecord(long caseId, long screenLkup, bool isForcedLocked)
        {
            UIRecordsLock objRecordsLocked = new UIRecordsLock();

            try
            {
                long currentLoginUserId = currentUser.ADM_UserMasterId;
                BLCommon objCommon = new BLCommon();
                ExceptionTypes result = objCommon.GetLockedRecordOrLockRecord(currentLoginUserId, screenLkup, caseId, isForcedLocked, out objRecordsLocked);

                if (result == (long)ExceptionTypes.Success)
                {                    
                    return Json(objRecordsLocked);
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RecordsLocked, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
            }

            objRecordsLocked.Status = (long)ExceptionTypes.UnknownError;
            objRecordsLocked.ErrorMessage = "The record is locked for editing by other user. Please retry.";
            return Json(objRecordsLocked, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UnlockRecord(long caseId, long screenLkup)
        {
            try
            {
                BLCommon objCommon = new BLCommon();
                ExceptionTypes result = objCommon.UnlockRecord(screenLkup, caseId);

                if (result == (long)ExceptionTypes.Success)
                {
                    return Json(new { Status = (long)ExceptionTypes.Success, ErrMsg = string.Empty });
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RecordsLocked, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
            }

            return Json(new { Status = (long)ExceptionTypes.UnknownError, ErrMsg = "Unlock failed. Please retry." });
        }
    }
}