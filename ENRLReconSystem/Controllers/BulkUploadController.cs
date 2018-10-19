using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using ENRLReconSystem.BL;
using System.Reflection;

namespace ENRLReconSystem.Controllers
{
    [ERSAuthenticationAttribute(Roles = "User")]
    public class BulkUploadController : Controller
    {
        string errorMessage = string.Empty;
        private BLBulkUpload _objBLBulkUpload = new BLBulkUpload();
        private UIUserLogin currentUser;
        public BulkUploadController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }
        // GET: BulkUpload

        /// <summary>
        /// BulkUpload Initial Load
        /// </summary>
        /// <returns></returns>
        public ActionResult BulkUpload()
        {
            try
            {
                return View(PLoadBulkUpload());
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw;
            }
            
        }
        /// <summary>
        /// Load partial view to upload excel file
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(int templateID)
        {
            try
            {
                return PartialView("_Upload", PLoadTemplateByID(templateID));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return PartialView("");
            }
           
        }
        /// <summary>
        /// Get Bulk upload search result
        /// </summary>
        /// <param name="objUIBulkUploadSearch"></param>
        /// <returns></returns>

        public ActionResult GetSearchResult(UIBulkUploadSearch objUIBulkUploadSearch)
        {
            try
            {
                return PartialView("_SearchResult", PLoadBulkUploadSearchResult(objUIBulkUploadSearch));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return PartialView("");
            }
            
        }

