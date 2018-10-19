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

namespace ENRLReconSystem.Controllers
{
    [ERSAuthenticationAttribute(Roles = "User")]
    public class AttachmentController : Controller
    {
        string errorMessage = string.Empty;
        private UIUserLogin currentUser;
        private BLCommon objBLCommon = new BLCommon();
        public AttachmentController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }
        /// <summary>
        /// Add file 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="genQueueId"></param>
        /// <returns></returns>
        public ActionResult AddPWAttachment(HttpPostedFileBase file,long genQueueId)
        {
            ExceptionTypes result = new ExceptionTypes();
            result = ExceptionTypes.UnknownError;
            string tempfilePath = string.Empty;
            string actualFilePath = string.Empty;
            string uploadFileFormats = string.Empty; 
            List<DOGEN_Attachments> lstDOGEN_Attachments = new List<DOGEN_Attachments>();
            try
            {
                //get the file format and validate
                uploadFileFormats = CacheUtility.GetMasterConfigurationByName(ConstantTexts.AttchmentFileFormat);
                string fileExt = (file != null && file.ContentLength > 0) ? Path.GetExtension(file.FileName).Replace(".", "") : string.Empty;
                if (fileExt.IsNullOrEmpty() && !uploadFileFormats.Contains("," + fileExt + ","))
                {
                    result = ExceptionTypes.UnknownError;
                    errorMessage = "Invalid File format.";
                }
                else
                {
                        if (SaveFileToActualFolder(file, out actualFilePath))
                        {
                            if (AddToAttchmentList(Path.GetFileName(file.FileName), Path.GetFileName(actualFilePath), actualFilePath, genQueueId,out lstDOGEN_Attachments, out errorMessage))
                            {
                                result = ExceptionTypes.Success;
                            }
                            else
                            {
                                result = ExceptionTypes.UnknownError;
                            }
                        }
                        else
                        {
                            result = ExceptionTypes.UnknownError;
                            errorMessage = "An error occured while moving file to actual folder.";
                        }
                }

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Attachments, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return PartialView("");
                }
                return PartialView("_Attachment",lstDOGEN_Attachments);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Attachments, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return PartialView("");
            }

        }
        public ActionResult UploadPWAttachment()
        {
            try
            {
                return PartialView("_UploadAttachment");
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Attachments, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw;
            }
        }
        /// <summary>
        /// Delete a attachment from the list and Attachment session
        /// when user remove attachment from process work screen
        /// </summary>
        /// <param name="slno"></param>
        /// <returns></returns>
        public ActionResult DeletePWAttchments(long attachmentId,long genRef)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            string returnMessage= "Error occoured while deleting.";
            ExceptionTypes result = new ExceptionTypes();
            List<DOGEN_Attachments> lstDOGEN_Attachments = new List<DOGEN_Attachments>();
            DOGEN_Attachments objDOGEN_Attachments = new DOGEN_Attachments();
            long loggedInUserId=0;
            try
            {
                
                loggedInUserId = currentUser.ADM_UserMasterId;
                objDOGEN_Attachments.GEN_AttachmentsId = attachmentId;
                objDOGEN_Attachments.GEN_QueueRef = genRef;
                result = objBLCommon.ManagePWAttachments(TimeZone,objDOGEN_Attachments, loggedInUserId, out lstDOGEN_Attachments, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return Json(new { ID = result, Message = returnMessage });
                }
                returnMessage = "Attachment deleted successfully.";
                return PartialView("_Attachment", lstDOGEN_Attachments);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Attachments, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return PartialView("");
            }
            
        }
        /// <summary>
        /// Download Attachments
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        public ActionResult DownloadPWAttchments(long attachmentId)
        {
            byte[] fileBytes = null;
            string fileName = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            DOGEN_Attachments objDOGEN_Attachments = new DOGEN_Attachments();
            try
            {

                result = objBLCommon.DownloadPWAttachments(attachmentId,out objDOGEN_Attachments, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Attachments, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return null;
                }
                fileBytes = System.IO.File.ReadAllBytes(objDOGEN_Attachments.FilePath);
                fileName = objDOGEN_Attachments.FileName;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Attachments, (long)ExceptionTypes.Uncategorized, "File Not Found", ex.Message);
                return Content("File Not Available.");
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Attachments, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Content("An error occoured.");
            }
           
        }

        /// <summary>
        /// Private method to save a attachment to actual folder path
        /// </summary>
        /// <param name="tempfilePath"></param>
        /// <param name="actualFilePath"></param>
        /// <returns></returns>
        private bool SaveFileToActualFolder(HttpPostedFileBase file, out string newFilePath)
        {
            bool isScucess = false;
            newFilePath = CacheUtility.GetMasterConfigurationByName(ConstantTexts.AttchmentFilePath); 
            string basePath = string.Empty;
            try
            {
               FileInfo fi = new FileInfo(file.FileName);
                basePath = newFilePath + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day;
                newFilePath = Path.Combine(basePath, DateTime.Now.ToString("yyyyMMddHHmmss_") + fi.Name); 
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                file.SaveAs(newFilePath);
                isScucess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Attachments, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
            return isScucess;
        }

        /// <summary>
        /// Private method to add the attachment to list and update the Attchment 
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="formattedFileName"></param>
        /// <param name="filePath"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool AddToAttchmentList(string FileName, string formattedFileName, string filePath, long genQueueId, out List<DOGEN_Attachments> lstDOGEN_Attachments, out string errorMessage)
        {
            bool isScucess = false;
            lstDOGEN_Attachments = new List<DOGEN_Attachments>();
            DOGEN_Attachments objDOGEN_Attachments = new DOGEN_Attachments();
            errorMessage = string.Empty;
            try
            {
                objDOGEN_Attachments.GEN_AttachmentsId = 0;
                objDOGEN_Attachments.GEN_QueueRef = genQueueId;
                objDOGEN_Attachments.FileName = FileName;
                objDOGEN_Attachments.UploadedFileName = formattedFileName;
                objDOGEN_Attachments.FilePath = filePath;
                //Save to data base
                if (ManagePWAttachments(objDOGEN_Attachments, currentUser.ADM_UserMasterId, out lstDOGEN_Attachments, out errorMessage))
                    isScucess = true;
                
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Attachments, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
            return isScucess;
        }
        /// <summary>
        /// Save / delete from db and return list of attachments
        /// </summary>
        /// <param name="objDOGEN_Attachments"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="lstDOGEN_Attachments"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool ManagePWAttachments(DOGEN_Attachments objDOGEN_Attachments, long loggedInUserId, out List<DOGEN_Attachments> lstDOGEN_Attachments, out string errorMessage)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            lstDOGEN_Attachments = new List<DOGEN_Attachments>();
            errorMessage = string.Empty;
            bool isScucess = false;
            try
            {
                ExceptionTypes result = objBLCommon.ManagePWAttachments(TimeZone,objDOGEN_Attachments, loggedInUserId, out lstDOGEN_Attachments, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                else
                    isScucess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Attachments, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
            return isScucess;
        }

    }
}