using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using ENRLReconSystem.DAL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;

namespace ERSBackgroundProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            StartBackgroundProcess process = null;

            try
            {
                long processType = 0;
                if (args != null && args.Length > 0)
                    processType = (args[0] != null && args[0].ToString().Length > 0) ? Convert.ToInt64(args[0]) : 0;

                if (processType == 0)
                    processType = AppConfigData.BackGroundProcessType;

                process = new StartBackgroundProcess();
                Console.WriteLine("Will Start Background Process : " + processType);
                process.StartProcess(processType, string.Empty);

            }
            catch (Exception ex)
            {
                process = new StartBackgroundProcess();
                process.StartProcess(0, ex.ToString() + ex.StackTrace);
            }
        }
    }
}
