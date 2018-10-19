using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.BL;
using ENRLReconSystem.Utility;
using OfficeOpenXml;
using ENRLReconSystem.DO;
using System.Reflection;
using System.Collections;

namespace ERSBackgroundProcess
{
    public class ExcelCreation
    {
        long _lCurrentMasterUserId = StartBackgroundProcess.CurrentMasterUserId;
        public ExceptionTypes StartExcelCreation(ExcelCreationConfig objExcelCreationConfig, out long lRecordsProcessed, out List<long> CaseIds)
        {
            CaseIds = new List<long>();
            Console.WriteLine("Excel Creation Started...");
            ExceptionTypes result = ExceptionTypes.Success;
            lRecordsProcessed = 0;//variable to keep track of total records processed
            try
            {
                BLCommon objBLCommon = new BLCommon();

                //List of excel sheets
                List<FDRSubmissionSheetData> lstExcelSheets = new List<FDRSubmissionSheetData>();

                //check for submission category
                if (objExcelCreationConfig.SubmissionCategory == FDRSubmissionCategory.SCC)
                {
                    FDRSubmissionSheetData SCCSheet = new FDRSubmissionSheetData();

                    //assign sheet position in workbook for all sheets
                    SCCSheet.ISheetNumber = (int)SCCFDRSubmissionExcelSheets.SCC;

                    //assign startting rows for each excel sheet
                    SCCSheet.IStartRow = objExcelCreationConfig.StartingRows[SCCSheet.ISheetNumber - 2];

                    //popualate data in excel sheets
                    //for each pdf file add row in one of the excel sheet data objects
                    foreach (var item in objExcelCreationConfig.LstPdffiles)
                    {
                        //create object to insert in BGP process details for each row created in excel sheet
                        DOCMN_BackgroundProcessDetails objDOCMN_BackgroundProcessDetails = new DOCMN_BackgroundProcessDetails();
                        objDOCMN_BackgroundProcessDetails.CMN_BackgroundProcessMasterRef = objExcelCreationConfig.LbgpMasterId;
                        objDOCMN_BackgroundProcessDetails.UploadFileName = item.Name;

                        //get contract name and HICN from Pdf file name
                        string[] arrContractHicn = item.Name.Replace(".pdf", "").Split('-');
                        //get create row for Contract and HICN
                        result = CreateRowData(arrContractHicn[0], arrContractHicn[1], objExcelCreationConfig.SubmissionCategory, out FDRSubmissionRow newRow, out long lActionRequeseted, out string errorMessage);

                        //if row is created succesfully
                        if (result == ExceptionTypes.Success)
                        {
                            SCCSheet.LstRowData.Add(newRow);
                            //add details to BGP deatils 
                            objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Success;
                            objDOCMN_BackgroundProcessDetails.GEN_QueueRef = newRow.GEN_QueueId;
                            //increase count of total records processed
                            lRecordsProcessed++;
                            CaseIds.Add(newRow.GEN_QueueId);
                        }
                        else
                        {
                            //if row creation failed update BGP deatils with fail result
                            objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                            objDOCMN_BackgroundProcessDetails.GEN_QueueRef = 0;
                            objDOCMN_BackgroundProcessDetails.FailureReason = errorMessage;
                        }
                        //insert into BGP details table
                        objBLCommon.InsertBackgroundProcessDetails(objDOCMN_BackgroundProcessDetails, out errorMessage);
                    }
                    //once finished processing and creating rows for all the pdf files
                    //add excel sheets to list Excel Sheets if there are rows in the Sheet data
                    if (SCCSheet.LstRowData.Count > 0)
                        lstExcelSheets.Add(SCCSheet);
                }
                else
                {
                    //excel sheets objects based on action requested
                    FDRSubmissionSheetData RetDisenrlSheet = new FDRSubmissionSheetData();
                    FDRSubmissionSheetData PBPSheet = new FDRSubmissionSheetData();
                    FDRSubmissionSheetData RetEnrlSheet = new FDRSubmissionSheetData();
                    FDRSubmissionSheetData ReInstmtSheet = new FDRSubmissionSheetData();

                    //assign sheet position in workbook for all sheets
                    RetDisenrlSheet.ISheetNumber = (int)FDRSubmissionExcelSheets.Ret_Disenrl;
                    PBPSheet.ISheetNumber = (int)FDRSubmissionExcelSheets.PBP;
                    RetEnrlSheet.ISheetNumber = (int)FDRSubmissionExcelSheets.Ret_Enrl;
                    ReInstmtSheet.ISheetNumber = (int)FDRSubmissionExcelSheets.REINSTMT;

                    //assign startting rows for each excel sheet
                    RetDisenrlSheet.IStartRow = objExcelCreationConfig.StartingRows[RetDisenrlSheet.ISheetNumber - 2];
                    PBPSheet.IStartRow = objExcelCreationConfig.StartingRows[PBPSheet.ISheetNumber - 2];
                    RetEnrlSheet.IStartRow = objExcelCreationConfig.StartingRows[RetEnrlSheet.ISheetNumber - 2];
                    ReInstmtSheet.IStartRow = objExcelCreationConfig.StartingRows[ReInstmtSheet.ISheetNumber - 2];

                    //popualate data in excel sheets
                    //for each pdf file add row in one of the excel sheet data objects
                    foreach (var item in objExcelCreationConfig.LstPdffiles)
                    {
                        //create object to insert in BGP process details for each row created in excel sheet
                        DOCMN_BackgroundProcessDetails objDOCMN_BackgroundProcessDetails = new DOCMN_BackgroundProcessDetails();
                        objDOCMN_BackgroundProcessDetails.CMN_BackgroundProcessMasterRef = objExcelCreationConfig.LbgpMasterId;
                        objDOCMN_BackgroundProcessDetails.UploadFileName = item.Name;

                        //get contract name and HICN from Pdf file name
                        string[] arrContractHicn = item.Name.Replace(".pdf", "").Split('-');
                        //get create row for Contract and HICN
                        result = CreateRowData(arrContractHicn[0], arrContractHicn[1], objExcelCreationConfig.SubmissionCategory, out FDRSubmissionRow newRow, out long lResolution, out string errorMessage);

                        //if row is created succesfully
                        if (result == ExceptionTypes.Success)
                        {
                            //add row to one of excel sheet based on action requested
                            switch (lResolution)
                            {
                                case (long)RPRResolution.PlanErrorReinstatement:
                                case (long)RPRResolution.ReinstatementDeathABTerm:
                                case (long)RPRResolution.ReinstatementOther:
                                case (long)RPRResolution.ReinstatementTRC14:
                                    ReInstmtSheet.LstRowData.Add(newRow);
                                    break;
                                case (long)RPRResolution.EffectiveDateChange:
                                case (long)RPRResolution.RetroEnrollment:
                                    RetEnrlSheet.LstRowData.Add(newRow);
                                    break;
                                case (long)RPRResolution.DisenrollmentDateChangeDODError:
                                case (long)RPRResolution.EnrollmentCancellation:
                                case (long)RPRResolution.RetroDisenrollment:
                                    RetDisenrlSheet.LstRowData.Add(newRow);
                                    break;
                                case (long)RPRResolution.PBPChange:
                                    PBPSheet.LstRowData.Add(newRow);
                                    break;
                            }

                            //add details to BGP deatils 
                            objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Success;
                            objDOCMN_BackgroundProcessDetails.GEN_QueueRef = newRow.GEN_QueueId;
                            //increase count of total records processed
                            lRecordsProcessed++;
                            CaseIds.Add(newRow.GEN_QueueId);
                        }
                        else
                        {
                            //if row creation failed update BGP deatils with fail result
                            objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                            objDOCMN_BackgroundProcessDetails.GEN_QueueRef = 0;
                            objDOCMN_BackgroundProcessDetails.FailureReason = errorMessage;
                        }
                        //insert into BGP details table
                        objBLCommon.InsertBackgroundProcessDetails(objDOCMN_BackgroundProcessDetails, out errorMessage);
                    }
                    //once finished processing and creating rows for all the pdf files
                    //add excel sheets to list Excel Sheets if there are rows in the Sheet data
                    if (RetDisenrlSheet.LstRowData.Count > 0)
                        lstExcelSheets.Add(RetDisenrlSheet);
                    if (PBPSheet.LstRowData.Count > 0)
                        lstExcelSheets.Add(PBPSheet);
                    if (RetEnrlSheet.LstRowData.Count > 0)
                        lstExcelSheets.Add(RetEnrlSheet);
                    if (ReInstmtSheet.LstRowData.Count > 0)
                        lstExcelSheets.Add(ReInstmtSheet);
                }
                //create excel workbook and save from list of excel sheets
                result = CreateExcel(lstExcelSheets, objExcelCreationConfig);
            }
            catch (Exception ex)
            {
                //log if any exception
                Console.WriteLine("Error : " + ex.Message);
                result = ExceptionTypes.Exception;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Starting Excel Creation", ex.StackTrace.ToString());
            }

            return result;
        }
        private ExceptionTypes CreateExcel(List<FDRSubmissionSheetData> lstExcelData, ExcelCreationConfig objExcelCreationConfig)
        {
            //check if there are any sheets in List of excel sheets to create excel
            if (lstExcelData.Count > 0)
            {
                try
                {
                    //read the template for creating new excel file
                    FileInfo reportTemplate = new FileInfo(objExcelCreationConfig.TemplateFilelocation + objExcelCreationConfig.TemplateFileName);
                    Console.WriteLine("Getting excel template : " + reportTemplate);
                    //parse template as excel workbook package for processing
                    ExcelPackage templatePackage = new ExcelPackage(reportTemplate);
                    //creating new file path at new location created at  begining of process
                    string strNewExcelFile = objExcelCreationConfig.NewFilesLocation + DateTime.UtcNow.ToString("yyyy.MM.dd ") + objExcelCreationConfig.NewExcelFileNamingConvention;
                    //create new excel file with new name
                    FileInfo newExcelFileInfo = new FileInfo(strNewExcelFile);
                    //if file with same name exists in location delete the file to replace with new file
                    if (newExcelFileInfo.Exists)
                        newExcelFileInfo.Delete();
                    //create new excel workbook package for new file
                    using (var package = new ExcelPackage(newExcelFileInfo))
                    {
                        Console.WriteLine("Creating RPC Submission Excel...");
                        //add 1st sheet (information sheet ) from template to new excel file
                        package.Workbook.Worksheets.Add(templatePackage.Workbook.Worksheets[1].Name, templatePackage.Workbook.Worksheets[1]);//adding information excel sheet
                        //Process each excel sheet in list of excel sheet to create excel sheet in workbook
                        foreach (FDRSubmissionSheetData item in lstExcelData)
                        {
                            //check if there are rows in excel sheet
                            if (item.LstRowData.Count > 0)
                            {
                                //get the location of the new excel sheet in workbook
                                int sheetNumber = (int)item.ISheetNumber;
                                //Get satrting row to add data in excel sheet skip two as Index start from 0 and 1 instruction excel sheet to skip
                                int iStartRow = item.IStartRow;
                                //adding sheet for each type of FDR submission action requested type
                                //add new sheet to new excel with name from template
                                ExcelWorksheet excelWorksheet = package.Workbook.Worksheets.Add(templatePackage.Workbook.Worksheets[sheetNumber].Name, templatePackage.Workbook.Worksheets[sheetNumber]);
                                //process each row to be added in excel sheet
                                foreach (FDRSubmissionRow rowData in item.LstRowData)
                                {
                                    //check for SCC Configuration
                                    if (objExcelCreationConfig.SubmissionCategory == FDRSubmissionCategory.SCC)
                                    {
                                        excelWorksheet.Cells[iStartRow, 1].LoadFromText(rowData.MemberContractID);
                                        excelWorksheet.Cells[iStartRow, 2].LoadFromText(rowData.MemberCurrentHICN);
                                        excelWorksheet.Cells[iStartRow, 3].LoadFromText(rowData.MemberLastName);
                                        excelWorksheet.Cells[iStartRow, 4].LoadFromText(rowData.MemberFirstName);
                                        excelWorksheet.Cells[iStartRow, 5].LoadFromText(rowData.EffectiveDate);
                                        excelWorksheet.Cells[iStartRow, 5].Style.Numberformat.Format = "MM/dd/yyyy";
                                        excelWorksheet.Cells[iStartRow, 6].LoadFromText(rowData.EndDate);
                                        excelWorksheet.Cells[iStartRow, 6].Style.Numberformat.Format = "MM/dd/yyyy";
                                        excelWorksheet.Cells[iStartRow, 7].LoadFromText(rowData.SCCRPRRequested);
                                        excelWorksheet.Cells[iStartRow, 7].Style.Numberformat.Format = "00000";
                                        excelWorksheet.Cells[iStartRow, 8].LoadFromText(rowData.SCCRPRRequestedZip);
                                        excelWorksheet.Cells[iStartRow, 8].Style.Numberformat.Format = "00000";
                                    }
                                    else
                                    {
                                        //based on sheet number ad row data with specific coloumn structure
                                        switch (sheetNumber)
                                        {
                                            case (int)FDRSubmissionExcelSheets.Ret_Disenrl:
                                                excelWorksheet.Cells[iStartRow, 1].LoadFromText(rowData.MemberContractID);
                                                excelWorksheet.Cells[iStartRow, 2].LoadFromText(rowData.MemberPBP);
                                                excelWorksheet.Cells[iStartRow, 2].Style.Numberformat.Format = "000";
                                                excelWorksheet.Cells[iStartRow, 3].LoadFromText(rowData.MemberCurrentHICN);
                                                excelWorksheet.Cells[iStartRow, 4].LoadFromText(rowData.MemberLastName);
                                                excelWorksheet.Cells[iStartRow, 5].LoadFromText(rowData.MemberFirstName);
                                                excelWorksheet.Cells[iStartRow, 6].LoadFromText(rowData.ElectionType);
                                                excelWorksheet.Cells[iStartRow, 7].LoadFromText(rowData.EffectiveDate);
                                                excelWorksheet.Cells[iStartRow, 7].Style.Numberformat.Format = "MM/dd/yyyy";
                                                break;
                                            case (int)FDRSubmissionExcelSheets.PBP:
                                            case (int)FDRSubmissionExcelSheets.Ret_Enrl:
                                                excelWorksheet.Cells[iStartRow, 1].LoadFromText(rowData.MemberContractID);
                                                excelWorksheet.Cells[iStartRow, 2].LoadFromText(rowData.MemberPBP);
                                                excelWorksheet.Cells[iStartRow, 2].Style.Numberformat.Format = "000";
                                                excelWorksheet.Cells[iStartRow, 3].LoadFromText("");
                                                excelWorksheet.Cells[iStartRow, 4].LoadFromText(rowData.MemberCurrentHICN);
                                                excelWorksheet.Cells[iStartRow, 5].LoadFromText(rowData.MemberLastName);
                                                excelWorksheet.Cells[iStartRow, 6].LoadFromText(rowData.MemberFirstName);
                                                excelWorksheet.Cells[iStartRow, 7].LoadFromText(rowData.ElectionType);
                                                excelWorksheet.Cells[iStartRow, 8].LoadFromText(rowData.EffectiveDate);
                                                excelWorksheet.Cells[iStartRow, 8].Style.Numberformat.Format = "MM/dd/yyyy";
                                                excelWorksheet.Cells[iStartRow, 9].LoadFromText(rowData.EndDate);
                                                excelWorksheet.Cells[iStartRow, 9].Style.Numberformat.Format = "MM/dd/yyyy";
                                                excelWorksheet.Cells[iStartRow, 10].LoadFromText(rowData.ApplicationDate);
                                                excelWorksheet.Cells[iStartRow, 10].Style.Numberformat.Format = "MM/dd/yyyy";
                                                break;
                                            case (int)FDRSubmissionExcelSheets.REINSTMT:
                                                excelWorksheet.Cells[iStartRow, 1].LoadFromText(rowData.MemberContractID);
                                                excelWorksheet.Cells[iStartRow, 2].LoadFromText(rowData.MemberPBP);
                                                excelWorksheet.Cells[iStartRow, 2].Style.Numberformat.Format = "000";
                                                excelWorksheet.Cells[iStartRow, 3].LoadFromText("");
                                                excelWorksheet.Cells[iStartRow, 4].LoadFromText(rowData.MemberCurrentHICN);
                                                excelWorksheet.Cells[iStartRow, 5].LoadFromText(rowData.MemberLastName);
                                                excelWorksheet.Cells[iStartRow, 6].LoadFromText(rowData.MemberFirstName);
                                                excelWorksheet.Cells[iStartRow, 7].LoadFromText(rowData.EffectiveDate);
                                                excelWorksheet.Cells[iStartRow, 7].Style.Numberformat.Format = "MM/dd/yyyy";
                                                excelWorksheet.Cells[iStartRow, 8].LoadFromText(rowData.EndDate);
                                                excelWorksheet.Cells[iStartRow, 8].Style.Numberformat.Format = "MM/dd/yyyy";
                                                break;
                                        }
                                    }
                                    //increment row number to set for next row in same excel sheet
                                    iStartRow++;
                                }
                            }
                        }
                        //save newly created excel workbook
                        package.Save();
                        Console.WriteLine("RPC Submission Excel Created :" + newExcelFileInfo.FullName);
                        return ExceptionTypes.Success;
                    }
                }
                catch (Exception ex)
                {
                    //log exception if any
                    Console.WriteLine("Error : " + ex.Message);
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Excel Creating", ex.StackTrace.ToString());
                    return ExceptionTypes.Exception;
                }
            }
            //Log error when there were no records in ERS for pdf files
            Console.WriteLine("No records for creating excel...");
            string error = "No excel file created since no records found for pdf name HICN and Contract in ERS";
            BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, error.ToString(), error.ToString());
            return ExceptionTypes.Success;
        }
        private ExceptionTypes CreateRowData(string strContractId, string strHICN, FDRSubmissionCategory SubmissionCategory,out FDRSubmissionRow objFDRSubmissionRow, out long lResolution, out string errorMessage)
        {
            objFDRSubmissionRow = new FDRSubmissionRow();
            lResolution = 0;
            try
            {
                BLFDR objBLFDR = new BLFDR();
                string[] arrHICN = strHICN.Trim().Split(' ');//split HICN to check if specific action requested suffix is present 
                long lTransactionTypeLkup = 0;
                ExceptionTypes result = ExceptionTypes.UnknownError;
                if (SubmissionCategory == FDRSubmissionCategory.SCC)
                {
                    //get ERS deatils for current HICN and Contract
                    result = objBLFDR.GetCaseDetails(strContractId, arrHICN[0], (long)DiscripancyType.SCCRPR, lTransactionTypeLkup, out List<FDRSubmissionRow> lstCases, out errorMessage);

                    //check for Multiple cases for same HICN and Contract but no Ttransaction type suffix to FIle name
                    if (lstCases.Count > 0)
                    {
                        //check if any case found
                        if (lstCases.Count == 1)
                        {
                            objFDRSubmissionRow = lstCases.FirstOrDefault();
                            objFDRSubmissionRow.Errormessage = "Case Id for : HICN-" + arrHICN[0] + " , Contract-" + strContractId + " , Discripancy Type-" + DiscripancyType.SCCRPR.ToString() + " is " + objFDRSubmissionRow.GEN_QueueId;
                            objFDRSubmissionRow.IsFDRSubmissionCompleted = true;                           
                            objBLFDR.InsertFDRSubmissionLog(lstCases.ToList(), _lCurrentMasterUserId, objFDRSubmissionRow.IsFDRSubmissionCompleted, objFDRSubmissionRow.Errormessage, out errorMessage);
                        }
                        else
                        {
                            objFDRSubmissionRow.Errormessage = "Multiple cases present for  HICN-" + arrHICN[0] + " , Contract-" + strContractId + " in Discrepancy Type SCC RPR";
                            objFDRSubmissionRow.IsFDRSubmissionCompleted = false;                          
                            objBLFDR.InsertFDRSubmissionLog(lstCases.ToList(), _lCurrentMasterUserId, objFDRSubmissionRow.IsFDRSubmissionCompleted, objFDRSubmissionRow.Errormessage,out errorMessage);
                            result = ExceptionTypes.UnknownError;
                        }
                    }
                    else
                    {
                        objFDRSubmissionRow.MemberContractID = strContractId;
                        objFDRSubmissionRow.MemberCurrentHICN = arrHICN[0];
                        objFDRSubmissionRow.IsFDRSubmissionCompleted = false;
                        lstCases.Add(objFDRSubmissionRow);
                        //set error msg as no case for HICN and contract
                        objFDRSubmissionRow.Errormessage = "No case found for : HICN-" + arrHICN[0] + " , Contract-" + strContractId + " , Discripancy Type-" + DiscripancyType.SCCRPR.ToString();
                        objBLFDR.InsertFDRSubmissionLog(lstCases.ToList(), _lCurrentMasterUserId, objFDRSubmissionRow.IsFDRSubmissionCompleted, objFDRSubmissionRow.Errormessage, out errorMessage);
                    }

                }
                else
                {
                    if (arrHICN.Length > 1)
                        lTransactionTypeLkup = GetTransactionTypeFromHICNSuffix(arrHICN[1]);
                    //check weather transaction type is is valid and is present in RPRActionRequested ENUM
                    if (arrHICN.Length == 1 || lTransactionTypeLkup != 0)
                    {
                        //get ERS deatils for current HICN and Contract
                        result = objBLFDR.GetCaseDetails(strContractId, arrHICN[0],(long)DiscripancyType.RPR ,lTransactionTypeLkup, out List<FDRSubmissionRow> lstCases, out errorMessage);
                        //set error msg as no case for HICN and contract
                        errorMessage = "No case found for : HICN-" + arrHICN[0] + " , Contract-" + strContractId + " , Discripancy Type-" + DiscripancyType.RPR.ToString() + " , Transaction Type-" + ((FDRTransactionType)lTransactionTypeLkup).ToString();

                        //check for Multiple cases for same HICN and Contract but no Ttransaction type suffix to FIle name
                        if (lstCases.Count > 0)
                        {
                            //check if any case found
                            if (lstCases.Count == 1)
                            {
                                objFDRSubmissionRow = lstCases.FirstOrDefault();
                                objFDRSubmissionRow.Errormessage = "Case Id for : HICN-" + arrHICN[0] + " , Contract-" + strContractId + " , Discripancy Type-" + DiscripancyType.RPR.ToString() + " , Transaction Type-" + ((FDRTransactionType)lTransactionTypeLkup).ToString() + " is " + objFDRSubmissionRow.GEN_QueueId;
                                lResolution = objFDRSubmissionRow.ResolutionLkup;
                                objFDRSubmissionRow.IsFDRSubmissionCompleted = true;                               
                                objBLFDR.InsertFDRSubmissionLog(lstCases.ToList(), _lCurrentMasterUserId, objFDRSubmissionRow.IsFDRSubmissionCompleted, objFDRSubmissionRow.Errormessage, out errorMessage);
                            }
                            else
                            {
                                objFDRSubmissionRow.Errormessage = "Multiple cases present for  HICN-" + arrHICN[0] + " , Contract-" + strContractId + ".Transaction Type Suffix is Required in file Name.";
                                objFDRSubmissionRow.IsFDRSubmissionCompleted = false;                              
                                objBLFDR.InsertFDRSubmissionLog(lstCases.ToList(), _lCurrentMasterUserId, objFDRSubmissionRow.IsFDRSubmissionCompleted, objFDRSubmissionRow.Errormessage, out errorMessage);
                                result = ExceptionTypes.UnknownError;
                            }
                        }
                        else
                        {
                            objFDRSubmissionRow.MemberContractID = strContractId;
                            objFDRSubmissionRow.MemberCurrentHICN = arrHICN[0];
                            objFDRSubmissionRow.IsFDRSubmissionCompleted = false;
                            lstCases.Add(objFDRSubmissionRow);
                            //set error msg as no case for HICN and contract
                            objFDRSubmissionRow.Errormessage = "No case found for : HICN-" + arrHICN[0] + " , Contract-" + strContractId + " , Discripancy Type-" + DiscripancyType.RPR.ToString();
                            objBLFDR.InsertFDRSubmissionLog(lstCases.ToList(), _lCurrentMasterUserId, objFDRSubmissionRow.IsFDRSubmissionCompleted, objFDRSubmissionRow.Errormessage, out errorMessage);
                        }
                    }
                    else
                    {
                        errorMessage = "Invalid trasaction type requested in file name : " + strContractId + '-' + strHICN + ".pdf";
                    }
                }
                Console.WriteLine(errorMessage);
                return result;
            }
            catch (Exception ex)
            {
                //log excepiton if any
                Console.WriteLine("Error : " + ex.Message);
                errorMessage = ex.Message;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Creating Excel Row For " + strContractId + " " + strHICN , ex.StackTrace.ToString());
                return ExceptionTypes.Exception;
            }
        }
        private long GetTransactionTypeFromHICNSuffix(string strHICNSuffix)
        {
            //return action requested based on HICN suffix in pdf file name
            switch (strHICNSuffix)
            {
                case "PB":
                    return (long)FDRTransactionType.PBP;
                case "RE":
                    return (long)FDRTransactionType.RetEnrl;
                case "RI":
                    return (long)FDRTransactionType.Reinstmt;
                case "RD":
                    return (long)FDRTransactionType.RetDis;
                default:
                    return 0;
            }
        }
    }
}
