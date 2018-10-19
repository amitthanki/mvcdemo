using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ERSBackgroundProcess
{
    public static class CacheUtility
    {

        /// <summary>
        /// Fetch all Configurations If no Cache is available
        /// </summary>
        private static void GetAllConfigurationIfNoCache()
        {
            long TimeZone = (long)DefaultTimeZone.CentralStandardTime;
            if (!MemoryCache.Default.Contains(ConstantTexts.ConfigurationsCacheKey))
            {
                BLConfigurations objBLConfigurations = new BLConfigurations();
                DOMGR_ConfigMaster objDOMGR_ConfigMaster = new DOMGR_ConfigMaster()
                {
                    IsActive = true
                };
                ExceptionTypes exResult = objBLConfigurations.SearchConfiguration(TimeZone, objDOMGR_ConfigMaster, out List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster, out string errorMessage);
                // Store data in the cache  
                AddToCache(ConstantTexts.ConfigurationsCacheKey, lstDOMGR_ConfigMaster, DateTime.Now.AddHours(1));
            }
        }

        /// <summary>
        /// Add to cache if not present in cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void AddToCache(string key, object value, DateTimeOffset Expiration)
        {
            CacheItemPolicy policy = new CacheItemPolicy()
            {
                AbsoluteExpiration = Expiration
            };
            MemoryCache.Default.Add(key, value, new CacheItemPolicy());
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveFromCache(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        /// <summary>
        /// Get from cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static object GetFromCache(string key)
        {
            return MemoryCache.Default.Get(key);
        }

        /// <summary>
        /// Get lookup value by id
        /// </summary>
        /// <param name="strConfigName"></param>
        /// <returns></returns>
        public static string GetConfigrationValueByConfigName(string strConfigName)
        {
            try
            {
                if (strConfigName.IsNullOrEmpty())
                    return string.Empty;
                //fetch all Configurations if not present cache
                GetAllConfigurationIfNoCache();
                List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster = GetFromCache(ConstantTexts.ConfigurationsCacheKey) as List<DOMGR_ConfigMaster>;
                return lstDOMGR_ConfigMaster.Where(x => x.ConfigName == strConfigName).FirstOrDefault().ConfigValue;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(StartBackgroundProcess.CurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BackgroundProcess, (long)ExceptionTypes.Uncategorized, "Exception Getting config value from cache", ex.StackTrace.ToString());
            }
            return string.Empty;
        }

        /// <summary>
        /// Fetch all lookupsDataSet of particular type in which id belongs
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<DOCMN_LookupMaster> GetAllLookupsFromCache(long? id)
        {
            try
            {
                GetAllLookupsIfNoCache();
                List<DOCMN_LookupMaster> lstDOCMN_LookupMaster = GetFromCache(ConstantTexts.LookupMasterCacheKey) as List<DOCMN_LookupMaster>;
                if (id != null)
                {
                    return lstDOCMN_LookupMaster = lstDOCMN_LookupMaster.Where(x => x.CMN_LookupTypeRef.Equals(id)).OrderBy(x => x.LookupValue).ToList();
                }
                else
                    return lstDOCMN_LookupMaster;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Fetch all lookup master and types if not present in cache
        /// </summary>
        private static void GetAllLookupsIfNoCache()
        {
            if (!(MemoryCache.Default.Contains(ConstantTexts.LookupTypeCacheKey) || MemoryCache.Default.Contains(ConstantTexts.LookupMasterCacheKey)))
            {
                List<DOCMN_LookupMaster> lstDOCMN_LookupMaster;
                List<DOCMN_LookupType> lstDOCMN_LookupType;
                BLLookup objBLLookup = new BLLookup();
                ExceptionTypes exResult = objBLLookup.GetAllLookups(null, out lstDOCMN_LookupType, out lstDOCMN_LookupMaster);

                lstDOCMN_LookupType = lstDOCMN_LookupType.Where(x => x.IsActive == true).ToList();
                AddToCache(ConstantTexts.LookupTypeCacheKey, lstDOCMN_LookupType, DateTime.Now.AddHours(1));
                lstDOCMN_LookupMaster = lstDOCMN_LookupMaster.Where(x => x.IsActive == true).ToList();
                AddToCache(ConstantTexts.LookupMasterCacheKey, lstDOCMN_LookupMaster, DateTime.Now.AddHours(1));
            }            
        }
    }
}
