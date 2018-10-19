using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.Utility
{
    public class AppConfigData
    {
        public static long BackGroundProcessType
        {
            get
            {
                if (ConfigurationManager.AppSettings["BackgroundProcessType"] != null)
                {
                    return Convert.ToInt64(ConfigurationManager.AppSettings["BackgroundProcessType"]);
                }
                else
                    return 0;
            }
        }

        public static Boolean UseJavaMQClient
        {
            get
            {
                if (ConfigurationManager.AppSettings["UseJavaMQClient"] != null)
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["UseJavaMQClient"]);
                }
                else
                    return false;
            }
        }

        //public static string  MQNonSecureChannel
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["MQNonSecureChannel"] != "")
        //        {
        //            return ConfigurationManager.AppSettings["MQNonSecureChannel"].ToString();
        //        }

        //        else return string.Empty;
        //    }
        //}
        //public static string MQSecureChannel
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["MQSecureChannel"] != "")
        //        {
        //            return ConfigurationManager.AppSettings["MQSecureChannel"].ToString();
        //        }

        //        else return string.Empty;
        //    }
        //}
        //public static string MQHostName
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["MQHost"] != "")
        //        {
        //            return ConfigurationManager.AppSettings["MQHost"].ToString();
        //        }

        //        else return string.Empty;
        //    }
        //}
        //public static string MQPortNumber
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["MQPort"] != "")
        //        {
        //            return ConfigurationManager.AppSettings["MQPort"].ToString();
        //        }

        //        else return string.Empty;
        //    }
        //}
        //public static string MQQueueManager
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["QueueManager"] != "")
        //        {
        //            return ConfigurationManager.AppSettings["QueueManager"].ToString();
        //        }

        //        else return string.Empty;
        //    }
        //}

        //public static string MQTopic
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["QueueManager"] != "")
        //        {
        //            return ConfigurationManager.AppSettings["QueueManager"].ToString();
        //        }

        //        else return string.Empty;
        //    }
        //}

        //public static bool IsMQSecureConnection
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["IsMQSecureConnection"] != "")
        //        {
        //            return ExtensionMethods.ToBoolean(ConfigurationManager.AppSettings["IsMQSecureConnection"]);
        //        }
        //        else return false;
        //    }
        //}

        //public static string GetQueueName
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["GetQueueName"] != "")
        //        {
        //            return (ConfigurationManager.AppSettings["GetQueueName"]);
        //        }
        //        else return "";
        //    }
        //}

        //public static string GetTopicName
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["GetTopicName"] != "")
        //        {
        //            return (ConfigurationManager.AppSettings["GetTopicName"]);
        //        }
        //        else return "";
        //    }
        //}

        //public static string Category
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["Category2"] != null)
        //        {
        //            return Convert.ToString(ConfigurationManager.AppSettings["Category2"]);
        //        }
        //        else
        //            return string.Empty;
        //    }
        //}

        //public static string Category2CTM
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["Category2CTM"] != null)
        //        {
        //            return Convert.ToString(ConfigurationManager.AppSettings["Category2CTM"]);
        //        }
        //        else
        //            return string.Empty;
        //    }
        //}

        //public static string Category3
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["Category3"] != null)
        //        {
        //            return Convert.ToString(ConfigurationManager.AppSettings["Category3"]);
        //        }
        //        else
        //            return string.Empty;
        //    }
        //}
        //public static string ReSubmission
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["ReSubmission"] != null)
        //        {
        //            return Convert.ToString(ConfigurationManager.AppSettings["ReSubmission"]);
        //        }
        //        else
        //            return string.Empty;
        //    }
        //}

        //public static string StartRowsForSheet
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["StartRowsForSheet"] != null)
        //        {
        //            return Convert.ToString(ConfigurationManager.AppSettings["StartRowsForSheet"]);
        //        }
        //        else
        //            return string.Empty;
        //    }
        //}
    }
}
