using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ENRLReconSystem
{
   
    public static class CacheUtility
    {
        /// <summary>
        /// Add to cache if not present in cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void AddToCache(string key, object value)
        {
            if (System.Web.HttpContext.Current.Cache[key] == null)
            {
                System.Web.HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);

            }
        }

        /// <summary>
        /// Add to cache, overwrite if already present
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void AddToCacheReload(string key, object value)
        {
            System.Web.HttpContext.Current.Cache.Insert(key, value, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }

        /// <summary>
        /// Get from cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static object GetCache(string key)
        {
            return System.Web.HttpContext.Current.Cache[key];
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        /// <param name="key"></param>
        public static void ClearCache(string key)
        {
            System.Web.HttpContext.Current.Cache.Remove(key);
        }

        public static List<DOCMN_LookupType> GetAllLookuptypeOrById(long? lookupTypeId = 0)
        {
            List<DOCMN_LookupType> lstDOCMN_LookupType = null;

            try
            {
                //fetch all lookupsDataSet if not present cache
                GetAllLookupsIfNoCache();
                lstDOCMN_LookupType = GetCache(ConstantTexts.LookupTypeCacheKey) as List<DOCMN_LookupType>;
                if (lookupTypeId > 0)
                    return lstDOCMN_LookupType.Where(x => x.CMN_LookupTypeId.Equals(lookupTypeId)).ToList();
                else
                    return lstDOCMN_LookupType.ToList();
            }
            catch (Exception ex)
            {
                return lstDOCMN_LookupType;
            }
        }
        /// <summary>
        /// Get All Master Configuration
        /// </summary>
        /// <returns></returns>
        public static List<DOMGR_ConfigMaster> GetAllMasterConfiguration()
        {
            List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster = new List<DOMGR_ConfigMaster>();

            try
            {
                GetAllConfigurationIfNoCache();
                lstDOMGR_ConfigMaster = GetCache(ConstantTexts.MasterConfigurarionsCacheKey) as List<DOMGR_ConfigMaster>;
                return lstDOMGR_ConfigMaster;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Get Master Configuration By Name
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetMasterConfigurationByName(string configName)
        {
            List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster = new List<DOMGR_ConfigMaster>();
            string configValue = string.Empty;
            try
            {
                GetAllConfigurationIfNoCache();
                lstDOMGR_ConfigMaster = GetCache(ConstantTexts.MasterConfigurarionsCacheKey) as List<DOMGR_ConfigMaster>;
                if (lstDOMGR_ConfigMaster.Where(xx => xx.ConfigName == configName && xx.IsActive == true).Count() > 0)
                {
                    configValue = lstDOMGR_ConfigMaster.Where(xx => xx.ConfigName == configName && xx.IsActive == true).FirstOrDefault().ConfigValue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return configValue;
        }
        /// <summary>
        /// Get lookup value by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetLookupValueById(long? id)
        {
            try
            {
                if (id == null)
                    return string.Empty;
                //fetch all lookupsDataSet if not present cache
                GetAllLookupsIfNoCache();
                List<DOCMN_LookupMaster> lstDOCMN_LookupMaster = GetCache(ConstantTexts.LookupMasterCacheKey) as List<DOCMN_LookupMaster>;
                return lstDOCMN_LookupMaster.Where(x => x.CMN_LookupMasterId.Equals(id)).FirstOrDefault().LookupValue;
            }
            catch (Exception ex)
            {

            }
            return string.Empty;
        }

        /// <summary>
        /// Get lookup by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DOCMN_LookupMaster GetLookupById(long? id)
        {
            try
            {
                if (id == null)
                    return null;
                //fetch all lookupsDataSet if not present cache
                GetAllLookupsIfNoCache();
                List<DOCMN_LookupMaster> lstDOCMN_LookupMaster = GetCache(ConstantTexts.LookupMasterCacheKey) as List<DOCMN_LookupMaster>;
                return lstDOCMN_LookupMaster.Where(x => x.CMN_LookupMasterId.Equals(id)).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return null;
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
                List<DOCMN_LookupMaster> lstDOCMN_LookupMaster = GetCache(ConstantTexts.LookupMasterCacheKey) as List<DOCMN_LookupMaster>;
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

        public static List<DOADM_UserPreference> GetAllUserPreferenceFromCache(long? id)
        {
            try
            {
                GetAllLookupsIfNoCache();
                List<DOADM_UserPreference> lstDOADM_UserPreference = GetCache(ConstantTexts.UserPreferenceCacheKey) as List<DOADM_UserPreference>;
                if (id != null)
                {
                    return lstDOADM_UserPreference = lstDOADM_UserPreference.Where(x => x.ADM_UserMasterRef.Equals(id)).OrderBy(x => x.ADM_UserPreferenceId).ToList();
                }
                else
                    return lstDOADM_UserPreference;
            }
            catch
            {
                return null;
            }
        }

        public static List<DOCMN_LookupMasterCorrelations> GetAllLookupMasterCorrelationFromCache(long? lookupTypeCorrelation, long? parentLkup, long? groupLkup = null)
        {
            try
            {
                GetAllLookupsCorrelationsIfNoCache();
                List<DOCMN_LookupMasterCorrelations> lstDOCMN_LookupMasterCorrelation = GetCache(ConstantTexts.LookupMasterCorrelationCacheKey) as List<DOCMN_LookupMasterCorrelations>;
                if (lookupTypeCorrelation != null)
                    lstDOCMN_LookupMasterCorrelation = lstDOCMN_LookupMasterCorrelation.Where(x => x.CMN_LookupTypeCorrelationsRef == lookupTypeCorrelation && x.IsActive == true).ToList();
                if (groupLkup != null && parentLkup != null)
                    lstDOCMN_LookupMasterCorrelation = lstDOCMN_LookupMasterCorrelation.Where(x => x.GroupingLookupMasterRef == groupLkup && x.CMN_LookupMasterParentRef == parentLkup && x.IsActive == true).ToList();
                else if (parentLkup != null)
                    lstDOCMN_LookupMasterCorrelation = lstDOCMN_LookupMasterCorrelation.Where(x => x.CMN_LookupMasterParentRef == parentLkup && x.IsActive == true).ToList();

                return lstDOCMN_LookupMasterCorrelation.OrderBy(x => x.LookupMasterChildValue).ToList();
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
            if (System.Web.HttpContext.Current.Cache[ConstantTexts.LookupTypeCacheKey] == null ||
                System.Web.HttpContext.Current.Cache[ConstantTexts.LookupMasterCacheKey] == null)
            {
                List<DOCMN_LookupMaster> lstDOCMN_LookupMaster;
                List<DOCMN_LookupType> lstDOCMN_LookupType;
                BLLookup objBLLookup = new BLLookup();
                ExceptionTypes exResult = objBLLookup.GetAllLookups(null, out lstDOCMN_LookupType, out lstDOCMN_LookupMaster);

                AddToCache(ConstantTexts.LookupTypeCacheKey, lstDOCMN_LookupType);
                lstDOCMN_LookupMaster = lstDOCMN_LookupMaster.Where(x => x.IsActive == true).ToList();
                AddToCache(ConstantTexts.LookupMasterCacheKey, lstDOCMN_LookupMaster);
            }
        }

        /// <summary>
        /// Fetch all Bulk Import Excel Template and column mappings in cache
        /// </summary>
        private static void GetAllBulkImportExcelTemplateIfNoCache()
        {
            string errorMessage = string.Empty;
            BLBulkUpload objBLBulkUpload = new BLBulkUpload();
            if (System.Web.HttpContext.Current.Cache[ConstantTexts.BulkImportExcelTemplateCacheKey] == null ||
                System.Web.HttpContext.Current.Cache[ConstantTexts.BulkImportExcelColumnMappingCacheKey] == null)
            {
                List<DOGEN_BulkImportExcelTemplateMaster> lstDOGEN_BulkImportExcelTemplateMaster = new List<DOGEN_BulkImportExcelTemplateMaster>();
                List<DOGEN_BulkImportColumnsMapping> lstDOGEN_BulkImportColumnsMapping = new List<DOGEN_BulkImportColumnsMapping>();
                UIDOGEN_BulkImportExcelTemplate objUIDOGEN_BulkImportExcelTemplate = new UIDOGEN_BulkImportExcelTemplate();
                ExceptionTypes exResult = objBLBulkUpload.GetBulkImportExcelTemplate(out objUIDOGEN_BulkImportExcelTemplate, out errorMessage);

                if (objUIDOGEN_BulkImportExcelTemplate.lstDOGEN_BulkImportExcelTemplateMaster.Count > 0)
                    lstDOGEN_BulkImportExcelTemplateMaster = objUIDOGEN_BulkImportExcelTemplate.lstDOGEN_BulkImportExcelTemplateMaster;
                if (objUIDOGEN_BulkImportExcelTemplate.lstDOGEN_BulkImportColumnsMapping.Count > 0)
                    lstDOGEN_BulkImportColumnsMapping = objUIDOGEN_BulkImportExcelTemplate.lstDOGEN_BulkImportColumnsMapping;

                AddToCache(ConstantTexts.BulkImportExcelTemplateCacheKey, lstDOGEN_BulkImportExcelTemplateMaster);
                AddToCache(ConstantTexts.BulkImportExcelColumnMappingCacheKey, lstDOGEN_BulkImportColumnsMapping);
            }
        }

        private static void GetAllLookupsCorrelationsIfNoCache()
        {
            long? TimeZone = (long)DefaultTimeZone.CentralStandardTime;
            string errorMsg = string.Empty;
            if (System.Web.HttpContext.Current.Cache[ConstantTexts.LookupTypeCorrelationCacheKey] == null ||
                System.Web.HttpContext.Current.Cache[ConstantTexts.LookupMasterCorrelationCacheKey] == null)
            {
                List<DOCMN_LookupTypeCorrelations> lstDOCMN_LookupTypeCorrelations;
                List<DOCMN_LookupMasterCorrelations> lstDOCMN_LookupMasterCorrelations;

                DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations = new DOCMN_LookupTypeCorrelations();
                objDOCMN_LookupTypeCorrelations.IsActive = true;

                BLLookupCorrelations objBLLookupCorrelation = new BLLookupCorrelations();
                ExceptionTypes exResult = objBLLookupCorrelation.GetAllLookupTypeCorrelations(TimeZone, objDOCMN_LookupTypeCorrelations, out lstDOCMN_LookupTypeCorrelations, out lstDOCMN_LookupMasterCorrelations, out errorMsg);

                AddToCache(ConstantTexts.LookupTypeCorrelationCacheKey, lstDOCMN_LookupTypeCorrelations);
                AddToCache(ConstantTexts.LookupMasterCorrelationCacheKey, lstDOCMN_LookupMasterCorrelations);
            }
        }

        private static void GetAllSkillsIfNoCache()
        {
            long? TimeZone = (long)DefaultTimeZone.CentralStandardTime;
            if (System.Web.HttpContext.Current.Cache[ConstantTexts.SkillCacheKey] == null)
            {
                BLSkills objBLSkills = new BLSkills();
                List<DOADM_SkillsMaster> lstDOADM_SkillsMaster = new List<DOADM_SkillsMaster>();
                DOADM_SkillsMaster objDOADM_SkillsMaster = new DOADM_SkillsMaster();
                objDOADM_SkillsMaster.IsActive = true;
                string errorMessage;
                ExceptionTypes skillResult = objBLSkills.SearchSkills(TimeZone, objDOADM_SkillsMaster, out lstDOADM_SkillsMaster, out errorMessage);
                if (skillResult != ExceptionTypes.Success)
                {
                    //Log
                }
                AddToCache(ConstantTexts.SkillCacheKey, lstDOADM_SkillsMaster);
            }
        }

        /// <summary>
        /// Fetch all skillsDataSet of particular type in which id belongs
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<DOADM_SkillsMaster> GetAllSkillsFromCache(long? id)
        {
            try
            {
                GetAllSkillsIfNoCache();
                List<DOADM_SkillsMaster> lstDOADM_SkillsMaster = GetCache(ConstantTexts.SkillCacheKey) as List<DOADM_SkillsMaster>;
                if (id != null)
                {
                    return lstDOADM_SkillsMaster = lstDOADM_SkillsMaster.Where(x => x.ADM_SkillsMasterId.Equals(id)).OrderBy(x => x.SkillsName).ToList();
                }
                else
                    return lstDOADM_SkillsMaster;
            }
            catch
            {
                return null;
            }
        }

        private static void GetAllReportsIfNoCache(long? id, string name)
        {
            if (System.Web.HttpContext.Current.Cache[ConstantTexts.ReportCacheKey] == null)
            {
                BLReports objBLReports = new BLReports();

                List<DORPT_ReportsMaster> lstDORPT_ReportsMaster = new List<DORPT_ReportsMaster>();
                string errorMessage;
                ExceptionTypes reportResult = objBLReports.GetAllReports(id, name, out lstDORPT_ReportsMaster, out errorMessage);
                if (reportResult != ExceptionTypes.Success)
                {
                    //Log
                }
                AddToCache(ConstantTexts.ReportCacheKey, lstDORPT_ReportsMaster.Where(x => x.ViewInUI == true).ToList());
            }
        }
        /// <summary>
        /// Get All master configuration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        private static void GetAllConfigurationIfNoCache()
        {
            long? TimeZone = (long)DefaultTimeZone.CentralStandardTime;
            string errorMessage = string.Empty;
            if (System.Web.HttpContext.Current.Cache[ConstantTexts.MasterConfigurarionsCacheKey] == null)
            {
                BLConfigurations objBLConfigurations = new BLConfigurations();
                List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster = new List<DOMGR_ConfigMaster>();
                DOMGR_ConfigMaster objDOMGR_ConfigMaster = new DOMGR_ConfigMaster();
                objDOMGR_ConfigMaster.IsActive = true;
                ExceptionTypes reportResult = objBLConfigurations.SearchConfiguration(TimeZone,objDOMGR_ConfigMaster, out lstDOMGR_ConfigMaster, out errorMessage);
                AddToCache(ConstantTexts.MasterConfigurarionsCacheKey, lstDOMGR_ConfigMaster);
            }
        }

        /// <summary>
        /// Fetch all reportDataSet of particular type in which id belongs
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<DORPT_ReportsMaster> GetAllReportsFromCache(long? id, string name)
        {
            try
            {
                GetAllReportsIfNoCache(id, name);
                List<DORPT_ReportsMaster> lstDORPT_ReportsMaster = GetCache(ConstantTexts.ReportCacheKey) as List<DORPT_ReportsMaster>;
                if (id != null)
                {
                    return lstDORPT_ReportsMaster = lstDORPT_ReportsMaster.Where(x => x.RPT_ReportsMasterId.Equals(id)).OrderBy(x => x.ReportName).ToList();
                }
                else if (name != string.Empty)
                {
                    return lstDORPT_ReportsMaster = lstDORPT_ReportsMaster.Where(x => x.ReportName.Contains(name)).OrderBy(x => x.ReportName).ToList();
                }
                else
                    return lstDORPT_ReportsMaster;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Fetch all lookupsDataSet of particular type in which id belongs
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static UIDOGEN_BulkImportExcelTemplate GetBulkImportExcelTemplateFromCache()
        {
            try
            {
                GetAllBulkImportExcelTemplateIfNoCache();
                UIDOGEN_BulkImportExcelTemplate objUIDOGEN_BulkImportExcelTemplate = new UIDOGEN_BulkImportExcelTemplate();
                objUIDOGEN_BulkImportExcelTemplate.lstDOGEN_BulkImportExcelTemplateMaster = GetCache(ConstantTexts.BulkImportExcelTemplateCacheKey) as List<DOGEN_BulkImportExcelTemplateMaster>;
                objUIDOGEN_BulkImportExcelTemplate.lstDOGEN_BulkImportColumnsMapping = GetCache(ConstantTexts.BulkImportExcelColumnMappingCacheKey) as List<DOGEN_BulkImportColumnsMapping>;
                return objUIDOGEN_BulkImportExcelTemplate;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Used to verify user permissions
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="permissionType"></param>
        /// <param name="discCat"></param>
        /// <returns>bool</returns>
        public static bool CheckUserPermission(UIUserLogin currentUser, long permissionType, long discCat, long lWorkQueue=0)
        {
            if (currentUser.UserSkills != null && currentUser.UserSkills.Count > 0)
            {
               // bool isWorkQueueAvailable = true;
                if (lWorkQueue != 0)
                {
                   bool isWorkQueueAvailable = currentUser.UserSkills.Any(x => x.WorkQueuesLkup == lWorkQueue);

                    switch (permissionType)
                    {
                        case (long)PermissionType.CanClone:
                            return currentUser.UserSkills.Any(x => x.CanClone && x.DiscrepancyCategoryLkup == discCat && x.WorkQueuesLkup == lWorkQueue);

                        case (long)PermissionType.CanCreate:
                            return currentUser.UserSkills.Any(x => x.CanCreate && x.DiscrepancyCategoryLkup == discCat && x.WorkQueuesLkup == lWorkQueue);

                        case (long)PermissionType.CanHistory:
                            return currentUser.UserSkills.Any(x => x.CanHistory && x.DiscrepancyCategoryLkup == discCat && x.WorkQueuesLkup == lWorkQueue);

                        case (long)PermissionType.CanMassUpdate:
                            return currentUser.UserSkills.Any(x => x.CanMassUpdate && x.DiscrepancyCategoryLkup == discCat && x.WorkQueuesLkup == lWorkQueue);

                        case (long)PermissionType.CanModify:
                            return currentUser.UserSkills.Any(x => x.CanModify && x.DiscrepancyCategoryLkup == discCat && x.WorkQueuesLkup == lWorkQueue);

                        case (long)PermissionType.CanReassign:
                            return currentUser.UserSkills.Any(x => x.CanReassign && x.DiscrepancyCategoryLkup == discCat && x.WorkQueuesLkup == lWorkQueue);

                        case (long)PermissionType.CanSearch:
                            return currentUser.UserSkills.Any(x => x.CanSearch && x.DiscrepancyCategoryLkup == discCat && x.WorkQueuesLkup == lWorkQueue);

                        case (long)PermissionType.CanUnlock:
                            return currentUser.UserSkills.Any(x => x.CanUnlock && x.DiscrepancyCategoryLkup == discCat && x.WorkQueuesLkup == lWorkQueue);

                        case (long)PermissionType.CanUpload:
                            return currentUser.UserSkills.Any(x => x.CanUpload && x.DiscrepancyCategoryLkup == discCat && x.WorkQueuesLkup == lWorkQueue);

                        case (long)PermissionType.CanView:
                            return currentUser.UserSkills.Any(x => x.CanView && x.DiscrepancyCategoryLkup == discCat && x.WorkQueuesLkup == lWorkQueue);

                        case (long)PermissionType.CanReopen:
                            return currentUser.UserSkills.Any(x => x.CanReopen && x.DiscrepancyCategoryLkup == discCat && x.WorkQueuesLkup == lWorkQueue);
                    }
                }
                else
                {
                    switch (permissionType)
                    {
                        case (long)PermissionType.CanClone:
                            return currentUser.UserSkills.Any(x => x.CanClone && x.DiscrepancyCategoryLkup == discCat);

                        case (long)PermissionType.CanCreate:
                            return currentUser.UserSkills.Any(x => x.CanCreate && x.DiscrepancyCategoryLkup == discCat);

                        case (long)PermissionType.CanHistory:
                            return currentUser.UserSkills.Any(x => x.CanHistory && x.DiscrepancyCategoryLkup == discCat);

                        case (long)PermissionType.CanMassUpdate:
                            return currentUser.UserSkills.Any(x => x.CanMassUpdate && x.DiscrepancyCategoryLkup == discCat);

                        case (long)PermissionType.CanModify:
                            return currentUser.UserSkills.Any(x => x.CanModify && x.DiscrepancyCategoryLkup == discCat);

                        case (long)PermissionType.CanReassign:
                            return currentUser.UserSkills.Any(x => x.CanReassign && x.DiscrepancyCategoryLkup == discCat);

                        case (long)PermissionType.CanSearch:
                            return currentUser.UserSkills.Any(x => x.CanSearch && x.DiscrepancyCategoryLkup == discCat);

                        case (long)PermissionType.CanUnlock:
                            return currentUser.UserSkills.Any(x => x.CanUnlock && x.DiscrepancyCategoryLkup == discCat);

                        case (long)PermissionType.CanUpload:
                            return currentUser.UserSkills.Any(x => x.CanUpload && x.DiscrepancyCategoryLkup == discCat);

                        case (long)PermissionType.CanView:
                            return currentUser.UserSkills.Any(x => x.CanView && x.DiscrepancyCategoryLkup == discCat);

                        case (long)PermissionType.CanReopen:
                            return currentUser.UserSkills.Any(x => x.CanReopen && x.DiscrepancyCategoryLkup == discCat);
                    }
                }
            }
            return false;
        }
    }
}