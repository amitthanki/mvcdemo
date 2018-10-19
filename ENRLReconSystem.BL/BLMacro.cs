using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DO;
using ENRLReconSystem.DO.DataObjects;
using ENRLReconSystem.DAL;
using ENRLReconSystem.Utility;
using System.Data;

namespace ENRLReconSystem.BL
{
   public class BLMacro
    {
        public List<DOMacroData> GetOpenNotMacro(string p_HouseholdID, string p_HICN, string p_Contract, String p_PBP, string p_EffectiveDate, string p_DiscrepancyType,out string errorMessage)
        {
            DALMacro objDALMacro = new DALMacro();
            ExceptionTypes result = objDALMacro.GetOpenNotMacro(p_HouseholdID,p_HICN,p_Contract,p_PBP,p_EffectiveDate,p_DiscrepancyType, out List<DOMacroData> Queues,out errorMessage);
            return Queues;
        }

        public long UpdateOpenNotMacro(DOMacroUpdate objDOMacroUpdate, long userid, out string errorMessage)
        {
            DALMacro objDALMacro = new DALMacro();
            ExceptionTypes result = new ExceptionTypes();
            result = objDALMacro.UpdateOpenNotMacro(objDOMacroUpdate, userid,out errorMessage);
            return (long)result;
        }

        public long UpdateFTTMacro(DOMacroUpdate objDOMacroUpdate, long userid, out string errorMessage)
        {
            DALMacro objDALMacro = new DALMacro();
            ExceptionTypes result = new ExceptionTypes();
            result = objDALMacro.UpdateFTTMacro(objDOMacroUpdate, userid, out errorMessage);
            return (long)result;
        }

        /// <summary>
        /// Method to get the cases for FTT Macros
        /// </summary>
        /// <param name="p_HouseholdID"></param>
        /// <param name="p_HICN"></param>
        /// <param name="p_Contract"></param>
        /// <param name="p_PBP"></param>
        /// <param name="p_EffectiveDate"></param>
        /// <param name="p_DiscrepancyType"></param>
        /// <returns></returns>
        public List<DOMacroData> GetCasesForFTTMacro(string p_HouseholdID, string p_HICN, string p_Contract, String p_PBP, string p_EffectiveDate, string p_DiscrepancyType, out string errorMessage)
        {
            DALMacro objDALMacro = new DALMacro();
            ExceptionTypes result = objDALMacro.GetCasesForFTTMacro(p_HouseholdID, p_HICN, p_Contract, p_PBP, p_EffectiveDate, p_DiscrepancyType, out List<DOMacroData> Queues,out errorMessage);
            return Queues;
        }

        /// <summary>
        /// Method to get the cases for TRC155 macros
        /// </summary>
        /// <param name="p_HouseholdID"></param>
        /// <param name="p_HICN"></param>
        /// <param name="p_Contract"></param>
        /// <param name="p_PBP"></param>
        /// <param name="p_EffectiveDate"></param>
        /// <param name="p_DiscrepancyType"></param>
        /// <returns></returns>
        public List<DOMacroData> GetCasesForTRC155Macro(string p_HouseholdID, string p_HICN, string p_Contract, String p_PBP, string p_EffectiveDate, string p_DiscrepancyType, out string errorMessage)
        {
            DALMacro objDALMacro = new DALMacro();
            ExceptionTypes result = objDALMacro.GetCasesForTRC155Macro(p_HouseholdID, p_HICN, p_Contract, p_PBP, p_EffectiveDate, p_DiscrepancyType, out List<DOMacroData> Queues,out errorMessage);
            return Queues;
        }

        /// <summary>
        /// Method to update the cases for TRC155 Macro
        /// </summary>
        /// <param name="objDOMacroUpdate"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public long UpdateCaseForTRC155Macro(DOMacroUpdate objDOMacroUpdate, long userid,out string errorMessage)
        {
            DALMacro objDALMacro = new DALMacro();
            ExceptionTypes result = new ExceptionTypes();
            result = objDALMacro.UpdateCaseForTRC155Macro(objDOMacroUpdate, userid, out errorMessage);
            return (long)result;
        }

    }
}
