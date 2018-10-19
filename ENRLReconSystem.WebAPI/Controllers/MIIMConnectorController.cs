using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ENRLReconSystem.DO.DataObjects;
using ENRLReconSystem.Utility;
using ENRLReconSystem.BL;
using System.Net.Http.Formatting;
using System.Data;
using System.Web;
using ENRLReconSystem.DO;
using System.Reflection;

namespace ENRLReconSystem.WebAPI.Controllers
{
    [Authorize]
    [AuthenticateMIIMUser(Roles = "MIIMUser")]
    public class MIIMConnectorController : ApiController
    {

        DOGEN_MIIMServiceTrace _objDOGEN_MIIMServiceTrace;
        BLServiceRequestResponse _objBLServiceRequestResponse = new BLServiceRequestResponse();
        long _userid = 6;

        /// <summary>
        /// Get OST Member details by receiving HICN input from MIIM
        /// </summary>
        /// <param name="HICN"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetERSAllQueueDetailsByHICN(string memberHICN)
        {
            MIIMServiceLog(MethodBase.GetCurrentMethod().Name, memberHICN, ConstantTexts.MIIMRecordFound, _userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.GetERSAllQueueDetailsByHICN);
            string RequestInputData = "Member HICN: " + memberHICN;
            HttpResponseMessage response = new HttpResponseMessage();
            string error = string.Empty;
            long userid = _userid;
            try
            {
                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMNew, userid, TarceMethodLkup.New.ToLong(), (long)MIIMServiceMethod.GetERSAllQueueDetailsByHICN);
                if (CheckUserDBAccess(out userid, out error))
                {
                    ///Log For User Authentication Success
                    MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMUserAuthSucc, userid, TarceMethodLkup.New.ToLong(), (long)MIIMServiceMethod.GetERSAllQueueDetailsByHICN);

                    BLMIIMIntegration objGetQueueDetailsByHICN = new BLMIIMIntegration();
                    List<DOMIIMGetQueue> lstDOMIIMGetQueue = new List<DOMIIMGetQueue>();
                    lstDOMIIMGetQueue = objGetQueueDetailsByHICN.GetQueueDetailsByHICN(memberHICN);
                    if (lstDOMIIMGetQueue.Count > 0)
                    {
                        var responseData = new { IsSuccess = true, Message = "", data = lstDOMIIMGetQueue };
                        //success with data
                        response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                        //Log
                        MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMRecordFound + String.Join(",",lstDOMIIMGetQueue.Select(x => x.CaseId).ToArray()), userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.GetERSAllQueueDetailsByHICN);

                    }
                    else
                    {
                        var responseData = new { IsSuccess = true, Message = "No records found for provided input", data = new List<string>() };
                        //success with no data
                        response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                        ///Log For User Authentication Success
                        MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMRecordNotFound, userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.GetERSAllQueueDetailsByHICN);
                    }
                }
                else
                {
                    var responseData = new { IsSuccess = false, Message = error, data = new List<string>() };
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                    ///Log For User Authentication fail
                    MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMUserAuthFail + ":" + error, userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.GetERSAllQueueDetailsByHICN);
                }
            }
            catch (Exception ex)
            {
                var responseData = new { IsSuccess = false, Message = "An error Occured", data = new List<string>() };
                response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                ///Log For error
                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ex.NullToString(), userid, TarceMethodLkup.Failed.ToLong(), (long)MIIMServiceMethod.GetERSAllQueueDetailsByHICN);
                BLCommon.LogError(userid, "GetERSAllQueueDetailsByHICN", (long)ErrorModuleName.MIIMService,(long)ExceptionTypes.Exception, "Exception in GetERSAllQueueDetailsByHICN", ex.ToString());
            }
            ///Log For Completed
            MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMRequestCompleted, userid, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.GetERSAllQueueDetailsByHICN);
            return response;

        }

        [HttpGet]
        public HttpResponseMessage GetQueueIdFromMIIMReferenceId(string ReferenceId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            BLMIIMIntegration objBLMIIMIntegration = new BLMIIMIntegration();
            string ersCaseId;
            string error = string.Empty;
            long userid = _userid;
            string RequestInputData = "ReferenceId: " + ReferenceId;
            MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMNew, userid, TarceMethodLkup.New.ToLong(), (long)MIIMServiceMethod.GetQueueIdFromMIIMReferenceId);
            try
            {
                if (CheckUserDBAccess(out userid, out error))
                {
                    ///Log For User Authentication Success
                    MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMUserAuthSucc, userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.GetQueueIdFromMIIMReferenceId);

                    objBLMIIMIntegration.GetQueueIdFromMIIMRefernceId(ReferenceId, out ersCaseId, out error);
                    if (ersCaseId != "")
                    {
                        var responseData = new { IsSuccess = true, Message = "", CaseID = ersCaseId };
                        response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                        MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMRecordFound + ersCaseId, userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.GetQueueIdFromMIIMReferenceId);
                    }
                    else
                    {
                        var responseData = new { IsSuccess = true, Message = "No records found for provided input", CaseID = "" };
                        MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMRecordNotFound, userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.GetQueueIdFromMIIMReferenceId);
                        response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);

                    }
                }
                else
                {
                    var responseData = new { IsSuccess = false, Message = error, CaseID = "" };
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                    MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMUserAuthFail + ":" + error, userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.GetQueueIdFromMIIMReferenceId);
                }
            }
            catch (Exception ex)
            {
                var responseData = new { IsSuccess = false, Message = "An error occured", CaseID = "" };
                response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ex.NullToString(), userid, TarceMethodLkup.Failed.ToLong(), (long)MIIMServiceMethod.GetQueueIdFromMIIMReferenceId);
                BLCommon.LogError(userid,"GetQueueIdFromMIIMReferenceId", (long)ErrorModuleName.MIIMService, (long)ExceptionTypes.Exception, "Exception in GetQueueIdFromMIIMReferenceId", ex.ToString());
            }
            ///Log For Completed
            MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMRequestCompleted, userid, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.GetQueueIdFromMIIMReferenceId);
            return response;
        }

        [Route("api/MIIMConnector/PostOOAPermanentAddressTrackComments")]
        [HttpPost]
        public HttpResponseMessage PostOOAPermanentAddressTrackComments(List<DOMIIMOOACommentUpdate> lstDOMIIMOOACommentUpdate)
        {

            HttpResponseMessage response = new HttpResponseMessage();
            long userid = _userid;
            string error=string.Empty;
            string RequestInputData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lstDOMIIMOOACommentUpdate).ToString();
            try
            {
                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMNew, userid, TarceMethodLkup.New.ToLong(), (long)MIIMServiceMethod.PostOOAPermanentAddressTrackComments);
                if (CheckUserDBAccess(out userid, out error))
                {
                    ///Log For User Authentication Success
                    MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMUserAuthSucc, userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.PostOOAPermanentAddressTrackComments);
                    BLMIIMIntegration objBLMIIMIntegration = new BLMIIMIntegration();
                    long result = objBLMIIMIntegration.UpdateOOAMIIMComments(lstDOMIIMOOACommentUpdate.Where(x => x.ERSCaseId != 0).ToList(), userid);//filtering invalid ERS case Id
                    if (result == 0)
                    {
                        var responseData = new { IsSuccess = true, Message = "Comments are updated" };
                        response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                        MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, "Comments are updated", userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.PostOOAPermanentAddressTrackComments);
                    }
                    else
                    {
                        var responseData = new { IsSuccess = true, Message = "Error while updating" };
                        response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                        MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, "Error while updating", userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.PostOOAPermanentAddressTrackComments);
                    }
                }
                else
                {
                    var responseData = new { IsSuccess = false, Message = error };
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                    MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMUserAuthFail + ":" + error, userid, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.PostOOAPermanentAddressTrackComments);
                }

            }
            catch (Exception ex)
            {
                var responseData = new { IsSuccess = false, Message = "An error occured"};
                response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ex.NullToString(), userid, TarceMethodLkup.Failed.ToLong(), (long)MIIMServiceMethod.PostOOAPermanentAddressTrackComments);
                BLCommon.LogError(userid, "PostOOAPermanentAddressTrackComments", (long)ErrorModuleName.MIIMService, (long)ExceptionTypes.Exception, "Exception in PostOOAPermanentAddressTrackComments", ex.ToString());
            }
            ///Log For Completed
            MIIMServiceLog(MethodBase.GetCurrentMethod().Name, RequestInputData, ConstantTexts.MIIMRequestCompleted, userid, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.PostOOAPermanentAddressTrackComments);
            return response;
        }
                
        public bool CheckUserDBAccess(out long userId, out string errosMsg)
        {
            userId = _userid;
            errosMsg = string.Empty;
            string loginName = HttpContext.Current.User.Identity.Name.Replace("MS\\", "");
            BLUserAdministration objBLUserAdministration = new BLUserAdministration();
            UIUserLogin loggedInUser;
            ExceptionTypes res = objBLUserAdministration.GetUserAccessPermission(loginName, null, null, null, out loggedInUser);
            if (res == ExceptionTypes.ZeroRecords)
            {
                errosMsg = "You are not part of ERS DB, Please contact your Administrator.";
                return false;
            }
            else if (res != ExceptionTypes.Success)
            {
                errosMsg = "An error occured while authorization, Please contact your Administrator.";
                return false;
            }
            else
            {
                userId = loggedInUser.ADM_UserMasterId;
                return true;
            }

        }

        /// <summary>
        /// Child method to log MIIM Request Response
        /// </summary>
        /// <param name="name"></param>
        /// <param name="requestInputData"></param>
        /// <param name="responseMsg"></param>
        /// <param name="userid"></param>
        /// <param name="new"></param>
        private void MIIMServiceLog(string name, string requestInputData, string responseMsg, long userid, long traceLkup, long WebServiceMethodLkup)
        {
            _objDOGEN_MIIMServiceTrace = new DOGEN_MIIMServiceTrace();
            try
            {
                _objDOGEN_MIIMServiceTrace.WebServiceMethodName = name;
                _objDOGEN_MIIMServiceTrace.RequestInputData = requestInputData;
                _objDOGEN_MIIMServiceTrace.ResponseStatusMessage = responseMsg;
                _objDOGEN_MIIMServiceTrace.CreatedByRef = userid;
                _objDOGEN_MIIMServiceTrace.TarceMethodLkup = traceLkup;
                _objDOGEN_MIIMServiceTrace.WebServiceMethodLkup = WebServiceMethodLkup;
                _objBLServiceRequestResponse.MIIMServiceLog(_objDOGEN_MIIMServiceTrace);

            }
            catch (Exception ex)
            {
                BLCommon.LogError(userid, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MIIMService, (long)ExceptionTypes.Uncategorized, ex.ToString(), "Error occured while MIIMServiceLog.");
            }
        }
    }
}
