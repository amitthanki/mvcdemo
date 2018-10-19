using ENRLReconSystem.DO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.Utility;
using ENRLReconSystem.BL;
using System.Reflection;
using System.Globalization;

namespace ERSBackgroundProcess
{
    public class FDRSubmission
    {
        long _lSubmissionCategory = 0;//Submission Category field
        string _strSubmissionCategory = string.Empty;//Submission Category Name field
        long _lCurrentMasterUserId = StartBackgroundProcess.CurrentMasterUserId;//Current system Id from ADM_User master -- static -- set while starting process
        BLFDR objBLFDR = new BLFDR();

        // Author: Pradeep K Patil
        // Create date: 08/17/2017
        // Method Description: Create Category 2 Submission
        // Method Name: CreateCategory2Submission
        /// <summary>
        /// Create Category 2 Submission
        /// </summary>
        public void CreateCategory2Submission()
        {
            //set properties values for Category 2 submission 
            _strSubmissionCategory = "FDR Submission Category 2";
            _lSubmissionCategory = (long)FDRSubmissionCategory.Category2;
            //Getting details from app.config
            //string[] arrConfig = AppConfigData.Category2.Split('|');
            string[] arrConfig = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.Category2).Split('|');
            StartFDRSubmission(arrConfig,ConstantTexts.FDRStartRowsForSheet, (long)BackgroundProcessType.FDRSubmissionCat2);
        }

