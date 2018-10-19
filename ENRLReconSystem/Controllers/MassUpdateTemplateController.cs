using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Data;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System.IO;

namespace ENRLReconSystem.Controllers
{
    public class MassUpdateTemplateController : Controller
    {
        // GET: MassUpdateTemplate
        string errorMessage = string.Empty;
        BLCommon _objBLCommon = new BLCommon();
        private UIUserLogin currentUser;

        /// <summary>
        /// Constructor to load the user session 
        /// </summary>
        public MassUpdateTemplateController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }
        ////////////////////////Mass Update By Template//////////////////////
        /// <summary>
        /// Get initial load for Mass Update Template
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                UIMassUpdateByTemplates objUIMassUpdateByTemplates = new UIMassUpdateByTemplates();
                objUIMassUpdateByTemplates.lstWorkbasket = CacheUtility.GetAllLookupsFromCache(LookupTypes.WorkBasket.ToInt64());
                objUIMassUpdateByTemplates.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, (long)currentUser.WorkBasketLkup);
                objUIMassUpdateByTemplates.WorkBasketLkup = (long)currentUser.WorkBasketLkup;
                objUIMassUpdateByTemplates.TemplateTypeLkup = TemplateType.MassUpdate.ToLong();
                objUIMassUpdateByTemplates.objDOGEN_BulkImportExcelTemplateMaster = PLoadTemplateByID(BulkImportExcelTemplateMaster.MassUpdateTemplate.ToInt32());
                objUIMassUpdateByTemplates.lstDOGEN_BulkImport = GetMassUpdateFileResult(objUIMassUpdateByTemplates);
                return View(objUIMassUpdateByTemplates);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdateByTemplate, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        /// <summary>
        /// Return a partialview with Massupdatetemplate data as a list
        /// </summary>
        /// <param name="objUIMassUpdateByTemplates"></param>
        /// <returns></returns>
        public ActionResult GetSearchResult(UIMassUpdateByTemplates objUIMassUpdateByTemplates)
        {
            try
            {
                objUIMassUpdateByTemplates.TemplateTypeLkup= TemplateType.MassUpdate.ToLong();
                return PartialView("_MassUpdateTemplateResult", GetMassUpdateFileResult(objUIMassUpdateByTemplates));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdateByTemplate, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return PartialView("");
            }

        }

        /// <summary>
        /// Load the popup where File can be selected for massUpdate and action can be selected to move the Queue to next level.
        /// </summary>
        /// <param name="lQueueLkup"></param>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public ActionResult LoadUploadMassUpdatePopup(long lQueueLkup, int templateID = 0)
        {
            try
            {
                int MaxMassUpdateRecordProcessCount = CacheUtility.GetMasterConfigurationByName(ConstantTexts.MaxMassUpdateRecordProcessCount).ToInt32();
                ViewBag.MaxMassUpdateRecordProcessCount = MaxMassUpdateRecordProcessCount;
                templateID = BulkImportExcelTemplateMaster.MassUpdateTemplate.ToInt32();//Mass update template ID

                UIMassUpdateUploadExtended objUIMassUpdateUploadExtended = new UIMassUpdateUploadExtended();
                objUIMassUpdateUploadExtended.objDOGEN_BulkImportExcelTemplateMaster = PLoadTemplateByID(templateID);

                objUIMassUpdateUploadExtended.lstActions = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.QueueVsAction, (long)lQueueLkup);
                if (objUIMassUpdateUploadExtended.lstActions.Count > 0)
                {
                    objUIMassUpdateUploadExtended.lstActions = objUIMassUpdateUploadExtended.lstActions.Where(xx => !Enum.GetValues(typeof(RemoveActionForMassUpdate)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef)).ToList();
                }
                return PartialView("_UploadMassUpdateTemplate", objUIMassUpdateUploadExtended);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdateByTemplate, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return PartialView("");
            }

        }

        /// <summary>
        ///  Mass Update - Upload excel file 
        ///  Step1: Validate the file extension Step2: 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="excelTemplateId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadExcelFile(HttpPostedFileBase file, long discripanctCatgoryLkup, long mostRecentQueueLkup)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = "Record created successfully";
            string tempfilePath = string.Empty;
            string actualFilePath = string.Empty;
            string uploadFileFormats = string.Empty;
            string origionalFileName = string.Empty;
            DataTable dtExcelData;
            long templateId = 7;
            List<string> headerColumns = null;
            long GEN_BulkImportID = 0;
            try
            {
                uploadFileFormats = ".xlsx";//need to configure
                string fileExt = (file != null && file.ContentLength > 0) ? Path.GetExtension(file.FileName).Replace(".", "") : string.Empty;
                origionalFileName = Path.GetFileName(file.FileName);
                //Validate the file extension
                if (fileExt.IsNullOrEmpty() && !uploadFileFormats.Contains("," + fileExt + ","))
                {
                    result = ExceptionTypes.UnknownError;
                    errorMessage = "Invalid File format.";

                }
                //Move the file to a temp nas drive 
                if (SaveFileToTempFolder(file, out tempfilePath))
                {
                    //Check the file and compare the sheetname, headers and if data is there or not
                    if (IsExcelFileValid(templateId, tempfilePath, origionalFileName, out headerColumns, out errorMessage))
                    {
                        // Load the Excel data into a Datatable with minimum validations
                        if (IsExcelDataValid(templateId, tempfilePath, headerColumns, out dtExcelData, out errorMessage))
                        {
                            //Move the file to actual folder
                            if (SaveFileToActualFolder(tempfilePath, out actualFilePath))
                            {
                                //Save the file and save the data in staging table for further processing
                                if (SaveToDB(origionalFileName, Path.GetFileName(actualFilePath), actualFilePath, currentUser.WorkBasketLkup, discripanctCatgoryLkup, templateId, dtExcelData, out GEN_BulkImportID, out errorMessage))
                                {
                                    result = ExceptionTypes.Success;
                                }
                                else
                                {
                                    result = ExceptionTypes.UnknownError;
                                    returnMessage = "An error occured while uploading the data." + "<br/><span style = 'color:white;' >" + errorMessage + "</ span >";
                                }
                            }
                            else
                            {
                                DeleteInvalidFile(tempfilePath);
                                result = ExceptionTypes.UnknownError;
                                returnMessage = "An error occured while moving file to actual folder." + "<br/><span style = 'color:white;' >" + errorMessage + "</ span >";
                            }
                        }
                        else
                        {
                            result = ExceptionTypes.UnknownError;
                            returnMessage = "An error occured while validating the data in file." + "<br/><span style = 'color:white;' >" + errorMessage + "</ span >";
                        }
                    }
                    else
                    {
                        DeleteInvalidFile(tempfilePath);
                        result = ExceptionTypes.UnknownError;
                        returnMessage = "Please Select a valid file to upload." + "<br/><span style = 'color:white;' >" + errorMessage + "</ span >";
                    }
                }
                else
                {
                    result = ExceptionTypes.UnknownError;
                    returnMessage = "Please Select a valid file to upload." + "<br/><span style = 'color:white;' >" + errorMessage + "</ span >";
                }

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdateByTemplate, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return Json(new { ID = result, Message = returnMessage, GENBulkImportID = GEN_BulkImportID });
                }



                return Json(new { ID = result, Message = returnMessage, GENBulkImportID = GEN_BulkImportID });

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }
        /// <summary>
        /// Download the sample template
        /// </summary>
        /// <param name="templateMasterId"></param>
        /// <returns></returns>
        public ActionResult DownloadTemplate(long templateMasterId)
        {
            byte[] fileBytes = null;
            string fileName = string.Empty;
            List<DOGEN_BulkImportExcelTemplateMaster> lstDOGEN_BulkImportExcelTemplateMaster = new List<DOGEN_BulkImportExcelTemplateMaster>();
            try
            {

                lstDOGEN_BulkImportExcelTemplateMaster = CacheUtility.GetBulkImportExcelTemplateFromCache().lstDOGEN_BulkImportExcelTemplateMaster;

                if (lstDOGEN_BulkImportExcelTemplateMaster.Where(xx => xx.GEN_BulkImportExcelTemplateMasterId == templateMasterId && xx.IsActive == true).Count() > 0)
                {
                    var item = lstDOGEN_BulkImportExcelTemplateMaster.Where(xx => xx.GEN_BulkImportExcelTemplateMasterId == templateMasterId && xx.IsActive == true).FirstOrDefault();
                    fileBytes = System.IO.File.ReadAllBytes(item.ExcelDirectoryPath + item.ExcelTemplateName);
                    fileName =  item.ExcelTemplateName;
                }

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdateByTemplate, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Content("File Not Available.");
            }

        }

        /// <summary>
        /// OST Action Update FROM MasUpdate Templates
        /// </summary>
        /// <param name="objDOGEN_OSTActions"></param>
        /// <returns></returns>

        public ActionResult OSTActionUpdate(DOGEN_OSTActions objDOGEN_OSTActions)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = string.Empty;
            try
            {
                objDOGEN_OSTActions.Gen_QueueIds = (!objDOGEN_OSTActions.GEN_BulkImportRef.IsNull() && objDOGEN_OSTActions.GEN_BulkImportRef > 0) ?
                                                     string.Empty : objDOGEN_OSTActions.Gen_QueueIds;
                objDOGEN_OSTActions.BusinessSegmentLkup = currentUser.BusinessSegmentLkup;
                objDOGEN_OSTActions.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                objDOGEN_OSTActions.RoleLkup = currentUser.RoleLkup;
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
        /// <summary>
        /// Eligibility Action Update from Mass Update Templates
        /// </summary>
        /// <param name="objDOGEN_EligibilityActions"></param>
        /// <returns></returns>
        public ActionResult EligibilityActionUpdate(DOGEN_EligibilityActions objDOGEN_EligibilityActions)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = string.Empty;
            try
            {
                objDOGEN_EligibilityActions.Gen_QueueIds = (!objDOGEN_EligibilityActions.GEN_BulkImportRef.IsNull() && objDOGEN_EligibilityActions.GEN_BulkImportRef > 0) ?
                                         string.Empty : objDOGEN_EligibilityActions.Gen_QueueIds;
                objDOGEN_EligibilityActions.BusinessSegmentLkup = currentUser.BusinessSegmentLkup;
                objDOGEN_EligibilityActions.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                objDOGEN_EligibilityActions.LoginID = currentUser.ADM_UserMasterId;
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
        /// <summary>
        /// RPR Action update from Mass Update Templates
        /// </summary>
        /// <param name="objDOGEN_RPRActions"></param>
        /// <returns></returns>
        public ActionResult RPRActionUpdate(DOGEN_RPRActions objDOGEN_RPRActions)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = string.Empty;
            try
            {

                objDOGEN_RPRActions.Gen_QueueIds = (!objDOGEN_RPRActions.GEN_BulkImportRef.IsNull() && objDOGEN_RPRActions.GEN_BulkImportRef > 0) ?
                                       string.Empty : objDOGEN_RPRActions.Gen_QueueIds;
                objDOGEN_RPRActions.BusinessSegmentLkup = currentUser.BusinessSegmentLkup;
                objDOGEN_RPRActions.LastUpdatedByRef = currentUser.ADM_UserMasterId;
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
        /// Load Queue based on Category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <returns></returns>
        private bool IsExcelDataValid(long templateId, string filePath, List<string> headerColumns, out DataTable dtExcel,out string errorMessage)
        {
            bool isSuccess = false;
            errorMessage = string.Empty;
          
            try
            {
                int MaxMassUpdateRecordProcessCount = CacheUtility.GetMasterConfigurationByName(ConstantTexts.MaxMassUpdateRecordProcessCount).ToInt32();
                dtExcel = new DataTable();
                DataRow drExcel = null;
                string headerNameToFillDBColumn = string.Empty;
                string sheetName = string.Empty;
                int startRow, startColumn;
                DOGEN_BulkImportExcelTemplateMaster objDOGEN_BulkImportExcelTemplateMaster = new DOGEN_BulkImportExcelTemplateMaster();
                //Fill the Sheet startrow start column from cache
                objDOGEN_BulkImportExcelTemplateMaster = CacheUtility.GetBulkImportExcelTemplateFromCache().lstDOGEN_BulkImportExcelTemplateMaster.Where(xx => xx.GEN_BulkImportExcelTemplateMasterId == templateId && xx.IsActive == true).FirstOrDefault();
                startRow = objDOGEN_BulkImportExcelTemplateMaster.StartRow;
                startColumn = objDOGEN_BulkImportExcelTemplateMaster.StartColumn;
                sheetName = objDOGEN_BulkImportExcelTemplateMaster.SheetName;

                using (var document = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = document.WorkbookPart;
                    Sheet sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(x => x.Name == sheetName);
                    string relationshipId = sheet.Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
                    SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
                    var sharedStringPart = workbookPart.SharedStringTablePart;

                    SharedStringItem[] values = null;
                    if (sharedStringPart != null)
                        values = sharedStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                    //List<string> HeaderColumns = null;
                    List<string> currentRowColumns = null;
                    int columnIndex = 0;
                    //Read last row

                    //Add the columns in Datatable
                    if (headerColumns.Count > 0)
                    {
                        foreach (string col in headerColumns)
                        {
                            dtExcel.Columns.Add(col, typeof(string));
                        }

                    }

                    Row lastRow = worksheetPart.Worksheet.Descendants<Row>().LastOrDefault();
                    if (lastRow != null && lastRow.RowIndex <=startRow )
                    {
                        isSuccess = false;
                        errorMessage = "Please add cases in the template for mass update.";
                        goto EndLebel;

                    }

                    if (lastRow != null && lastRow.RowIndex - startRow > MaxMassUpdateRecordProcessCount)
                    {
                        isSuccess = false;
                        errorMessage = "Maximum"+ MaxMassUpdateRecordProcessCount+" cases can be processed at a time";
                        goto EndLebel;

                    }
                    //Loop all excel rows to get column values mapping with form fields.
                    for (int i = startRow + 1; i <= lastRow.RowIndex; i++)
                    {
                        //Get current row from excel.
                        Row currentRow = worksheetPart.Worksheet.Descendants<Row>().Where(r => r.RowIndex == i).FirstOrDefault();

                        if (currentRow == null)
                            continue;
                        //Get current row columns
                        Cell[] currentRowCells = currentRow.Descendants<Cell>().ToArray();
                        columnIndex = 0;

                        if (currentRowCells == null)
                            continue;

                        currentRowColumns = new List<string>();
                        //Loop to get the Current cells value and puh it to List currentRowColumns
                        foreach (var cell in currentRowCells)
                        {
                            // Gets the column index of the cell with data
                            int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                            cellColumnIndex--; //zero based index
                            if (columnIndex < cellColumnIndex)
                            {
                                do
                                {
                                    var value = "";//Insert blank data here;
                                    currentRowColumns.Add(value);
                                    columnIndex++;
                                }
                                while (columnIndex < cellColumnIndex);
                            }

                            // The cells contains a string input that is not a formula
                            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && values != null)
                            {
                                var index = int.Parse(cell.CellValue.Text);
                                var value = values[index].InnerText;
                                currentRowColumns.Add(value);
                            }
                            else
                            {
                                if (cell.CellValue != null)
                                {
                                    currentRowColumns.Add(cell.CellValue.Text);
                                }
                                else if (cell.DataType != null && cell.DataType.Value == CellValues.InlineString && cell.InlineString != null &&
                                      cell.InlineString.Text != null)
                                {
                                    currentRowColumns.Add(cell.InlineString.Text.InnerText.ToString());
                                }
                                else
                                    currentRowColumns.Add(string.Empty);
                            }
                            columnIndex++;
                        }
                        if (!CheckRowHasData(currentRowColumns))
                            continue;

                        headerNameToFillDBColumn = string.Empty;
                        drExcel = dtExcel.NewRow();

                        for (int j = startColumn - 1; j <= currentRowColumns.Count - 1; j++)
                        {

                            headerNameToFillDBColumn = headerColumns[j];

                            if (!string.IsNullOrEmpty(headerNameToFillDBColumn))
                            {
                                drExcel[headerNameToFillDBColumn] = currentRowColumns[j];
                            }
                        }
                        dtExcel.Rows.Add(drExcel);
                    }
                }
                EndLebel:
                if (dtExcel != null && dtExcel.Rows.Count > 0)
                {
                    isSuccess = true;
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SaveFileToTempFolder(HttpPostedFileBase file, out string tempfilePath)
        {
            bool isUploadSuccess = false;
            try
            {
                tempfilePath = string.Empty;
                string webServerTempPath = CacheUtility.GetMasterConfigurationByName(ConstantTexts.webServerTempPath);//Temp directory location on the NAS drive
                string basePathTemp = string.Empty;
                //path
                basePathTemp = webServerTempPath + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day;
                tempfilePath = Path.Combine(basePathTemp, DateTime.Now.ToString("yyyyMMddHHmmss_") + Path.GetFileName(file.FileName));
                if (!Directory.Exists(basePathTemp))
                {
                    Directory.CreateDirectory(basePathTemp);
                }
                file.SaveAs(tempfilePath);
                isUploadSuccess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                isUploadSuccess = false;
                throw ex;
            }
            return isUploadSuccess;
        }
        private bool IsExcelFileValid(long excelTemplateId, string filePath, string fileName, out List<string> headerColumns, out string errorMessage)
        {
            bool isExcelFileValid = true;
            errorMessage = string.Empty;
            bool repeatedColumnFound = false;
            string[] uploadFileColumnsList = null;
            string[] templateColumnsList;
            DOGEN_BulkImportExcelTemplateMaster objDOGEN_BulkImportExcelTemplateMaster = new DOGEN_BulkImportExcelTemplateMaster();
            List<DOGEN_BulkImportColumnsMapping> lstDOGEN_BulkImportColumnsMapping = new List<DOGEN_BulkImportColumnsMapping>();
            headerColumns = new List<string>();
            try
            {
                objDOGEN_BulkImportExcelTemplateMaster = CacheUtility.GetBulkImportExcelTemplateFromCache().lstDOGEN_BulkImportExcelTemplateMaster.Where(xx => xx.GEN_BulkImportExcelTemplateMasterId == excelTemplateId && xx.IsActive == true).FirstOrDefault();
                lstDOGEN_BulkImportColumnsMapping = CacheUtility.GetBulkImportExcelTemplateFromCache().lstDOGEN_BulkImportColumnsMapping.Where(xx => xx.GEN_BulkImportExcelTemplateMasterRef == excelTemplateId && xx.IsActive == true).ToList();
                templateColumnsList = lstDOGEN_BulkImportColumnsMapping.OrderBy(x => x.ColumnSequence).Select(s => s.ColumnDisplayName).ToArray();

                headerColumns = GetXlsxHeaderRowAsString(filePath, objDOGEN_BulkImportExcelTemplateMaster.SheetName, objDOGEN_BulkImportExcelTemplateMaster.StartRow, out errorMessage, out repeatedColumnFound);
                if (repeatedColumnFound)
                {
                    errorMessage = "There are some repeated columns in the excel.";
                    isExcelFileValid = false;
                    return isExcelFileValid;
                }

                if (headerColumns != null && headerColumns.Count > 0)
                {
                    if (headerColumns.Count >= objDOGEN_BulkImportExcelTemplateMaster.StartColumn)
                    {
                        uploadFileColumnsList = headerColumns.Skip((int)objDOGEN_BulkImportExcelTemplateMaster.StartColumn - 1).ToArray();
                    }

                    bool retVal = true;
                    string missingColumnNames = string.Empty;
                    if (uploadFileColumnsList != null)
                    {
                        if (uploadFileColumnsList.Length == templateColumnsList.Length)
                        {
                            for (int i = 0; i < templateColumnsList.Length; i++)
                            {
                                if (uploadFileColumnsList[i].Trim().Length > 0 && templateColumnsList[i].Trim() != uploadFileColumnsList[i].Trim())
                                {
                                    retVal = false;
                                    missingColumnNames += ',' + templateColumnsList[i];
                                }
                            }
                            if (retVal == false)
                            {
                                errorMessage = "Following column/s are missing or renamed or not as per template order in the uploaded excel:" + missingColumnNames.TrimStart(',') + ".";
                                isExcelFileValid = false;
                            }
                            else
                                isExcelFileValid = true;//if all columns are present in uploaded file and in same order then return true
                        }
                        else if (uploadFileColumnsList.Length > 0)
                        {
                            for (int i = 0; i < templateColumnsList.Length; i++)
                            {
                                //column matching is not casesensitive
                                if (i < uploadFileColumnsList.Length)
                                {
                                    if (templateColumnsList[i] != uploadFileColumnsList[i])
                                    {
                                        missingColumnNames += ',' + templateColumnsList[i];
                                    }
                                }
                                else
                                {
                                    missingColumnNames += ',' + templateColumnsList[i];
                                }
                            }
                            errorMessage = "Following column/s are missing or renamed or not as per template order in the uploaded excel:" + missingColumnNames.TrimStart(',') + ".";
                            isExcelFileValid = false;
                        }
                    }
                }
                else
                {
                    isExcelFileValid = false;
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }

            }
            catch (Exception ex)
            {
                errorMessage += ex.Message + ex.StackTrace;
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                isExcelFileValid = false;
            }
            return isExcelFileValid;
        }

        private List<string> GetXlsxHeaderRowAsString(string filePath, string sheetName, long startRow, out string errorMessage, out bool repeatedColumnFound)
        {
            errorMessage = string.Empty;
            List<string> headerRowFromExcel = null;
            repeatedColumnFound = false;
            try
            {
                using (var document = SpreadsheetDocument.Open(filePath, false))
                {

                    var workbookPart = document.WorkbookPart;
                    var workbook = workbookPart.Workbook;

                    var sheet = workbook.Descendants<Sheet>().FirstOrDefault(x => x.Name == sheetName);
                    if (sheet != null)
                    {
                        var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                        var sharedStringPart = workbookPart.SharedStringTablePart;
                        SharedStringItem[] values = null;
                        if (sharedStringPart != null)
                            values = sharedStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                        Row row = new Row();
                        row = worksheetPart.Worksheet.Descendants<Row>().FirstOrDefault();

                        if (row != null)
                        {
                            var cells = row.Descendants<Cell>();
                            if (cells != null)
                            {
                                headerRowFromExcel = new List<string>();
                                long columnIndex = 0;
                                foreach (var cell in cells)
                                {
                                    // Gets the column index of the cell with data
                                    int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                    cellColumnIndex--; //zero based index

                                    if (columnIndex < cellColumnIndex)
                                    {
                                        do
                                        {
                                            var value = "";//Insert blank data here;
                                            headerRowFromExcel.Add(value);
                                            columnIndex++;
                                        }
                                        while (columnIndex < cellColumnIndex);
                                    }

                                    // The cells contains a string input that is not a formula
                                    if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && values != null)
                                    {
                                        var index = int.Parse(cell.CellValue.Text);
                                        var value = values[index].InnerText;

                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            if (headerRowFromExcel.Contains(value)) { repeatedColumnFound = true; }
                                            headerRowFromExcel.Add(value);
                                        }
                                    }
                                    else
                                    {
                                        if (cell.CellValue != null)
                                        {
                                            if (!string.IsNullOrEmpty(cell.CellValue.Text))
                                            {
                                                if (headerRowFromExcel.Contains(cell.CellValue.Text)) { repeatedColumnFound = true; }
                                                headerRowFromExcel.Add(cell.CellValue.Text);
                                            }
                                        }
                                        else if (cell.DataType != null && cell.DataType.Value == CellValues.InlineString && cell.InlineString != null &&
                                                 cell.InlineString.Text != null)
                                        {
                                            if (!string.IsNullOrEmpty(cell.InlineString.Text.InnerText.ToString()))
                                            {
                                                if (headerRowFromExcel.Contains(cell.InlineString.Text.InnerText.ToString())) { repeatedColumnFound = true; }
                                                headerRowFromExcel.Add(cell.InlineString.Text.InnerText.ToString());
                                            }
                                        }
                                        else
                                            headerRowFromExcel.Add(string.Empty);
                                    }
                                    columnIndex++;
                                }
                            }
                            else
                            {
                                errorMessage = "Could not read the cells in the excel.";
                            }
                        }
                        else
                        {
                            errorMessage = "Could not read the cells in the excel.";
                        }
                    }
                    else
                    {
                        errorMessage = "Could not find the sheet '" + sheetName + "' in the excel.";
                    }
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                errorMessage += ex.Message + ex.StackTrace;
            }
            return headerRowFromExcel;
        }

        private int? GetColumnIndexFromName(string columnName)
        {
            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }

            return number;
        }
        private string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        }
        private void DeleteInvalidFile(string tempFilePath)
        {
            try
            {

                System.IO.File.Delete(tempFilePath);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;

            }
        }
        /// <summary>
        /// it will save the file to actual path
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newFilePath"></param>
        /// <returns></returns>
        private bool SaveFileToActualFolder(string filePath, out string newFilePath)
        {
            bool isScucess = false;
            string basePath = string.Empty;
            try
            {
                newFilePath = CacheUtility.GetMasterConfigurationByName(ConstantTexts.MassUpdateFilePath);
                FileInfo fi = new FileInfo(filePath);
                basePath = newFilePath + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day;
                newFilePath = basePath + "\\" + fi.Name;
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                fi.MoveTo(newFilePath);
                isScucess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
            return isScucess;
        }

        private bool CheckRowHasData(List<string> currentRowCells)
        {
            bool hasData = false;
            foreach (string str in currentRowCells)
            {
                if (str != string.Empty)
                {
                    hasData = true;
                    break;
                }
            }
            return hasData;
        }


        private bool SaveToDB(string oldFileName, string newFileName, string filePath, long? workbasketId, long discripanctCatgoryLkup, long templateId, DataTable dtFileData, out long GEN_BulkImportID, out string errorMessage)
        {
            bool isScuess = false;
            DOGEN_BulkImport objDOGEN_BulkImport = new DOGEN_BulkImport();
            errorMessage = string.Empty;
            GEN_BulkImportID = 0;
            try
            {
                long loginUserID = currentUser.ADM_UserMasterId;
                objDOGEN_BulkImport.WorkBasketLkup = workbasketId;
                objDOGEN_BulkImport.DiscrepancyCategoryLkup = discripanctCatgoryLkup;
                objDOGEN_BulkImport.GEN_BulkImportExcelTemplateMasterRef = templateId;
                objDOGEN_BulkImport.ExcelFileName = oldFileName;
                objDOGEN_BulkImport.DuplicateFileName = newFileName;
                objDOGEN_BulkImport.ExcelFilelPath = filePath;
                objDOGEN_BulkImport.ImportStatusLkup = (long)ImportStatus.ReadyForImport;
                objDOGEN_BulkImport.IsActive = true;
                ExceptionTypes result = _objBLCommon.SaveBulkUpload(objDOGEN_BulkImport, loginUserID, dtFileData, out GEN_BulkImportID, out errorMessage);
                if (result == ExceptionTypes.Success)
                {
                    isScuess = true;
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
            return isScuess;
        }
        /// <summary>
        /// Load the template from the cache memory
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        private DOGEN_BulkImportExcelTemplateMaster PLoadTemplateByID(int templateID)
        {


            DOGEN_BulkImportExcelTemplateMaster objDOGEN_BulkImportExcelTemplateMaster = new DOGEN_BulkImportExcelTemplateMaster();
            try
            {
                objDOGEN_BulkImportExcelTemplateMaster = CacheUtility.GetBulkImportExcelTemplateFromCache().lstDOGEN_BulkImportExcelTemplateMaster.Where(xx => xx.GEN_BulkImportExcelTemplateMasterId == templateID).FirstOrDefault();
                return objDOGEN_BulkImportExcelTemplateMaster;

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// Get Bulk import search results as list with template type as MassUpdate Template
        /// </summary>
        /// <param name="objUIMassUpdateByTemplates"></param>
        /// <returns></returns>
        private List<DOGEN_BulkImport> GetMassUpdateFileResult(UIMassUpdateByTemplates objUIMassUpdateByTemplates)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            List<DOGEN_BulkImport> lstDOGEN_BulkImport = new List<DOGEN_BulkImport>();
            errorMessage = string.Empty;
            try
            {
                ExceptionTypes result = _objBLCommon.GetBulkUploadSearchResult(TimeZone, objUIMassUpdateByTemplates, out lstDOGEN_BulkImport, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }

                return lstDOGEN_BulkImport;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Download the template
        /// </summary>
        /// <param name="templateMasterId"></param>
        /// <returns></returns>
        public ActionResult DownloadFile(long genBulkImportId)
        {
            UIMassUpdateByTemplates objUIMassUpdateByTemplates = new UIMassUpdateByTemplates();
            List<DOGEN_BulkImport> lstDOGEN_BulkImport = new List<DOGEN_BulkImport>();
            byte[] fileBytes = null;
            string fileName = string.Empty;
            string massUpdateFilePath = string.Empty;
            long? timeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            try
            {
                 massUpdateFilePath = CacheUtility.GetMasterConfigurationByName(ConstantTexts.MassUpdateFilePath);
                objUIMassUpdateByTemplates.Gen_BulkImportId = genBulkImportId;
                objUIMassUpdateByTemplates.TemplateTypeLkup = TemplateType.MassUpdate.ToInt64();

                ExceptionTypes result = _objBLCommon.GetBulkUploadSearchResult(timeZone, objUIMassUpdateByTemplates, out lstDOGEN_BulkImport, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return Content("File Not Available.");
                }

                if (lstDOGEN_BulkImport != null && lstDOGEN_BulkImport.Count>0)
                {
                    var item = lstDOGEN_BulkImport.Where(xx => xx.GEN_BulkImportId == genBulkImportId && xx.IsActive == true).FirstOrDefault();
                    fileBytes = System.IO.File.ReadAllBytes(item.ExcelFilelPath);
                    fileName =  item.ExcelFileName;
                }
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Content("File Not Available.");
            }

        }




    }
}