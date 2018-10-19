using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Data;
using System.Configuration;
using ENRLReconSystem.DO;
using ENRLReconSystem.BL;
using ENRLReconSystem.Utility;
using System.Reflection;

namespace ERSBackgroundProcess
{
    public class FDRResponseProcessing
    {
        #region Constants & Properties

        int _startRow = 9;
        int _startColumn = 0;

        DataRow _drExcel = null;
        string _contractNo = string.Empty;
        string _transactionType = string.Empty;
        string _colReinstmt = ConfigurationManager.AppSettings["Reinstmt"];
        string _colRetDis = ConfigurationManager.AppSettings["RetDis"];
        string _colRetEnrl = ConfigurationManager.AppSettings["RetEnrl"];
        string _colSCC = ConfigurationManager.AppSettings["SCC"];
        string _colPBP = ConfigurationManager.AppSettings["PBP"];

        string _sourcePath = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.FDRSourcePath);
        string _tempFolderPath = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.FDRTempFolder);
        string _targetPath = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.FDRTargetPath);
        string _errorFolder = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.FDRErrorFolder);
        string _alreadyProcessedFolder = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.FDRAlreadyProcessed);
        string _errorLogFolder = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.FDRErrorLog);

        ExceptionTypes _retValue;
        DOCMN_BackgroundProcessDetails _objDOCMN_BackgroundProcessDetails = new DOCMN_BackgroundProcessDetails();
        DOCMN_BackgroundProcessMaster _objDOCMN_BackgroundProcessMaster = new DOCMN_BackgroundProcessMaster();
        long _lCurrentMasterUserId = StartBackgroundProcess.CurrentMasterUserId;//Current system Id from ADM_User master -- static -- set while starting process
        #endregion

        #region Start FDR Process

        // Author: Amit Thanki
        // Create date: 08/17/2017
        // Method Description: Start FDR Response Processing
        // Method Name: StartFDRResponseProcessing
        /// <summary>
        /// Start FDR Response Processing
        /// </summary>
        public void StartFDRResponseProcessing()
        {
            try
            {
             
                //Copy files from Source Folder to Temp Folder, read all files in Temp folder, validate each excel and moved to target folder.
                GetValidFilesFromSource();

                //Get data from Import Table with Ready Status, read each excel data and insert into Staging table.
                ReadExcelDataAndInsertToStaging();

                //Get Queue Id for all valid record and update the FDR details.
                GetQueueIDAndUpdateFDRDetails();
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, ex.Message, ex.StackTrace.ToString());
            }
        }

        // Author: Amit Thanki
        // Create date: 08/17/2017
        // Method Description: Copy files from Source Folder to Temp Folder, read all files in Temp folder, validate each excel and moved to target folder.
        // Method Name: GetValidFilesFromSource
        /// <summary>
        /// Copy files from Source Folder to Temp Folder, read all files in Temp folder, validate each excel and moved to target folder.
        /// </summary>
        /// <returns></returns>
        public bool GetValidFilesFromSource()
        {
           
            BLFDR objBLFDR = new BLFDR();
            DOGEN_FDR objDOGEN_FDR = new DOGEN_FDR();
            bool isValidExcel = true;
            List<string> resultColumn;
            //  DirectoryInfo dir = new DirectoryInfo(SourcePath);
            DirectoryInfo dir = new DirectoryInfo(_sourcePath);          
            var subFolderNamesTargetPath = Directory.GetDirectories(_sourcePath);
            foreach (var subFolderName in subFolderNamesTargetPath)
            {
                if ((subFolderName != _alreadyProcessedFolder) && (subFolderName != _errorFolder) && (subFolderName != _errorLogFolder))
                {
                    string submmisionId = string.Empty;
                    var subFolder = new DirectoryInfo(subFolderName);
                    submmisionId = subFolder.Name;
                    FileInfo[] files1 = subFolder.GetFiles();

                    if (files1.Count() > 0)
                    {
                        Console.WriteLine("Copying Files Started for " + subFolder.Name + "...");
                    }

                    foreach (FileInfo file1 in files1)
                    {
                        string temppath = Path.Combine(_tempFolderPath, DateTime.Now.ToString("yyyyMMddHHmmss") + file1.Name);
                        // Copy the file.
                        // file.CopyTo(temppath, false);
                        file1.CopyTo(temppath, false);
                        //}
                        Console.WriteLine("Copied File : " + file1.Name);
                        //FileInfo[] files = dir.GetFiles();
                        //string submmisionId = string.Empty;
                        //foreach (FileInfo file in files)
                        //{
                        //    submmisionId = file.Name;
                        //    // Create the path to the new copy of the file.
                        //    string temppath = Path.Combine(TempFolder, file.Name + DateTime.Now.ToString("yyyyMMddHHmmss"));
                        //    // Copy the file.
                        //    file.CopyTo(temppath, false);
                        //}
                        //FileInfo[] TempFiles = new DirectoryInfo(TempFolder).GetFiles();
                        //    foreach (FileInfo Tempfile in TempFiles)
                        //    {
                        using (var document = SpreadsheetDocument.Open(file1.FullName, false))
                        {
                            
                            DataTable dtExcel = new DataTable();
                            WorkbookPart workbookPart = document.WorkbookPart;
                            Sheet sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault();
                            string relationshipId = sheet.Id.Value;
                            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
                            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
                            var sharedStringPart = workbookPart.SharedStringTablePart;

                            SharedStringItem[] values = null;
                            if (sharedStringPart != null)
                                values = sharedStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                            List<string> HeaderColumns = null;
                            int columnIndex = 0;
                            //check sheet data.
                            if (sheet == null)
                                return false;

                            // Heaader Contract Number 
                            //Read header row
                            Row headerRowContractNumber = worksheetPart.Worksheet.Descendants<Row>().Where(r => r.RowIndex == 5).FirstOrDefault();

                            //check for header row details
                            if (headerRowContractNumber == null)
                                return false;


                            //Get header cell array
                            var headerCells1 = headerRowContractNumber.Descendants<Cell>().ToArray();

                            if (headerCells1 == null)
                                return false;

                            List<string> HeaderColumns1 = new List<string>();

                            foreach (var cell in headerCells1)
                            {
                                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && values != null)
                                {
                                    var index = int.Parse(cell.CellValue.Text);
                                    var value = values[index].InnerText;
                                    _contractNo = value.Trim();
                                }
                            }

                            // Heaader Transaction Type 
                            //Read header row
                            Row headerRowTransType = worksheetPart.Worksheet.Descendants<Row>().Where(r => r.RowIndex == 7).FirstOrDefault();

                            //check for header row details
                            if (headerRowTransType == null)
                                return false;

                            #region Get Header Row details

                            //Get header cell array
                            var headerCells2 = headerRowTransType.Descendants<Cell>().ToArray();

                            if (headerCells2 == null)
                                return false;

                            List<string> HeaderColumns2 = new List<string>();

                            foreach (var cell in headerCells2)
                            {
                                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && values != null)
                                {
                                    var index = int.Parse(cell.CellValue.Text);
                                    var value = values[index].InnerText;
                                    _transactionType = value.Trim();
                                }

                            }
                            #endregion
                            if (_transactionType == "REINSTMT")
                            {
                                resultColumn = _colReinstmt.Split('|').ToList();
                            }
                            else if (_transactionType == "RetDis")
                            {
                                resultColumn = _colRetDis.Split('|').ToList();
                            }
                            else if (_transactionType == "RetEnrl")
                            {
                                resultColumn = _colRetEnrl.Split('|').ToList();
                            }
                            else if (_transactionType == "SCC") //SCC
                            {
                                resultColumn = _colSCC.Split('|').ToList();
                            }
                            else //pbp
                            {
                                resultColumn = _colPBP.Split('|').ToList();
                            }
                         

                            //Read header row
                            Row headerRow = worksheetPart.Worksheet.Descendants<Row>().Where(r => r.RowIndex == _startRow).FirstOrDefault();

                            //check for header row details
                            if (headerRow == null)
                                return false;

                            #region Get Header Row details

                            //Get header cell array
                            var headerCells = headerRow.Descendants<Cell>().ToArray();

                            if (headerCells == null)
                                return false;

                            HeaderColumns = new List<string>();

                            foreach (var cell in headerCells)
                            {

                                // Gets the column index of the cell with data
                                int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                cellColumnIndex--; //zero based index
                                if (columnIndex < cellColumnIndex)
                                {
                                    do
                                    {
                                        var value = "";//Insert blank data here;
                                        HeaderColumns.Add(value);
                                        columnIndex++;
                                    }
                                    while (columnIndex < cellColumnIndex);
                                }

                                // The cells contains a string input that is not a formula
                                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && values != null)
                                {
                                    var index = int.Parse(cell.CellValue.Text);
                                    var value = values[index].InnerText;

                                    HeaderColumns.Add(value);

                                    if (!string.IsNullOrEmpty(value))
                                        dtExcel.Columns.Add(value);
                                }
                                else
                                {
                                    if (cell.CellValue != null)
                                    {
                                        HeaderColumns.Add(cell.CellValue.Text);

                                        if (!string.IsNullOrEmpty(cell.CellValue.Text.Trim()))
                                            dtExcel.Columns.Add(cell.CellValue.Text);

                                    }
                                    else if (cell.DataType != null && cell.DataType.Value == CellValues.InlineString && cell.InlineString != null &&
                                          cell.InlineString.Text != null)
                                    {
                                        HeaderColumns.Add(cell.InlineString.Text.InnerText.ToString());

                                        if (!string.IsNullOrEmpty(cell.InlineString.Text.InnerText.ToString().Trim()))
                                            dtExcel.Columns.Add(cell.InlineString.Text.InnerText.ToString());
                                    }
                                    else
                                    {
                                        HeaderColumns.Add(string.Empty);
                                    }
                                }
                              
                                if (dtExcel.Columns.Count == HeaderColumns.Count)
                                {
                                    var columnName = dtExcel.Columns[columnIndex].ColumnName;

                                    if (columnName != null && columnName.Trim() != resultColumn[columnIndex].Trim())
                                    {
                                       
                                        isValidExcel = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    isValidExcel = false;
                                    break;
                                }

                                columnIndex++;
                                //}
                            }
                            #endregion
                        }

                        if (!isValidExcel)
                        {
                            //LOG ERROR
                            // Copy files to Error Folder. if  InValid
                            string FinalPAth = Path.Combine(_errorFolder, DateTime.Now.ToString("yyyyMMddHHmmss") + file1.Name);
                            // Copy the file.
                            //file1.CopyTo(FinalPAth, false);
                            file1.MoveTo(FinalPAth);
                            BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, "Invalid File :- " + file1.Name, "Invalid File");
                            //FileStream fs = File.Create(ErrorLog);
                            //Byte[] title = new UTF8Encoding(true).GetBytes("Invalid File");
                            //fs.Write(title, 0, title.Length);
                            //byte[] author = new UTF8Encoding(true).GetBytes("Column Missing for Transaction Type");
                            //fs.Write(author, 0, author.Length);
                            // dtExcel = null;
                        }
                        else
                        {
                            // Copy files to Target Folder. if Valid                      
                            //string FinalPAth = Path.Combine(TargetPath, Tempfile.Name);
                            //// Copy the file.
                            //Tempfile.CopyTo(FinalPAth, false);
                            //objDOGEN_FDR.ExcelFileName = Tempfile.FullName;
                            //objDOGEN_FDR.ExcelFilePath = FinalPAth;
                            //objDOGEN_FDR.TransactionTypeLValue = TransactionType;
                            //objDOGEN_FDR.SubmissionID = Convert.ToInt64(submmisionId);
                            //objBLFDR.InsertFDRBulkImport(objDOGEN_FDR, out string errormessage);
                            string FinalPAth = Path.Combine(_targetPath, DateTime.Now.ToString("yyyyMMddHHmmss") + file1.Name);
                            // Copy the file.
                            file1.CopyTo(FinalPAth, false);
                            objDOGEN_FDR.ExcelFileName = file1.Name;
                            objDOGEN_FDR.ExcelFilePath = FinalPAth;
                            objDOGEN_FDR.TransactionTypeLValue = _transactionType;
                            objDOGEN_FDR.ContractNumber = _contractNo;
                            objDOGEN_FDR.SubmissionID = Convert.ToString(submmisionId);
                            //objDOCMN_BackgroundProcessDetails.UploadFileName = file1.FullName;
                            //objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Success;
                            //InsertBGPDetails(objDOCMN_BackgroundProcessDetails);

                            //if (!objBLFDR.InsertFDRBulkImport(objDOGEN_FDR, out string errormessage))
                            //{
                            //    string ErrorPath = Path.Combine(ErrorFolder, DateTime.Now.ToString("yyyyMMddHHmmss") + file1.Name);
                            //    file1.CopyTo(ErrorPath, false);
                            //    BLCommon.LogError(2, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Application, (long)ExceptionTypes.Uncategorized, "Transaction Type Or Contract Number is Invalid" + DateTime.Now.ToString("yyyyMMddHHmmss") + file1.Name, Convert.ToString(errormessage));
                            //}                               
                            //drExcel = null;
                        }
                        // }--
                        if (isValidExcel)
                        {
                            if (!objBLFDR.InsertFDRBulkImport(objDOGEN_FDR, out string errormessage))
                            {
                                string ErrorPath = Path.Combine(_errorFolder, DateTime.Now.ToString("yyyyMMddHHmmss") + file1.Name);
                                // file1.CopyTo(ErrorPath, false);                               
                                file1.MoveTo(ErrorPath);
                                //file1.Delete(temppath);
                                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, "Transaction Type Or Contract Number is Invalid :- " + file1.Name, Convert.ToString(errormessage));
                                //FileStream fs = File.Create(ErrorLog);
                                //Byte[] title = new UTF8Encoding(true).GetBytes("Invalid File");
                                //fs.Write(title, 0, title.Length);
                                //byte[] author = new UTF8Encoding(true).GetBytes("Transaction Type Or Contract Number is Invalid");
                                //fs.Write(author, 0, author.Length);
                            }
                            else
                            {
                                string AlreadyProcessedPath = Path.Combine(_alreadyProcessedFolder, DateTime.Now.ToString("yyyyMMddHHmmss") + file1.Name);
                                file1.MoveTo(AlreadyProcessedPath);
                            }
                        }
                        _drExcel = null;


                        //} // --
                    }
                    isValidExcel = true;

                }
                //return isValidExcel;
            } //---
            return isValidExcel;

        }

        // Author: Amit Thanki
        // Create date: 08/17/2017
        // Method Description: Get data from Import Table with Ready Status, read each excel data and insert into Staging table.
        // Method Name: ReadExcelDataAndInsertToStaging
        /// <summary>
        /// Get data from Import Table with Ready Status, read each excel data and insert into Staging table.
        /// </summary>
        /// <returns></returns>
        public bool ReadExcelDataAndInsertToStaging()
        {
            BLFDR objBLFDR = new BLFDR();
            DOGEN_FDR objDOGEN_FDR = new DOGEN_FDR();
            List<string> resultColumn;
            //   DirectoryInfo dir = new DirectoryInfo(TargetPath);
            // FileInfo[] files = dir.GetFiles();
            // string[] Targetfiles = Directory.GetFiles(TargetPath);
            // foreach (string file in Targetfiles)
            // {
            //objBLFDR.GetBulkImportID(out long BulkImportID,out String FilePath, out string ErrorMessage);
            if (GetReadyToProcess(out long BulkImportID, out string FilePath))
            {

                do
                {
                    InsertBGPMaster(out long BgpMasterID);
                    //if (!ProcessLockRecord(_lCurrentMasterUserId, (long)ScreenType.FDR, BulkImportID, true))
                    //{
                    //    _objDOCMN_BackgroundProcessDetails.GEN_FDRUploadStagingRef = BulkImportID;
                    //    _objDOCMN_BackgroundProcessDetails.UploadFileName = FilePath;
                    //    _objDOCMN_BackgroundProcessDetails.CMN_BackgroundProcessMasterRef = BgpMasterID;
                    //    _objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                    //    _objDOCMN_BackgroundProcessDetails.FailureReason = "FDR Record Not Locked";
                    //    InsertBGPDetails(_objDOCMN_BackgroundProcessDetails);
                    //    _objDOCMN_BackgroundProcessMaster.CMN_BackgroundProcessMasterId = BgpMasterID;
                    //    _objDOCMN_BackgroundProcessMaster.BGPStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                    //    _objDOCMN_BackgroundProcessMaster.TotalRecordProcessed = 0;
                    //    _objDOCMN_BackgroundProcessMaster.LastUpdatedByRef = StartBackgroundProcess.CurrentMasterUserId;
                    //    UpdateBGPMaster(_objDOCMN_BackgroundProcessMaster);
                    //    return false;
                    //}

                    using (var document = SpreadsheetDocument.Open(FilePath, false))
                    {
                        Console.WriteLine("File Path " + FilePath + "...");
                        //DataTable dtExcel = new DataTable("TVP_FDRStaging");
                        DataTable dtExcel = new DataTable();
                        WorkbookPart workbookPart = document.WorkbookPart;
                        Sheet sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault();
                        string relationshipId = sheet.Id.Value;
                        WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
                        SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
                        var sharedStringPart = workbookPart.SharedStringTablePart;

                        SharedStringItem[] values = null;
                        if (sharedStringPart != null)
                            values = sharedStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                        List<string> HeaderColumns = null;
                        List<string> currentRowColumns = null;
                        int columnIndex = 0;
                        //check sheet data.
                        if (sheet == null)
                            return false;

                        // Heaader Contract Number 
                        //Read header row
                        Row headerRowContractNumber = worksheetPart.Worksheet.Descendants<Row>().Where(r => r.RowIndex == 5).FirstOrDefault();

                        //check for header row details
                        if (headerRowContractNumber == null)
                            return false;

                        #region Get Contract Number details

                        //Get header cell array
                        var headerCells1 = headerRowContractNumber.Descendants<Cell>().ToArray();

                        if (headerCells1 == null)
                            return false;

                        List<string> HeaderColumns1 = new List<string>();

                        foreach (var cell in headerCells1)
                        {
                            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && values != null)
                            {
                                var index = int.Parse(cell.CellValue.Text);
                                var value = values[6].InnerText;
                                _contractNo = value;
                            }
                        }

                        #endregion

                        // Heaader Transaction Type 
                        //Read header row
                        Row headerRowTransType = worksheetPart.Worksheet.Descendants<Row>().Where(r => r.RowIndex == 7).FirstOrDefault();

                        //check for header row details
                        if (headerRowTransType == null)
                            return false;

                        #region Get Transaction Type details

                        //Get header cell array
                        var headerCells2 = headerRowTransType.Descendants<Cell>().ToArray();

                        if (headerCells2 == null)
                            return false;

                        List<string> HeaderColumns2 = new List<string>();

                        foreach (var cell in headerCells2)
                        {
                            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && values != null)
                            {
                                var index = int.Parse(cell.CellValue.Text);
                                var value = values[index].InnerText;
                                _transactionType = value.Trim();
                            }
                        }

                        #endregion

                        if (_transactionType == "REINSTMT")
                        {
                            resultColumn = _colReinstmt.Split('|').ToList();
                        }
                        else if (_transactionType == "RetDis")
                        {
                            resultColumn = _colRetDis.Split('|').ToList();
                        }
                        else if (_transactionType == "RetEnrl")
                        {
                            resultColumn = _colRetEnrl.Split('|').ToList();
                        }
                        else if (_transactionType == "SCC") //SCC
                        {
                            resultColumn = _colSCC.Split('|').ToList();
                        }
                        else //PBP
                        {
                            resultColumn = _colPBP.Split('|').ToList();
                        }

                        Console.WriteLine("Transaction Type " + _transactionType + "...");

                        // Main 

                        //Read header row
                        Row headerRow = worksheetPart.Worksheet.Descendants<Row>().Where(r => r.RowIndex == _startRow).FirstOrDefault();

                        //check for header row details
                        if (headerRow == null)
                            return false;

                        #region Get Header Row details

                        //Get header cell array
                        var headerCells = headerRow.Descendants<Cell>().ToArray();

                        if (headerCells == null)
                            return false;

                        HeaderColumns = new List<string>();
                        // bool isValidExcel = true;

                        foreach (var cell in headerCells)
                        {
                            //foreach (var cellColumn in resultColumn)
                            //{                            
                            // Gets the column index of the cell with data
                            int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                            cellColumnIndex--; //zero based index
                            if (columnIndex < cellColumnIndex)
                            {
                                do
                                {
                                    var value = "";//Insert blank data here;
                                    HeaderColumns.Add(value);
                                    columnIndex++;
                                }
                                while (columnIndex < cellColumnIndex);
                            }

                            // The cells contains a string input that is not a formula
                            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && values != null)
                            {
                                var index = int.Parse(cell.CellValue.Text);
                                var value = values[index].InnerText;

                                HeaderColumns.Add(value);

                                if (!string.IsNullOrEmpty(value))
                                    dtExcel.Columns.Add(value);
                            }
                            else
                            {
                                if (cell.CellValue != null)
                                {
                                    HeaderColumns.Add(cell.CellValue.Text);

                                    if (!string.IsNullOrEmpty(cell.CellValue.Text.Trim()))
                                        dtExcel.Columns.Add(cell.CellValue.Text);

                                }
                                else if (cell.DataType != null && cell.DataType.Value == CellValues.InlineString && cell.InlineString != null &&
                                      cell.InlineString.Text != null)
                                {
                                    HeaderColumns.Add(cell.InlineString.Text.InnerText.ToString());

                                    if (!string.IsNullOrEmpty(cell.InlineString.Text.InnerText.ToString().Trim()))
                                        dtExcel.Columns.Add(cell.InlineString.Text.InnerText.ToString());
                                }
                                else
                                {
                                    HeaderColumns.Add(string.Empty);
                                }
                            }
                        }
                        #endregion

                        //Read last row
                        Row lastRow = worksheetPart.Worksheet.Descendants<Row>().LastOrDefault();


                        //Loop all excel rows to get column values mapping with form fields.
                        for (int i = _startRow + 1; i <= lastRow.RowIndex; i++)
                        {


                            //Get current row from excel.
                            Row currentRow = worksheetPart.Worksheet.Descendants<Row>().Where(r => r.RowIndex == i).FirstOrDefault();

                            if (currentRow == null)
                                continue;

                            #region Read excel row and get the value

                            //Get current row columns
                            Cell[] currentRowCells = currentRow.Descendants<Cell>().ToArray();
                            columnIndex = 0;

                            if (currentRowCells == null)
                                continue;

                            currentRowColumns = new List<string>();

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
                            //} //---
                            #endregion


                            if (!CheckRowHasData(currentRowColumns))
                                continue;

                            // //headerNameToFillDBColumn = string.Empty;
                            // //dtExcel.Columns.Add("Contract Number");
                            // //dtExcel.Columns.Add("Transaction Type");
                            //if (TransactionType == "REINSTMT")
                            //{
                            //    dtExcel.Columns.Add("Requested SCC");
                            //    dtExcel.Columns.Add("Requested ZIP");
                            //}
                            //else if (TransactionType == "RetDis")
                            //{
                            //    dtExcel.Columns.Add("Requested PBP");
                            //    dtExcel.Columns.Add("Requested CMS Segment");
                            //    dtExcel.Columns.Add("Application Receipt Date");
                            //    dtExcel.Columns.Add("Requested SCC");
                            //    dtExcel.Columns.Add("Requested ZIP");
                            //}
                            //else if (TransactionType == "RetEnrl")
                            //{
                            //    dtExcel.Columns.Add("Application Receipt Date");
                            //    dtExcel.Columns.Add("Requested SCC");
                            //    dtExcel.Columns.Add("Requested ZIP");
                            //}
                            //else if (TransactionType == "SCC") //scc                            
                            //{
                            //    dtExcel.Columns.Add("Requested CMS Segment");
                            //    dtExcel.Columns.Add("Requested PBP");
                            //    dtExcel.Columns.Add("Requested Election Period");
                            //    dtExcel.Columns.Add("Application Receipt Date");
                            //}
                            //else //pbp
                            //{
                            //    dtExcel.Columns.Add("Requested SCC");
                            //    dtExcel.Columns.Add("Requested ZIP");
                            //}
                            _drExcel = dtExcel.NewRow();

                            for (int j = _startColumn; j <= currentRowColumns.Count - 1; j++)
                            {
                                if (j <= 12)
                                {
                                    if (currentRowColumns[j] != "")
                                    {

                                        //drExcel["Contract Number"] = ConstractNo;
                                        //drExcel["Transaction Type"] = TransactionType;

                                        if (_transactionType == "REINSTMT")
                                        {
                                            if ((j == 3) || (j == 4) || (j == 8) || (j == 9) || (j == 10))
                                            {
                                                string a = currentRowColumns[j];
                                                currentRowColumns[j] = GetDatetimeValue(a);
                                            }
                                        }
                                        else if (_transactionType == "RetDis")
                                        {
                                            if ((j == 3) || (j == 4) || (j == 6) || (j == 7))
                                            {
                                                string a = currentRowColumns[j];
                                                currentRowColumns[j] = GetDatetimeValue(a);
                                            }
                                        }
                                        else if (_transactionType == "RetEnrl")
                                        {
                                            if ((j == 3) || (j == 4) || (j == 8) || (j == 9))
                                            {
                                                string a = currentRowColumns[j];
                                                currentRowColumns[j] = GetDatetimeValue(a);
                                            }
                                        }
                                        else if (_transactionType == "SCC") //SCC
                                        {
                                            if ((j == 3) || (j == 4) || (j == 7) || (j == 8))
                                            {
                                                string a = currentRowColumns[j];
                                                currentRowColumns[j] = GetDatetimeValue(a);
                                            }
                                        }
                                        else //PBP
                                        {
                                            if ((j == 3) || (j == 4) || (j == 8) || (j == 9) || (j == 10))
                                            {
                                                string a = currentRowColumns[j];
                                                currentRowColumns[j] = GetDatetimeValue(a);
                                            }
                                        }
                                        _drExcel[j] = currentRowColumns[j];

                                    }
                                }

                            }
                            dtExcel.Rows.Add(_drExcel);
                        }
                        //dtExcel.Rows.Add(drExcel);
                        objDOGEN_FDR.dtExcelData = dtExcel;
                        objDOGEN_FDR.FDRBulkImportID = BulkImportID;
                        ////Perform Database Operations 
                        //objBLFDR.InsertFDRStagingData(objDOGEN_FDR, out string errormessage);
                    }
                    // Insert Excel data to FDR Staging Table.                  
                    Console.WriteLine("Data Start Insert into FDR Staging Table with Bulk Import ID:-" + BulkImportID + "..." );
                    if (!InsertExcelToStaging(objDOGEN_FDR, out string message))
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, "Getting Error While Insert Excel data for file :- " + FilePath, message.ToString());
                    objDOGEN_FDR.dtExcelData = null;
                    _objDOCMN_BackgroundProcessDetails.GEN_FDRUploadStagingRef = BulkImportID;
                    _objDOCMN_BackgroundProcessDetails.UploadFileName = FilePath;
                    if (message != "")
                    {
                        _objDOCMN_BackgroundProcessDetails.CMN_BackgroundProcessMasterRef = BgpMasterID;
                        _objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                        _objDOCMN_BackgroundProcessDetails.FailureReason = message;
                    }
                    else
                    {
                        _objDOCMN_BackgroundProcessDetails.CMN_BackgroundProcessMasterRef = BgpMasterID;
                        _objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Success;
                    }
                    InsertBGPDetails(_objDOCMN_BackgroundProcessDetails);

                    // Validation Sp
                    Console.WriteLine("Validation SP will Call for BulkImport ID:-" + BulkImportID + "...");
                    if (!customValidationSP(objDOGEN_FDR.FDRBulkImportID, out String ErrorMessage))
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, "Getting Error While Validate Excel Data for file :- " + FilePath, ErrorMessage.ToString());

                    //Check Total Record Count and Invalid Count Record
                    Console.WriteLine("Check Total Record count and Invalid Record Count for  BulkImport ID:-" + BulkImportID + "...");
                    if (!CheckValidRecordCount(objDOGEN_FDR.FDRBulkImportID, out String Message))
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, "Getting Error While Count Record For File :- " + FilePath, Message.ToString());

                    //Process Unlock Record
                    if (!ProcessUnlockRecord((long)ScreenType.FDR, BulkImportID))
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, "Getting Error While Unlock FDR Record", "");
                    _objDOCMN_BackgroundProcessMaster.CMN_BackgroundProcessMasterId = BgpMasterID;
                    _objDOCMN_BackgroundProcessMaster.BGPStatusLkup = (long)BackgroundProcessRecordStatus.Success;
                    _objDOCMN_BackgroundProcessMaster.TotalRecordProcessed = 0;
                    _objDOCMN_BackgroundProcessMaster.LastUpdatedByRef = StartBackgroundProcess.CurrentMasterUserId;
                    UpdateBGPMaster(_objDOCMN_BackgroundProcessMaster);
                } while (GetReadyToProcess(out BulkImportID, out FilePath));
                //}
                //{              
            }
            else
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, "No Excel File To Read", "");
                return false;
            }
            // }
            return false;
        }

        // Author: Amit Thanki
        // Create date: 08/17/2017
        // Method Description: Get Queue Id for all valid record and update the FDR details.
        // Method Name: GetQueueIDAndUpdateFDRDetails
        /// <summary>
        /// Get Queue Id for all valid record and update the FDR details.
        /// </summary>
        /// <returns></returns>
        public bool GetQueueIDAndUpdateFDRDetails()
        {
            bool isSuccess = false;
            if (GetQueueID(out long QueueID, out string CMSPRocessDate, out string DispositionCode, out string DispositionCodeDesc, out long TransactionType, out string ErrorMessage))
            {
                do
                {
                    if (QueueID != 0)
                    {
                        Console.WriteLine("Found Queue Based on HICN,ContractNumber,Resolution and Effective Date and Trying to Lock:- " + QueueID + "...");
                        if (ProcessLockRecord(_lCurrentMasterUserId, (long)ScreenType.Queue, QueueID, false))
                        {
                            Console.WriteLine("Record Successfully Locked and will Update Queue and Actions" + QueueID + "...");
                            UpdateFDRQueueAction(QueueID, CMSPRocessDate, DispositionCode, DispositionCodeDesc, TransactionType, out ErrorMessage);
                            Console.WriteLine("Update FDR Queue Action " + QueueID + "...");
                            if (ErrorMessage != "")
                            {
                                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, "Getting Error while Update RPR Action Queue = " + QueueID, ErrorMessage.ToString());
                            }
                            if (!ProcessUnlockRecord((long)ScreenType.Queue, QueueID))
                                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, "Getting Error While Unlock Queue Record", "");
                        } 
                    }
                    else
                    {
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, "Not able to find related ERS ID", ErrorMessage.ToString());
                    }
                } while ((GetQueueID(out QueueID, out CMSPRocessDate, out DispositionCode, out DispositionCodeDesc, out TransactionType, out ErrorMessage)));
            }
            return isSuccess;
        }

        #endregion

        #region Insert & Update to BGP Tables

        // Author: Amit Thanki
        // Create date: 08/17/2017
        // Method Description: Insert a row into BGP Master for tracking
        // Method Name: InsertBGPMaster
        /// <summary>
        /// Insert a row into BGP Master for tracking
        /// </summary>
        /// <param name="bgpMasterId"></param>
        /// <returns>bool</returns>
        public bool InsertBGPMaster(out long bgpMasterId)
        {
            bool isSuccess = false;

            BLCommon objCommon = new BLCommon();
            _retValue = objCommon.InsertBackgroundProcessMaster((long)BackgroundProcessType.FDRResponseProcessing, StartBackgroundProcess.CurrentMasterUserId, out bgpMasterId, out string errorMessage);

            if (_retValue == ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                return isSuccess;

            return isSuccess;
        }

        // Author: Amit Thanki
        // Create date: 08/17/2017
        // Method Description: Insert a row into BGP Details for tracking
        // Method Name: InsertBGPDetails
        /// <summary>
        /// Insert a row into BGP Details for tracking
        /// </summary>
        /// <param name="objDOCMN_BackgroundProcessDetails"></param>
        /// <returns>bool</returns>
        public bool InsertBGPDetails(DOCMN_BackgroundProcessDetails objDOCMN_BackgroundProcessDetails)
        {
            bool isSuccess = false;
            //Insert BGP Master Row
            BLCommon objCommon = new BLCommon();
            //Insert BGP Master Row
            objCommon.InsertBackgroundProcessDetails(objDOCMN_BackgroundProcessDetails, out string ErrorMessage);
            return isSuccess;

        }

        // Author: Amit Thanki
        // Create date: 08/17/2017
        // Method Description: Update BGP Master row with status & duration along with record count processed
        // Method Name: UpdateBGPMaster
        /// <summary>
        /// Update BGP Master row with status & duration along with record count processed
        /// </summary>
        /// <param name="objDOCMN_BackgroundProcessMaster"></param>
        /// <returns>bool</returns>
        public bool UpdateBGPMaster(DOCMN_BackgroundProcessMaster objDOCMN_BackgroundProcessMaster)
        {
            bool isSuccess = false;
            //Insert BGP Master Row
            BLCommon objCommon = new BLCommon();
            //Insert BGP Master Row
            objCommon.UpdateBackgroundProcessMaster(objDOCMN_BackgroundProcessMaster, out string ErrorMessage);
            return isSuccess;

        }

        #endregion

        #region Support Method for Reading and getting data from excel


        private static bool CheckRowHasData(List<string> currentRowCells)
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

        public string GeDelimmetedString(List<string> strList, string delimiter)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string str in strList)
            {
                if (sb.Length > 0)
                {
                    sb.Append(delimiter);
                }
                sb.Append(str);
            }
            return sb.ToString();
        }

        public static int? GetColumnIndexFromName(string columnName)
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

        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            string date = match.Value;

            return match.Value;
        }

        private string GetDatetimeValue(string currentColumnValue)
        {
            try
            {
                int letterCounter = Regex.Matches(currentColumnValue, @"[a-zA-Z]").Count;
                if (letterCounter > 0)
                {
                    DateTime dateTime;
                    if (DateTime.TryParse(currentColumnValue, out dateTime))
                    {
                        currentColumnValue = dateTime.Date.ToString();
                    }
                    else
                    {
                        //Some error occured
                        currentColumnValue = string.Empty;
                    }
                }
                else
                {
                    if (currentColumnValue.Contains('.'))//for date with time format
                    {

                        currentColumnValue = DateTime.FromOADate(Convert.ToDouble(currentColumnValue)).Date.ToString();
                    }
                    else
                    {
                        int dateExcelSerialNumber = Convert.ToInt32(currentColumnValue);
                        if (dateExcelSerialNumber > 59) dateExcelSerialNumber -= 1;
                        currentColumnValue = new DateTime(1899, 12, 31).AddDays(dateExcelSerialNumber).Date.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                //Error Occured
                currentColumnValue = string.Empty;
            }

            return currentColumnValue;
        }

        public bool InsertExcelToStaging(DOGEN_FDR objDOGEN_FDR, out string ErrorMessage)
        {
            bool isSuccess = false;
            ErrorMessage = string.Empty;
            try
            {
                BLFDR objBLFDR = new BLFDR();
                _retValue = objBLFDR.InsertFDRStagingData(objDOGEN_FDR, out ErrorMessage);
                if (_retValue != ExceptionTypes.Success || !string.IsNullOrEmpty(ErrorMessage))
                {
                    return isSuccess;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                ErrorMessage += ex.Message + ex.StackTrace;
            }

            return isSuccess;

        }

        public bool GetReadyToProcess(out long BulkImportID, out String FilePath)
        {
            BLFDR objBLFDR = new BLFDR();
            DOGEN_FDR objDOGEN_FDR = new DOGEN_FDR();
            objBLFDR.GetBulkImportID(out BulkImportID, out FilePath, out string ErrorMessage);
            if (BulkImportID != 0)
            {
                return true;
            }
            else
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, ErrorMessage.ToString(), ErrorMessage.ToString());
            return false;
        }

        //Process Lock Record
        public bool ProcessLockRecord(long UserID, long ScreenType, long CaseID, bool IsRelockRequired)
        {
            UIRecordsLock objRecordsLocked = new UIRecordsLock();
            bool isSuccess = false;
            try
            {
                BLCommon objBLCommon = new BLCommon();
                _retValue = objBLCommon.GetLockedRecordOrLockRecord(UserID, ScreenType, CaseID, IsRelockRequired, out objRecordsLocked);
                if (_retValue == ExceptionTypes.Success && objRecordsLocked.Status == (long)ExceptionTypes.Success && objRecordsLocked.CreatedByRef == UserID)
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), ex.Message.ToString());
                //ErrorMessage += ex.Message + ex.StackTrace;
            }

            return isSuccess;
        }

        //Process Unlock Record
        public bool ProcessUnlockRecord(long ScreenType, long CaseID)
        {
            UIRecordsLock objRecordsLocked = new UIRecordsLock();
            bool isSuccess = false;
            try
            {
                BLCommon objBLCommon = new BLCommon();
                _retValue = objBLCommon.UnlockRecord(ScreenType, CaseID);
                if (_retValue == ExceptionTypes.Success && objRecordsLocked.Status == (long)ExceptionTypes.Success)
                {
                    isSuccess = true;
                    Console.WriteLine("Record Successfully Unlocked" + CaseID + "...");
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), ex.Message.ToString());
                //ErrorMessage += ex.Message + ex.StackTrace;
            }

            return isSuccess;
        }

        public bool customValidationSP(long BulkImportID, out string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            bool isSuccess = false;
            try
            {
                BLFDR objBLFDR = new BLFDR();
                objBLFDR.ValidationSP(BulkImportID, out ErrorMessage);
                if (_retValue != ExceptionTypes.Success || !string.IsNullOrEmpty(ErrorMessage))
                {
                    return isSuccess;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                // ErrorMessage += ex.Message + ex.StackTrace;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), ErrorMessage.ToString());

            }

            return isSuccess;
        }

        public bool CheckValidRecordCount(long BulkImportID, out string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            bool isSuccess = false;
            try
            {
                BLFDR objBLFDR = new BLFDR();
                objBLFDR.CheckValidRecordCount(BulkImportID, out ErrorMessage);
                if (_retValue != ExceptionTypes.Success || !string.IsNullOrEmpty(ErrorMessage))
                {
                    return isSuccess;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                // ErrorMessage += ex.Message + ex.StackTrace;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), ErrorMessage.ToString());

            }

            return isSuccess;
        }

        public bool GetQueueID(out long QueueID, out string CMSProcessDate, out string DispositionCode, out string DispositionCodeDescription, out long TransactionTypeLkup, out string errorMessage)
        {
            QueueID = 0;
            CMSProcessDate = string.Empty;
            DispositionCode = string.Empty;
            DispositionCodeDescription = string.Empty;
            TransactionTypeLkup = 0;
            errorMessage = string.Empty;
            bool isSuccess = false;
            try
            {
                BLFDR objBLFDR = new BLFDR();
                objBLFDR.GetQueueID(out QueueID, out CMSProcessDate, out DispositionCode, out DispositionCodeDescription, out TransactionTypeLkup, out errorMessage);
                if (_retValue != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
                {
                    return isSuccess;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                // ErrorMessage += ex.Message + ex.StackTrace;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), errorMessage.ToString());

            }

            return isSuccess;
        }

        public bool UpdateFDRQueueAction(long QueueID, string CMSProcessDate, string DispositionCode, string DispositionCodeDescription, long TransactionTypeLkup, out string errorMessage)
        {

            errorMessage = string.Empty;
            bool isSuccess = false;
            try
            {
                BLFDR objBLFDR = new BLFDR();
                objBLFDR.UpdateRPRQueue(QueueID, CMSProcessDate, DispositionCode, DispositionCodeDescription, TransactionTypeLkup, out errorMessage);
                if (_retValue != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
                {
                    return isSuccess;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                // ErrorMessage += ex.Message + ex.StackTrace;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRResponseProcessing, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), errorMessage.ToString());

            }

            return isSuccess;
        }

        #endregion
    }
}

/*--------------- FDR Response Processing Steps for Understanding --------------------------
// 1st Step
//Getting all files from Source --done
//Move into a temp path -- done
//Read temp path, go through each excel one by one (Open excel, get Transaction Type, read column and compare config) --done
//Validate whether excel valid or not depending upon Transaction Type. --done
//If Valid - Move file from Temp to Destination folder + Insert a record into ImportMaster table  (FileName, FilePath, Sts -> ready for Import) + in Source Folder _ Move the file to Destination Folder.
//If Invalid - Delete from temp folder + In Source Folder _ Move file to Error folder _ Log the error details into log file and put it into ErrorLog folder.


// 2nd Step
//Call getReadyToProcess()  --Get top 1 import id, excel file path where Sts - Ready and Not locked  done
//If Not got any record - Stop the process
//Else 
//Lock the import id --done
//If not able to lock, proceed for next record (Do...while - call again getReadyToProcess())-- done
//If Record locked success - Use excel file path and open the excel  --  done
//Read all data from excel - Insert into staging table  
//Run validation query - Mark Invalid, Duplicate, Isprocess columns
//Pull rest of valid records from staging and start reading value and convert it to appropriate value.
//If anything not able to convert to appropriate value - Mark it again as invalid.
//Now rest all are your real valid record (Response) which can be join with Queue table.
//Unlock the record


//3rd Step
//Top 1 - Query IsProcessed = 0 records from staging table
//do
//Get queue id from queue table for staging record
//Try to lock the queue id 
//If lock not success - Don't do anything, go for next staging record
//If lock success - Update the queue record and Update the staging row as IsProcess=1 and Unlock the queue

//select top 1 with is proceeed 0 or null and get hic,Lastname,FirstName 
//Call another sp locked Queue table and Update Queue and Unlock Queue table with id.
//update staging table with with isproceed  1.


//  step  PBP
//check Queu
//get Queue and Locke Queue then after update action and Update FDR Import table.


//FDRFileDownload
//FDRFileProcessing
//FDRResponseUpdate

*/
