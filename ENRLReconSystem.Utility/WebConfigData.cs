using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.Utility
{
    public class WebConfigData
    {


        public static bool DebugMode
        {
            get
            {
                if (ConfigurationManager.AppSettings["DebugMode"] == "1")
                {
                    return true;
                }
                return false;
            }
        }
        public static string MSDomain
        {
            get
            {
                if (ConfigurationManager.AppSettings["MSDomain"] == null)
                {
                    return "";
                }
                return ConfigurationManager.AppSettings["MSDomain"].ToString();
            }
        }

        public static string AdminSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["AdminSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["AdminSID"].ToString();
            }
        }
        public static string AdminOSTSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["AdmOSTSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["AdmOSTSID"].ToString();
            }
        }
        public static string AdminEligSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["AdmEligSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["AdmEligSID"].ToString();
            }
        }
        public static string AdminRPRSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["AdmRPRSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["AdmRPRSID"].ToString();
            }
        }
        public static string ManagerOSTSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["MgrOSTSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["MgrOSTSID"].ToString();
            }
        }
        public static string ManagerEligSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["MgrEligSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["MgrEligSID"].ToString();
            }
        }
        public static string ManagerRPRSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["MgrRPRSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["MgrRPRSID"].ToString();
            }
        }
        public static string ProcessorOSTSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["PrcrOSTSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["PrcrOSTSID"].ToString();
            }
        }
        public static string ProcessorEligSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["PrcrEligSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["PrcrEligSID"].ToString();
            }
        }
        public static string ProcessorRPRSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["PrcrRPRSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["PrcrRPRSID"].ToString();
            }
        }
        public static string ViewerOSTSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["VwrOSTSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["VwrOSTSID"].ToString();
            }
        }
        public static string ViewerEligSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["VwrEligSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["VwrEligSID"].ToString();
            }
        }
        public static string ViewerRPRSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["VwrRPRSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["VwrRPRSID"].ToString();
            }
        }
        public static string WebServiceSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["WebServiceSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["WebServiceSID"].ToString();
            }
        }
        public static string MacroServiceSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["MacroServiceSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["MacroServiceSID"].ToString();
            }
        }
        public static string MIIMSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["MIIMSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["MIIMSID"].ToString();
            }
        }
        public static string RestrictedSID
        {
            get
            {
                if (ConfigurationManager.AppSettings["RestrictedSID"] == null)
                    return "";
                return ConfigurationManager.AppSettings["RestrictedSID"].ToString();
            }
        }
        public static string BulkUploadFilePath
        {
            get
            {
                if (ConfigurationManager.AppSettings["BulkUploadFilePath"] == null)
                    return "";
                return ConfigurationManager.AppSettings["BulkUploadFilePath"].ToString();
            }
        }
        //public static string BulkUploadFileFormats
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["BulkUploadFileFormats"] == null)
        //            return "";
        //        return ConfigurationManager.AppSettings["BulkUploadFileFormats"].ToString();
        //    }
        //}

        //public static string webServerTempPath
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["webServerTempPath"] == null)
        //            return "";
        //        return ConfigurationManager.AppSettings["webServerTempPath"].ToString();
        //    }
        //}

        //public static string AttchmentFileFormat
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["AttchmentFileFormat"] == null)
        //            return "";
        //        return ConfigurationManager.AppSettings["AttchmentFileFormat"].ToString();
        //    }
        //}
        //public static string AttchmentFilePath
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["AttchmentFilePath"] == null)
        //            return "";
        //        return ConfigurationManager.AppSettings["AttchmentFilePath"].ToString();
        //    }
        //}

        public static string OneStopShopReportURL
        {
            get
            {
                if (ConfigurationManager.AppSettings["OneStopShopReportURL"] == null)
                    return "";
                return ConfigurationManager.AppSettings["OneStopShopReportURL"].ToString();
            }
        }

        public static string ServerType
        {
            get
            {
                if (ConfigurationManager.AppSettings["ServerType"] == null)
                    return string.Empty;
                return ConfigurationManager.AppSettings["ServerType"].ToString();
            }
        }

        public static string DmLogin
        {
            get
            {
                if (ConfigurationManager.AppSettings["DmLogin"] == null)
                    return string.Empty;
                return ConfigurationManager.AppSettings["DmLogin"].ToString();
            }
        }
        public static string DmPwd
        {
            get
            {
                if (ConfigurationManager.AppSettings["DmPassword"] == null)
                    return string.Empty;
                return ConfigurationManager.AppSettings["DmPassword"].ToString();
            }
        }
    }
}