        /// <summary>
        ///  Bulk Import - Upload excel file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="excelTemplateId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadExcelFile(HttpPostedFileBase file,long workbasketId,long discripanctCatgoryLkup, long excelTemplateId)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = "Record created successfully";
            string tempfilePath = string.Empty;
            string actualFilePath = string.Empty;
            string uploadFileFormats = string.Empty;
            string origionalFileName = string.Empty; 
            try
            {
                uploadFileFormats = CacheUtility.GetMasterConfigurationByName(ConstantTexts.BulkUploadFileFormats);

                string fileExt = (file != null && file.ContentLength > 0) ? Path.GetExtension(file.FileName).Replace(".", "") : string.Empty;
                origionalFileName = Path.GetFileName(file.FileName);
                if (fileExt.IsNullOrEmpty() && !uploadFileFormats.Contains("," + fileExt + ","))
                {
                    result = ExceptionTypes.UnknownError;
                    errorMessage= "Invalid File format.";

                }
                else
                {
                    if (SaveFileToTempFolder(file, out tempfilePath))
                    {
                        if (IsExcelFileValid(excelTemplateId, tempfilePath, origionalFileName, out errorMessage))
                        {
                            IsExcelDataValid();
                            if (SaveFileToActualFolder(tempfilePath, out actualFilePath))
                            {
                                if (SaveToDB(Path.GetFileName(file.FileName), Path.GetFileName(actualFilePath), actualFilePath, workbasketId, discripanctCatgoryLkup, excelTemplateId, out errorMessage))
                                {
                                    result = ExceptionTypes.Success;
                                }
                                else
                                {
                                    result = ExceptionTypes.UnknownError;
                                    errorMessage = "An error occured while uploading the data.";
                                }
                            }
                            else
                            {
                                result = ExceptionTypes.UnknownError;
                                errorMessage = "An error occured while moving file to actual folder.";
                            }
                        }
                        else
                        {
                            DeleteInvalidFile(tempfilePath);
                            result = ExceptionTypes.UnknownError;
                            errorMessage = "Please Select a valid file to upload";
                        }
                    }
                    else
                    {
                        result = ExceptionTypes.UnknownError;
                        errorMessage = "Please Select a valid file to upload";
                    }
                }


                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = errorMessage });
                }

                return Json(new { ID = result, Message = returnMessage });

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }
        /// <summary>
        /// Download template based on template ID
        /// </summary>
        /// <param name="templateMasterId"></param>
        /// <returns></returns>

        public FileResult DownloadTemplate(long templateMasterId)
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
                    fileName = item.ExcelTemplateName;
                }

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw;
            }

        }


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
        private UIBulkUploadSearch PLoadBulkUpload()
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            UIBulkUploadSearch objUIBulkUploadSearch = new UIBulkUploadSearch();
            List<DOGEN_BulkImport> lstDOGEN_BulkImport = new List<DOGEN_BulkImport>();
            errorMessage = string.Empty;
            try
            {
                long workBasketLkp = currentUser.WorkBasketLkup.ToInt64();
                objUIBulkUploadSearch.WorkbasketLkup = workBasketLkp;
                objUIBulkUploadSearch.lstWorkbasket = CacheUtility.GetAllLookupsFromCache(LookupTypes.WorkBasket.ToInt64());
                objUIBulkUploadSearch.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, workBasketLkp);
                objUIBulkUploadSearch.lstDOGEN_BulkImportExcelTemplateMaster = CacheUtility.GetBulkImportExcelTemplateFromCache().lstDOGEN_BulkImportExcelTemplateMaster.Where(x=>x.TemplateTypeLkup== TemplateType.BulkUpload.ToInt64()).ToList();
                objUIBulkUploadSearch.TemplateTypeLkup = TemplateType.BulkUpload.ToLong();
                ExceptionTypes result = _objBLBulkUpload.GetBulkUploadSearchResult(TimeZone,objUIBulkUploadSearch,out lstDOGEN_BulkImport, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
                objUIBulkUploadSearch.lstDOGEN_BulkImport = lstDOGEN_BulkImport;

                return objUIBulkUploadSearch;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
        }
        private List<DOGEN_BulkImport> PLoadBulkUploadSearchResult(UIBulkUploadSearch objUIBulkUploadSearch)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            List<DOGEN_BulkImport> lstDOGEN_BulkImport = new List<DOGEN_BulkImport>();
            errorMessage = string.Empty;
            try
            {
                objUIBulkUploadSearch.TemplateTypeLkup=TemplateType.BulkUpload.ToLong();
                ExceptionTypes result = _objBLBulkUpload.GetBulkUploadSearchResult(TimeZone,objUIBulkUploadSearch,out lstDOGEN_BulkImport, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
                return lstDOGEN_BulkImport;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
        }
        private bool SaveFileToTempFolder(HttpPostedFileBase file, out string tempFilePath)
        {
            bool isUploadSuccess = false;
            try
            {
                tempFilePath = string.Empty;
                string webServerTempPath = CacheUtility.GetMasterConfigurationByName(ConstantTexts.webServerTempPath);
                //path
                tempFilePath = Path.Combine(webServerTempPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + Path.GetFileName(file.FileName));
                if (!Directory.Exists(webServerTempPath))
                {
                    Directory.CreateDirectory(webServerTempPath);
                }
                file.SaveAs(tempFilePath);
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
        private bool IsExcelFileValid(long excelTemplateId, string filePath,string fileName, out string errorMessage)
        {
            bool isExcelFileValid = true;
            errorMessage = string.Empty;
            bool repeatedColumnFound = false;
            string[] uploadFileColumnsList = null;
            string[] templateColumnsList;
            DOGEN_BulkImportExcelTemplateMaster objDOGEN_BulkImportExcelTemplateMaster = new DOGEN_BulkImportExcelTemplateMaster();
            List<DOGEN_BulkImportColumnsMapping> lstDOGEN_BulkImportColumnsMapping = new List<DOGEN_BulkImportColumnsMapping>();
            try
            {
                objDOGEN_BulkImportExcelTemplateMaster = CacheUtility.GetBulkImportExcelTemplateFromCache().lstDOGEN_BulkImportExcelTemplateMaster.Where(xx => xx.GEN_BulkImportExcelTemplateMasterId == excelTemplateId && xx.IsActive == true).FirstOrDefault();
                lstDOGEN_BulkImportColumnsMapping = CacheUtility.GetBulkImportExcelTemplateFromCache().lstDOGEN_BulkImportColumnsMapping.Where(xx => xx.GEN_BulkImportExcelTemplateMasterRef == excelTemplateId && xx.IsActive == true).ToList();
                templateColumnsList = lstDOGEN_BulkImportColumnsMapping.OrderBy(x => x.ColumnSequence).Select(s => s.ColumnDisplayName).ToArray();
              
                if (fileName != objDOGEN_BulkImportExcelTemplateMaster.ExcelTemplateName)
                {
                    errorMessage = "Invalid File.";
                    return false;

                }

                var getXlsxHeaderRowAsStringRetVal = GetCSVHeaderRowAsString(filePath, objDOGEN_BulkImportExcelTemplateMaster.SheetName, objDOGEN_BulkImportExcelTemplateMaster.StartRow, out errorMessage, out repeatedColumnFound);
                //var getXlsxHeaderRowAsStringRetVal = GetXlsxHeaderRowAsString(filePath, objDOGEN_BulkImportExcelTemplateMaster.SheetName, objDOGEN_BulkImportExcelTemplateMaster.StartRow, out errorMessage, out repeatedColumnFound);
                if (repeatedColumnFound)
                {
                    errorMessage = "There are some repeated columns in the excel.";
                    isExcelFileValid = false;
                    return isExcelFileValid;
                }

                if (getXlsxHeaderRowAsStringRetVal != null && getXlsxHeaderRowAsStringRetVal.Count > 0)
                {
                    if (getXlsxHeaderRowAsStringRetVal.Count >= objDOGEN_BulkImportExcelTemplateMaster.StartColumn)
                    {
                        uploadFileColumnsList = getXlsxHeaderRowAsStringRetVal.Skip((int)objDOGEN_BulkImportExcelTemplateMaster.StartColumn - 1).ToArray();
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
                }
                if (!isExcelFileValid)
                {
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
        /// <summary>
        /// Return and Check If data is missing or required field is Missing in the Excel
        /// </summary>
        /// <returns></returns>
        private bool IsExcelDataValid()
        {

            try
            {
                return true;

            }
            catch (Exception)
            {

                throw;
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
                newFilePath = CacheUtility.GetMasterConfigurationByName(ConstantTexts.BulkUploadFilePath);
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
        /// <summary>
        /// make entry to database
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="newFileName"></param>
        /// <param name="filePath"></param>
        /// <param name="workbasketId"></param>
        /// <param name="discripanctCatgoryLkup"></param>
        /// <param name="templateId"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool SaveToDB(string oldFileName, string newFileName, string filePath, long workbasketId, long discripanctCatgoryLkup, long templateId, out string errorMessage)
        {
            bool isScuess = false;
            DOGEN_BulkImport objDOGEN_BulkImport = new DOGEN_BulkImport();
            errorMessage = string.Empty;
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
                ExceptionTypes result = _objBLBulkUpload.SaveBulkUpload(objDOGEN_BulkImport, loginUserID, out errorMessage);
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
        //private List<string> GetXlsxHeaderRowAsString(string filePath, string sheetName, long startRow, out string errorMessage, out bool repeatedColumnFound)
        //{
        //    errorMessage = string.Empty;
        //    List<string> headerRowFromExcel = null;
        //    repeatedColumnFound = false;
        //    try
        //    {
        //        using (var document = SpreadsheetDocument.Open(filePath, false))
        //        {

        //            var workbookPart = document.WorkbookPart;
        //            var workbook = workbookPart.Workbook;

        //            var sheet = workbook.Descendants<Sheet>().FirstOrDefault(x => x.Name == sheetName);
        //            if (sheet != null)
        //            {
        //                var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
        //                var sharedStringPart = workbookPart.SharedStringTablePart;
        //                SharedStringItem[] values = null;
        //                if (sharedStringPart != null)
        //                    values = sharedStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

        //                Row row = new Row();

        //                if (startRow == 1)
        //                {
        //                    row = worksheetPart.Worksheet.Descendants<Row>().FirstOrDefault();
        //                }
        //                else
        //                {
        //                    row = worksheetPart.Worksheet.Descendants<Row>().Skip(Convert.ToInt32(startRow) - 1).FirstOrDefault();
        //                }
        //                if (row != null)
        //                {
        //                    var cells = row.Descendants<Cell>();
        //                    if (cells != null)
        //                    {
        //                        headerRowFromExcel = new List<string>();
        //                        long columnIndex = 0;
        //                        foreach (var cell in cells)
        //                        {
        //                            // Gets the column index of the cell with data
        //                            int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
        //                            cellColumnIndex--; //zero based index

        //                            if (columnIndex < cellColumnIndex)
        //                            {
        //                                do
        //                                {
        //                                    var value = "";//Insert blank data here;
        //                                    headerRowFromExcel.Add(value);
        //                                    columnIndex++;
        //                                }
        //                                while (columnIndex < cellColumnIndex);
        //                            }

        //                            // The cells contains a string input that is not a formula
        //                            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && values != null)
        //                            {
        //                                var index = int.Parse(cell.CellValue.Text);
        //                                var value = values[index].InnerText;

        //                                if (!string.IsNullOrEmpty(value))
        //                                {
        //                                    if (headerRowFromExcel.Contains(value)) { repeatedColumnFound = true; }
        //                                    headerRowFromExcel.Add(value);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (cell.CellValue != null)
        //                                {
        //                                    if (!string.IsNullOrEmpty(cell.CellValue.Text))
        //                                    {
        //                                        if (headerRowFromExcel.Contains(cell.CellValue.Text)) { repeatedColumnFound = true; }
        //                                        headerRowFromExcel.Add(cell.CellValue.Text);
        //                                    }
        //                                }
        //                                else if (cell.DataType != null && cell.DataType.Value == CellValues.InlineString && cell.InlineString != null &&
        //                                         cell.InlineString.Text != null)
        //                                {
        //                                    if (!string.IsNullOrEmpty(cell.InlineString.Text.InnerText.ToString()))
        //                                    {
        //                                        if (headerRowFromExcel.Contains(cell.InlineString.Text.InnerText.ToString())) { repeatedColumnFound = true; }
        //                                        headerRowFromExcel.Add(cell.InlineString.Text.InnerText.ToString());
        //                                    }
        //                                }
        //                                else
        //                                    headerRowFromExcel.Add(string.Empty);
        //                            }
        //                            columnIndex++;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        errorMessage = "Could not read the cells in the excel.";
        //                    }
        //                }
        //                else
        //                {
        //                    errorMessage = "Could not read the cells in the excel.";
        //                }
        //            }
        //            else
        //            {
        //                errorMessage = "Could not find the sheet '" + sheetName + "' in the excel.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
        //        errorMessage += ex.Message + ex.StackTrace;
        //    }
        //    return headerRowFromExcel;
        //}

        private List<string> GetCSVHeaderRowAsString(string filePath, string sheetName, long startRow, out string errorMessage, out bool repeatedColumnFound)
        {
            errorMessage = string.Empty;
            List<string> headerRowFromExcel = null;
            repeatedColumnFound = false;
            try
            {
                headerRowFromExcel = new List<string>();
                using (CsvFileReader reader = new CsvFileReader(filePath))
                {
                    CsvRow row = new CsvRow();
                    reader.ReadRow(row);
                    if (!row.IsNull())
                    {
                        foreach (string cellval in row)
                        {
                            if (!cellval.IsNullOrEmpty())
                            {
                                if (headerRowFromExcel.Contains(cellval)) { repeatedColumnFound = true; }
                                headerRowFromExcel.Add(cellval);
                            }
                        }
                    }
                    else
                    {
                        errorMessage = "Could not read the header in the csv.";
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
                //Soft delete can be implemented in future
                System.IO.File.Delete(tempFilePath);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;

            }
        }
    }
}