        public void CreateCategory2CTMSubmission()
        {
            //set properties values for Category 2 CTM submission 
            _strSubmissionCategory = "FDR Submission Category 2 CTM";
            _lSubmissionCategory = (long)FDRSubmissionCategory.Category2CTM;
            //Getting details from app.config
            //string[] arrConfig = AppConfigData.Category2CTM.Split('|');
            string[] arrConfig = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.Category2CTM).Split('|');
            StartFDRSubmission(arrConfig, ConstantTexts.FDRStartRowsForSheet, (long)BackgroundProcessType.FDRSubmissionCat2CTM);
        }

        public void CreateCategory3Submission()
        {
            //set properties values for Category 3 submission 
            _strSubmissionCategory = "FDR Submission Category 3";
            _lSubmissionCategory = (long)FDRSubmissionCategory.Category3;
            //Getting details from app.config
            //string[] arrConfig = AppConfigData.Category3.Split('|');
            string[] arrConfig = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.Category3).Split('|');
            StartFDRSubmission(arrConfig, ConstantTexts.FDRStartRowsForSheet, (long)BackgroundProcessType.FDRSubmissionCat3);
        }

        public void CreateReSubmission()
        {
            //set properties values for Resubmission 
            _strSubmissionCategory = "FDR Resubmission";
            _lSubmissionCategory = (long)FDRSubmissionCategory.ReSubmission;
            //Getting details from app.config
            //string[] arrConfig = AppConfigData.ReSubmission.Split('|');
            string[] arrConfig = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.ReSubmission).Split('|');
            StartFDRSubmission(arrConfig, ConstantTexts.FDRStartRowsForSheet, (long)BackgroundProcessType.FDRResubmission);
        }

        internal void CreateSCCFDRSubmission()
        {
            //set properties values for FDR Submission SCC
            _strSubmissionCategory = "FDR Submission SCC";
            _lSubmissionCategory = (long)FDRSubmissionCategory.SCC;
            //Getting details from app.config
            //string[] arrConfig = AppConfigData.ReSubmission.Split('|');
            string[] arrConfig = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.SCC).Split('|');
            StartFDRSubmission(arrConfig,ConstantTexts.SCCFDRStartRowsForSheet,(long)BackgroundProcessType.FDRSubmissionSCC);
        }

        private void StartFDRSubmission(string[] arrConfig, string startRowsConfigName, long lBackgroundProcessType)
        {
            string dayOfTheWeek = string.Empty;
            string fdrSubmissionPath = string.Empty;
            DateTime startDate;
            ExceptionTypes objExceptionTypesResult;
            List<IncludeInTodaysSubmission> lstIncludeInTodaysSubmission = new List<IncludeInTodaysSubmission>();
            List<FileInfo> LstPdffiles = new List<FileInfo>();
            try
            {
                if (arrConfig.Length == 7)
                {
                    //Insert BGP Master Row
                    BLCommon objCommon = new BLCommon();
                    objCommon.InsertBackgroundProcessMaster(lBackgroundProcessType, _lCurrentMasterUserId, out long bgpMasterId, out string errorMessage);


                    objExceptionTypesResult = objBLFDR.GetIncludeInTodaysSubmission(out lstIncludeInTodaysSubmission, out string errorMsg);
                    if (objExceptionTypesResult != ExceptionTypes.Success)
                    {
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, errorMsg, errorMsg);
                    }

                    long lRecordsProcessed = 0;//default value for records processed
                    long lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Success;//default status for process
                    startDate = DateTime.UtcNow.AddDays(-Convert.ToInt32(arrConfig[5]));
                    dayOfTheWeek = startDate.DayOfWeek.NullToString();
                    switch (_lSubmissionCategory)
                    {
                        case (long)FDRSubmissionCategory.Category2:                        
                        case (long)FDRSubmissionCategory.SCC:
                            if (dayOfTheWeek == "Sunday" || dayOfTheWeek == "Saturday")//sunday||Satday
                            {
                                startDate = DateTime.UtcNow.AddDays(-(Convert.ToInt32(arrConfig[5]) + Convert.ToInt32(arrConfig[5])));
                                dayOfTheWeek = startDate.DayOfWeek.NullToString();
                            }
                            fdrSubmissionPath = arrConfig[0] + dayOfTheWeek;
                            break;
                        case (long)FDRSubmissionCategory.Category2CTM:
                            if (dayOfTheWeek == "Sunday")//sunday
                            {
                                startDate = DateTime.UtcNow.AddDays(-(Convert.ToInt32(arrConfig[5]) + Convert.ToInt32(arrConfig[5])));
                                dayOfTheWeek = startDate.DayOfWeek.NullToString();
                            }
                            fdrSubmissionPath = arrConfig[0] + dayOfTheWeek;
                            break;
                        case (long)FDRSubmissionCategory.ReSubmission:
                            if (dayOfTheWeek == "Sunday")//sunday
                            {
                                startDate = DateTime.UtcNow.AddDays(-3);//Friday
                                dayOfTheWeek = startDate.DayOfWeek.NullToString();
                            }
                            fdrSubmissionPath = arrConfig[0] + dayOfTheWeek;
                            break;
                        case (long)FDRSubmissionCategory.Category3:
                            fdrSubmissionPath = arrConfig[0] + "Submission " + GetSubmissionNumberForCat3();
                            break;
                    }

                    //create Process configuration deatils object from config data
                    ExcelCreationConfig objExcelCreationConfig = new ExcelCreationConfig()
                    {
                        PdfFileslocation = fdrSubmissionPath,
                        TemplateFilelocation = arrConfig[1],
                        TemplateFileName = arrConfig[2],
                        NewFilesLocation = arrConfig[3],
                        NewExcelFileNamingConvention = arrConfig[4],
                        FilterStartDate = DateTime.UtcNow.AddDays(-Convert.ToInt32(arrConfig[5])),
                        FilterEndDate = DateTime.UtcNow.AddDays(-Convert.ToInt32(arrConfig[6])),
                        LstPdffiles = new List<FileInfo>(),
                        LbgpMasterId = bgpMasterId,
                        SubmissionCategory = (FDRSubmissionCategory)_lSubmissionCategory,
                        //StartingRows = AppConfigData.StartRowsForSheet.Split('|').Select(x => x.ToInt32()).ToList()
                        StartingRows = CacheUtility.GetConfigrationValueByConfigName(startRowsConfigName).Split('|').Select(x => x.ToInt32()).ToList()
                    };
                    DirectoryInfo info = new DirectoryInfo(objExcelCreationConfig.PdfFileslocation);

                    //filter files to be accessed based on number of PdfsRetrivalStartDay and PdfsRetrivalEndDay from config 
                    objExcelCreationConfig.LstPdffiles = info.GetFiles("*-*.pdf").ToList();
                    //objExcelCreationConfig.LstPdffiles = info.GetFiles("*-*.pdf").ToList();

                    //Logic for include in todays submission
                    if (lstIncludeInTodaysSubmission.Count > 0)
                    {
                        info = new DirectoryInfo(arrConfig[0].NullToString());
                        foreach (var item in lstIncludeInTodaysSubmission)
                        {
                            if (info.GetFiles(item.SubmissionFileName, SearchOption.AllDirectories).Count() > 0)
                            {
                                objExcelCreationConfig.LstPdffiles.Add(info.GetFiles(item.SubmissionFileName, SearchOption.AllDirectories).FirstOrDefault());
                            }
                        }
                    }
                    //create new folder location
                    objExcelCreationConfig.NewFilesLocation = objExcelCreationConfig.NewFilesLocation + DateTime.UtcNow.ToString("yyyy/MM/dd") + "\\";

                    if (objExcelCreationConfig.LstPdffiles.Count > 0)
                    {
                        ExceptionTypes result;
                        Console.WriteLine("Creating directory..");
                        //create new folder for copying files and creating excels
                        Directory.CreateDirectory(objExcelCreationConfig.NewFilesLocation);
                        Console.WriteLine("New directory created : " + objExcelCreationConfig.NewFilesLocation);

                        FilesCopy objFilesCopy = new FilesCopy();
                        //copy files to new location
                        result = objFilesCopy.StartFilesCopy(objExcelCreationConfig);
                        if (result != ExceptionTypes.Success)
                        {
                            string ex = "Copying PDF Files Failed " + _strSubmissionCategory;
                            BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                        }

                        ExcelCreation objExcelCreation = new ExcelCreation();
                        //create FDR submission excel
                        result = objExcelCreation.StartExcelCreation(objExcelCreationConfig, out lRecordsProcessed,out List<long> CaseIds);

                        //if excel creation fails log error
                        if (result != ExceptionTypes.Success)
                        {
                            string ex = "Excel Creation Failed for " + _strSubmissionCategory;
                            BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                            lRecordsProcessed = 0;
                            lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                        }
                        else if (lRecordsProcessed > 0 && CaseIds.Count > 0)
                        {
                            //update the FDRPackagedate and FDRSUbmissioncategorylkup fields for all ers case Ids.
                 
                            result = objBLFDR.UpdateFDRPackageDate(CaseIds, objExcelCreationConfig.SubmissionCategory,_lCurrentMasterUserId ,out errorMessage);
                            if (result != ExceptionTypes.Success)
                            {
                                string ex = "Updating FDR Package date failed for " + _strSubmissionCategory;
                                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, errorMessage, ex.ToString());
                            }

                        }
                    }
                    else
                    {
                        //if no pdf files in location to create FDR submission log error
                        lRecordsProcessed = 0;
                        lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                        string ex = "No pdfs for "+ _strSubmissionCategory +" at : " + objExcelCreationConfig.PdfFileslocation + " created between " + objExcelCreationConfig.FilterStartDate.ToShortDateString() + " and " + objExcelCreationConfig.FilterEndDate.ToShortDateString();
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                        Console.WriteLine(ex);
                    }
                    //log BGP run status
                    DOCMN_BackgroundProcessMaster objDOCMN_BackgroundProcessMaster = new DOCMN_BackgroundProcessMaster();
                    objDOCMN_BackgroundProcessMaster.CMN_BackgroundProcessMasterId = bgpMasterId;
                    objDOCMN_BackgroundProcessMaster.TotalRecordProcessed = lRecordsProcessed;
                    objDOCMN_BackgroundProcessMaster.BGPStatusLkup = lBGPStatusLkup;
                    objDOCMN_BackgroundProcessMaster.LastUpdatedByRef = _lCurrentMasterUserId;
                    objCommon.UpdateBackgroundProcessMaster(objDOCMN_BackgroundProcessMaster, out errorMessage);
                }
                else
                {
                    //if app.config configuration for FDR submission is wrong
                    throw new InvalidDataException("Invalid App Configuration data for " + _strSubmissionCategory + " in app.config");
                }
            }
            catch (Exception ex)
            {
                //log exception if any
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Starting FDR Submission " + _strSubmissionCategory, ex.StackTrace.ToString());
            }
        }

        //private int GetWeekOfMonth(DateTime date)
        //{
        //    DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

        //    while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
        //        date = date.AddDays(1);

        //    return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        //}

        int GetSubmissionNumberForCat3()
        {
            DateTime startDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime currentFriday = startDate;

            int fridayWeekCount = 0;
            int submissionNum = 0;

            //Find the nearest Friday forward of the start date
            while (currentFriday.DayOfWeek != DayOfWeek.Friday)
            {
                currentFriday = currentFriday.AddDays(1);
            }

            //FIND ALL THE FRIDAYS!
            int currentYear = startDate.Year;
            while (currentFriday.Year == currentYear && currentFriday < DateTime.Today)
            {
                fridayWeekCount++;
                currentFriday = currentFriday.AddDays(7);
            }
            fridayWeekCount++;
            if (fridayWeekCount > 0)
            {
                submissionNum = (fridayWeekCount % 3) != 0 ? (fridayWeekCount % 3) : 3;
            }
            else
            {

            }

            return submissionNum;
        }

        #region Info

        //<!--FDR Submission Configs-->
        //<!--<add key = "Submission Type" value="Location to Pick PDF Files|Location to Pick template Excel File(xlsx)|Template File Name(template.xlsx)|Location To save New File|New File name convention string|strating row to insert data from in sheet|Files filter start day before today(0 for today)|Files filter End day before today(0 for today)"/>-->

        #endregion
    }
}
