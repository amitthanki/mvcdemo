using ENRLReconSystem.DAL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.BL
{
    public class BLSkills
    {
        public ExceptionTypes retValue;
        public ExceptionTypes SearchSkills(long? TimeZone,DOADM_SkillsMaster objDOADM_SkillsDetails, out List<DOADM_SkillsMaster> lstDOADM_SkillsMaster, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALSkills objDALResources = new DALSkills();
            return retValue = objDALResources.SearchSkills(TimeZone,objDOADM_SkillsDetails, out lstDOADM_SkillsMaster, out errorMessage);
        }

        public ExceptionTypes SaveSkills(DOADM_SkillsMaster objDOADM_SkillsMaster, long lLoginId, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALSkills objDALSkills = new DALSkills();
            return retValue = objDALSkills.AddSkills(objDOADM_SkillsMaster, lLoginId, out errorMessage);
        }
    }
}
