using ENRLReconSystem.DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.Utility;
using ENRLReconSystem.BL;
using System.Reflection;

namespace ERSBackgroundProcess
{
    public class FilesCopy
    {
        public ExceptionTypes StartFilesCopy(ExcelCreationConfig objExcelCreationConfig)
        {
            Console.WriteLine("Copying Files Started...");
            ExceptionTypes result = ExceptionTypes.Success; 
            try
            {
                //copy filtered pdf files in objExcelCreationConfig.LstPdffiles to destination location
                foreach (var file in objExcelCreationConfig.LstPdffiles)
                {
                    try
                    {
                        if (File.Exists(objExcelCreationConfig.NewFilesLocation + file.Name))//If same file already exists then delete to replace
                            File.Delete(objExcelCreationConfig.NewFilesLocation + file.Name);

                        //move file
                        File.Move(file.FullName, objExcelCreationConfig.NewFilesLocation + file.Name);
                        Console.WriteLine("Copied File : "+ file.Name);
                    }
                    catch (IOException ex)
                    {
                        //catch IO exception to check for duplicate file
                        //check if file exists
                        bool exists = File.Exists(objExcelCreationConfig.NewFilesLocation + file.Name);
                        if (!exists)//if file does not exist throw exception
                            throw ex;
                        Console.WriteLine("Error : " + ex.Message);
                        //if file exists log and continue copying other files
                        BLCommon.LogError(StartBackgroundProcess.CurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception PDF file"+ file.Name +" already exists in location", ex.StackTrace.ToString());
                    }
                }
                Console.WriteLine("Copying Files Completed.");
            }
            catch (Exception ex)
            {
                //log any other exception other than duplicate file
                Console.WriteLine("Error : " + ex.Message);
                result = ExceptionTypes.Exception;
                BLCommon.LogError(StartBackgroundProcess.CurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Copying PDF File", ex.StackTrace.ToString());
            }
            //return result of files copy
            return result;
        }
    }
}
