using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using ENRLReconSystem.DAL;

namespace ENRLReconSystem.BL
{
    public class BLRPR
    {
        DALRPR _objDALRPR = new DALRPR();
        ExceptionTypes _retValue;
        public ExceptionTypes Create(DOGEN_Queue objDOGEN_Queue, List<DOGEN_Attachments> lstDOGEN_Attachments, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALRPR.Create(objDOGEN_Queue, lstDOGEN_Attachments, out errorMessage);
        }

        public ExceptionTypes GetGenQueueByID(long? TimeZone,long genQueueID, out DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALRPR.GetGenQueueByID(TimeZone,genQueueID, out objDOGEN_Queue, out errorMessage);
        }

        public ExceptionTypes SaveRPRActions(DOGEN_RPRActions objDOGEN_RPRActions, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALRPR.SaveRPRActions(objDOGEN_RPRActions, out errorMessage);
        }

        public ExceptionTypes CheckDuplicate(string strMemberCurrentHICN, long? lMemberContractIDLkup, DateTime? dtRPRRequestedEffectiveDate, long? lRPRActionRequestedLkup, out long lCaseID)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALRPR.CheckDuplicate(strMemberCurrentHICN, lMemberContractIDLkup, dtRPRRequestedEffectiveDate, lRPRActionRequestedLkup, out lCaseID);
        }
    }
}
