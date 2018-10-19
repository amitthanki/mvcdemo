using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ENRLReconSystem.WebAPI.Models;
using ENRLReconSystem.Utility;
using System.Reflection;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web;

namespace ENRLReconSystem.WebAPI.Controllers
{
    public class GetGpsMemberDataController : ApiController
    {
       
        [HttpGet]        
        public HttpResponseMessage GetMBI(string MBI,long UserID)
        {
         HttpResponseMessage responseMessage = new HttpResponseMessage();
         DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
         HttpContextBase abstractContext = new HttpContextWrapper(System.Web.HttpContext.Current);
         var url = abstractContext.Request.Url;
         List<DOGEN_GPSData> lstDOGEN_GPSData = new List<DOGEN_GPSData>();
            try
            {
               
                lstDOGEN_GPSData = PPersonSearch(MBI,UserID).ToList();
                var output = JsonConvert.SerializeObject(lstDOGEN_GPSData);
                //var responseData = new { IsSuccess = true, Message = "", data = lstDOGEN_GPSData };
                var responseData = output;
                objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);                
                
            }
            catch (Exception ex)
            {
                BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            objDOGEN_AEGPSServiceTrace.CreatedByRef = UserID;
            objDOGEN_AEGPSServiceTrace.RequestData = url.ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName= MethodBase.GetCurrentMethod().Name;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetMemberDemographicalDetails;
            objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
            TraceWebApi(objDOGEN_AEGPSServiceTrace);
            return responseMessage;
        }

        [HttpGet]
        public HttpResponseMessage GetHouseHoldID(string HouseHoldID,long UserID)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            HttpContextBase abstractContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            var url = abstractContext.Request.Url;
            List<DOGEN_GPSData> lstDOGEN_GPSData = new List<DOGEN_GPSData>();
            try
            {               
                lstDOGEN_GPSData = GetMemberInfoByHouseHoldID(HouseHoldID,UserID).ToList();
                var output = JsonConvert.SerializeObject(lstDOGEN_GPSData);
                //var responseData = new { IsSuccess = true, Message = "", data = lstDOGEN_GPSData };
                var responseData = output;
                objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

               
            }
            catch (Exception ex)
            {
                BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            objDOGEN_AEGPSServiceTrace.CreatedByRef = UserID;
            objDOGEN_AEGPSServiceTrace.RequestData = url.ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().Name;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetMemberEligibilityService;
            objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
            TraceWebApi(objDOGEN_AEGPSServiceTrace);
            return responseMessage;
        }
        [Route("GetEmployerSummary/{HouseHoldID}/{UserID}")]
        [HttpGet]
        public HttpResponseMessage GetEmployerSummary(string HouseHoldID, long UserID)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            HttpContextBase abstractContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            var url = abstractContext.Request.Url;
            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
            try
            {
                
                objDOGEN_GPSData = GetEmployerSummaryDetails(HouseHoldID, UserID);
                var output = JsonConvert.SerializeObject(objDOGEN_GPSData);
                //var responseData = new { IsSuccess = true, Message = "", data = lstDOGEN_GPSData };
                var responseData = output;
                objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

               
            }
            catch (Exception ex)
            {
                BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            objDOGEN_AEGPSServiceTrace.CreatedByRef = UserID;
            objDOGEN_AEGPSServiceTrace.RequestData = url.ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().Name;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetEmployerSummary;
            objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
            TraceWebApi(objDOGEN_AEGPSServiceTrace);
            return responseMessage;
        }

        [HttpGet]
        public HttpResponseMessage GetTRRData(string IndividualID, long UserID)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            HttpContextBase abstractContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            var url = abstractContext.Request.Url;
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            try
            {             
                objDOGEN_Queue = PGetTRRData(IndividualID,UserID);
                var output = JsonConvert.SerializeObject(objDOGEN_Queue);
                //var responseData = new { IsSuccess = true, Message = "", data = lstDOGEN_GPSData };
                var responseData = output;
                objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

               
            }
            catch (Exception ex)
            {
                BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            objDOGEN_AEGPSServiceTrace.CreatedByRef = UserID;
            objDOGEN_AEGPSServiceTrace.RequestData = url.ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().Name;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetTRRSummaryInfoService;
            objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
            TraceWebApi(objDOGEN_AEGPSServiceTrace);
            return responseMessage;
        }

        [NonAction]
        private List<DOGEN_GPSData> GetMemberInfoByHouseHoldID(string gpsHouseholdId, long UserID)
        {
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            objDOGEN_GPSServiceRequestParameter.HouseholdId = gpsHouseholdId;
            List<DOGEN_GPSData> lstDOGEN_GPSData = new List<DOGEN_GPSData>();
            string errorMessage = string.Empty;
            try
            {
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = UserID;
                objGPSServiceGetMethods.GetMemberEligibilityService(objDOGEN_GPSServiceRequestParameter, out lstDOGEN_GPSData, out errorMessage);
                if (!errorMessage.IsNullOrEmpty())
                {
                    BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                }
                return lstDOGEN_GPSData;

            }
            catch (Exception ex)
            {
                BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }

        }

        [NonAction]
        private DOGEN_GPSData GetEmployerSummaryDetails(string gpsHouseholdId,long UserID)
        {
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            objDOGEN_GPSServiceRequestParameter.HouseholdId = gpsHouseholdId;
            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
            string errorMessage = string.Empty;
            try
            {
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = UserID;
                objGPSServiceGetMethods.GetEmployerSummary(objDOGEN_GPSServiceRequestParameter, out objDOGEN_GPSData, out errorMessage);
                if (!errorMessage.IsNullOrEmpty())
                {
                    BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                }
                return objDOGEN_GPSData;

            }
            catch (Exception ex)
            {
                BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }

        }

        [NonAction]
        private List<DOGEN_GPSData> PPersonSearch(string hICNNumber,long UserID)
        {
            string errorMessage = string.Empty;
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber = hICNNumber;          
            List<DOGEN_GPSData> lstDOGEN_GPSData = new List<DOGEN_GPSData>();
            errorMessage = string.Empty;
            try
            {
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = UserID;
                objGPSServiceGetMethods.GetMemberDemographicalDetails(objDOGEN_GPSServiceRequestParameter, out lstDOGEN_GPSData, out errorMessage);
                if (!errorMessage.IsNullOrEmpty())
                {
                    BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                }
                return lstDOGEN_GPSData;

            }
            catch (Exception ex)
            {
                BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }

        }

        [NonAction]
        public DOGEN_Queue PGetTRRData(string IndividualID, long UserID)
        {
           string errorMessage = string.Empty;

            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            try
            {

                objDOGEN_GPSServiceRequestParameter.IndividualId = IndividualID;
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = UserID;
                //web service call for TRR data
                objGPSServiceGetMethods.GetTRRSummaryInfoService(objDOGEN_GPSServiceRequestParameter, ref objDOGEN_Queue, out errorMessage);
                if (!errorMessage.IsNullOrEmpty())
                {
                    BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                }
            }
            catch (Exception ex)
            {

                BLCommon.LogError(UserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());

            }

            return objDOGEN_Queue;
        }

        [Route("api/GetGpsMemberData/PostCMSTransaction")]
        [HttpPost]
        public HttpResponseMessage PostCMSTransaction(JObject jsonResult)
        {
           
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            HttpContextBase abstractContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            var url = abstractContext.Request.Url;
            long LoginUserID = 0;
            long Gen_QueueID = 0;
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DOGEN_GPSServiceRequestParameter item = JsonConvert.DeserializeObject<DOGEN_GPSServiceRequestParameter>(jsonResult.ToString());

                string errorMessage = string.Empty;               
                string BirthDate = item.BirthDate;
                string CaseNumber = item.CaseNumber;
                string ContractNumber = item.ContractNumber;
                string EffectiveEndDate = item.EffectiveEndDate;
                string EffectiveStartDate = item.EffectiveStartDate;
                string MedicareClaimNumber = item.MedicareClaimNumber;
                string LastName = item.LastName;
                string PbpNo = item.PbpNo;
                LoginUserID = item.LoggedInUserId;
                long? ActionLkup = item.ActionLkup;
                Gen_QueueID = Convert.ToInt64(item.ERSCaseId);
                DOCMSPostTransaction(BirthDate, CaseNumber, ContractNumber, EffectiveStartDate, EffectiveEndDate, MedicareClaimNumber, LastName, PbpNo,LoginUserID, ActionLkup, out errorMessage);
                if (errorMessage == "")
                {
                    var responseData = new { IsSuccess = true, data = errorMessage };
                    objDOGEN_AEGPSServiceTrace.ResponseData = JsonConvert.SerializeObject(responseData);
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                    //success with data
                    response = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);
                    // return response(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

                }
                else
                {
                    var responseData = new { IsSuccess = false, data = errorMessage };
                    objDOGEN_AEGPSServiceTrace.ResponseData = JsonConvert.SerializeObject(responseData);
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                    //failure with data
                    response = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

                }
            }
            catch (Exception ex)
            {
                var responseData = new { IsSuccess = false, data = ex.Message };
                objDOGEN_AEGPSServiceTrace.ResponseData = JsonConvert.SerializeObject(responseData);
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                //failure with data
                BLCommon.LogError(LoginUserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

            }

            objDOGEN_AEGPSServiceTrace.CreatedByRef = LoginUserID;
            objDOGEN_AEGPSServiceTrace.RequestData = jsonResult.ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().Name;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.CreateCMSTransactionService;
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = Gen_QueueID;
            TraceWebApi(objDOGEN_AEGPSServiceTrace);
            return response;
        }

        [NonAction]
        private void DOCMSPostTransaction(string BirthDate, string CaseNumber, string ContractNumber, string EffectiveStartDate, string EffectiveEndDate, string MedicareClaimNumber, string LastName, string PbpNo,long LoginUserID,long? ActionLkup, out string errorMessage)
        {
           
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            errorMessage = string.Empty;
            try
            {

                objDOGEN_GPSServiceRequestParameter.BirthDate = BirthDate;
                objDOGEN_GPSServiceRequestParameter.CaseNumber = CaseNumber;
                objDOGEN_GPSServiceRequestParameter.ContractNumber = ContractNumber;
                objDOGEN_GPSServiceRequestParameter.EffectiveEndDate = EffectiveEndDate;
                objDOGEN_GPSServiceRequestParameter.EffectiveStartDate = EffectiveStartDate;               
                objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber = MedicareClaimNumber;
                objDOGEN_GPSServiceRequestParameter.LastName = LastName;
                objDOGEN_GPSServiceRequestParameter.PbpNo = PbpNo;
                objDOGEN_GPSServiceRequestParameter.TransactionCode = ((long)CMSTransactionCode.TRR76).ToString();
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = LoginUserID;
                objDOGEN_GPSServiceRequestParameter.ActionLkup = ActionLkup;
                objGPSServiceGetMethods.CreateCMSTransactionService(objDOGEN_GPSServiceRequestParameter, out errorMessage);//Do CMS Post Transaction
            }
            catch (Exception ex)
            {
                BLCommon.LogError(LoginUserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }

        [Route("api/GetGpsMemberData/PostOutOfAreaService")]
        [HttpPost]
        public IHttpActionResult PostOutOfAreaService(JObject jsonResult)
        {
            long LoginUserID = 0;
            long ERSID = 0;
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            HttpContextBase abstractContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            var url = abstractContext.Request.Url;          
            //HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DOGEN_GPSServiceRequestParameter item = JsonConvert.DeserializeObject<DOGEN_GPSServiceRequestParameter>(jsonResult.ToString());
                string errorMessage = string.Empty;
                string CaseNumber = item.CaseNumber;
                string HouseholdId = item.HouseholdId;
                string DisenrollmentDate = item.OutOfAreaDisenrollmentDate;
                LoginUserID = item.LoggedInUserId;
                ERSID = Convert.ToInt64(item.ERSCaseId);
                DOOutOfAreaServiceService(CaseNumber, HouseholdId, DisenrollmentDate,LoginUserID,out errorMessage);
                objDOGEN_AEGPSServiceTrace.CreatedByRef = LoginUserID;
                objDOGEN_AEGPSServiceTrace.RequestData = jsonResult.ToString();
                objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().Name;
                objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.MaintainOutOfAreaServiceService;
                objDOGEN_AEGPSServiceTrace.GEN_QueueRef = ERSID;
                objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                TraceWebApi(objDOGEN_AEGPSServiceTrace);
                if (errorMessage == "")
                {
                    var responseData = new { IsSuccess = true, data = errorMessage };
                    //success with data
                    return Content(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

                }
                else
                {
                    var responseData = new { IsSuccess = false, data = errorMessage };
                    //failure with data
                    return Content(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

                }
                
            }
            catch (Exception ex)
            {
                var responseData = new { IsSuccess = false, data = ex.Message };
                //failure with data
                BLCommon.LogError(LoginUserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return Content(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

            }

        }

        [NonAction]
        private void DOOutOfAreaServiceService(string caseNumber, string householdId, string DisenrollmentDate, long loginUserID,out string errorMessage)
        {
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            errorMessage = string.Empty;
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            try
            {
                //requestDetails.applicationDate = objDOGEN_GPSServiceRequestParameter.ApplicationDate;              
                objDOGEN_GPSServiceRequestParameter.CaseNumber = caseNumber;
                objDOGEN_GPSServiceRequestParameter.HouseholdId = householdId;
                objDOGEN_GPSServiceRequestParameter.OutOfAreaDisenrollmentDate = DisenrollmentDate;
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = loginUserID;               
                objGPSServiceGetMethods.MaintainOutOfAreaServiceService(objDOGEN_GPSServiceRequestParameter, out errorMessage);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(loginUserID, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }

        public void TraceWebApi(DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace)
        {
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            objBLServiceRequestResponse.InsertAEGPSServiceTrace(objDOGEN_AEGPSServiceTrace);
        }

        //public HttpResponseMessage GetMemberHICN(string HICN)
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    try
        //    {
        //        DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
        //        string errorMessage = string.Empty;
        //        objDOGEN_GPSData = PPersonSearch(HICN, out errorMessage);               
        //        if (errorMessage == "")
        //        {
        //            var responseData = new { IsSuccess = true, data = objDOGEN_GPSData };
        //            //success with data
        //            response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //            return response;
        //        }
        //        else
        //        {
        //            var responseData = new { IsSuccess = false, data = errorMessage };
        //            //failure with data
        //            response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //            return response;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        var responseData = new { IsSuccess = false, data = ex.Message };
        //        //failure with data
        //        BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
        //        response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //        return response;
        //    }

        //}
        //    [NonAction]
        //    private DOGEN_GPSData PPersonSearch(string hICNNumber, out string errorMessage)
        //    {
        //        GPSServiceGetMethod objGPSServiceGetMethods = new GPSServiceGetMethod();
        //        DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
        //        objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber = hICNNumber;
        //        DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
        //        errorMessage = string.Empty;
        //        try
        //        {
        //            objDOGEN_GPSServiceRequestParameter.LoggedInUserId = 1;//currentUser.ADM_UserMasterId;
        //            objGPSServiceGetMethods.GetMemberDemographicalDetails(objDOGEN_GPSServiceRequestParameter, out objDOGEN_GPSData, out errorMessage);
        //            if (!errorMessage.IsNullOrEmpty())
        //            {
        //                BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
        //            }
        //            return objDOGEN_GPSData;

        //        }
        //        catch (Exception ex)
        //        {
        //            BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
        //            throw ex;
        //        }

        //    }
        //    [NonAction]
        //    private List<MacroGetEffectiveDate> PEligibilitySearch(string AccountID, out string errorMessage)
        //    {
        //        GPSServiceGetMethod objGPSServiceGetMethods = new GPSServiceGetMethod();
        //        DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
        //        objDOGEN_GPSServiceRequestParameter.HouseholdId = AccountID;
        //        //objDOGEN_GPSServiceRequestParameter.EmployerId = EmployeeID;
        //        MacroGetEffectiveDate objMacroGetEffectiveDate = new MacroGetEffectiveDate();
        //        List<MacroGetEffectiveDate> lstDOGEN_GPSData = new List<MacroGetEffectiveDate>();
        //        errorMessage = string.Empty;
        //        try
        //        {
        //            objDOGEN_GPSServiceRequestParameter.LoggedInUserId = 1;//currentUser.ADM_UserMasterId;
        //            objGPSServiceGetMethods.GetMemberEligibilityService(objDOGEN_GPSServiceRequestParameter, out lstDOGEN_GPSData, out errorMessage);
        //            if (!errorMessage.IsNullOrEmpty())
        //            {
        //                BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
        //            }
        //            return lstDOGEN_GPSData;

        //        }
        //        catch (Exception ex)
        //        {
        //            BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
        //            throw ex;
        //        }

        //    }
        //    public HttpResponseMessage GetGPSHouseholdID(string AccountID)
        //    {
        //        HttpResponseMessage response = new HttpResponseMessage();
        //        try
        //        {
        //            MacroGetEffectiveDate objMacroGetEffectiveDate = new MacroGetEffectiveDate();
        //            List<MacroGetEffectiveDate> lstDOGEN_GPSData = new List<MacroGetEffectiveDate>();
        //            string errorMessage = string.Empty;  
        //            lstDOGEN_GPSData = PEligibilitySearch(AccountID, out errorMessage).ToList();
        //            objMacroGetEffectiveDate = lstDOGEN_GPSData.FirstOrDefault();
        //            if (errorMessage == "")
        //            {
        //                var responseData = new { IsSuccess = true, data = objMacroGetEffectiveDate };
        //                //success with data
        //                response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //                return response;
        //            }
        //            else
        //            {
        //                var responseData = new { IsSuccess = false, data = errorMessage };
        //                //failure with data
        //                response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //                return response;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            var responseData = new { IsSuccess = false, data = ex.Message };
        //            //failure with data
        //            BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
        //            response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //            return response;
        //        }

        //    }

        //    public HttpResponseMessage GetTRRData(string IndividualID)
        //    {
        //        HttpResponseMessage response = new HttpResponseMessage();
        //        try
        //        {
        //            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
        //            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
        //            string errorMessage = string.Empty;
        //            objDOGEN_Queue = PGetTRRData(IndividualID, out errorMessage);

        //            var TrrSummary = objDOGEN_Queue.lstTrrSummaryTransaction;
        //            var BadHistory = objDOGEN_Queue.lstBadHistoryTransaction;
        //            var BadPending = objDOGEN_Queue.lstBadPendingTransaction;
        //            if (errorMessage == "")
        //            {
        //                var responseData = new { IsSuccess = true, data = new { TrrSummaryTransaction = TrrSummary, BadHistoryTransaction = BadHistory, BadPendingTransaction= BadPending } };              
        //                //success with data
        //                response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //                return response;
        //            }
        //            else
        //            {
        //                var responseData = new { IsSuccess = false, data = errorMessage };
        //                //failure with data
        //                response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //                return response;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            var responseData = new { IsSuccess = false, data = ex.Message };
        //            //failure with data
        //            BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
        //            response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //            return response;
        //        }

        //    }
        //    [NonAction]
        //    public DOGEN_Queue PGetTRRData(string IndividualID,out string errorMessage)
        //    {           
        //        errorMessage = string.Empty;          

        //        GPSServiceGetMethod objGPSServiceGetMethods = new GPSServiceGetMethod();
        //        DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
        //        DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
        //        DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
        //        try
        //        { 

        //                objDOGEN_GPSServiceRequestParameter.IndividualId = IndividualID;
        //                //web service call for TRR data
        //                objGPSServiceGetMethods.GetTRRSummaryInfoService(objDOGEN_GPSServiceRequestParameter, ref objDOGEN_Queue, out errorMessage);
        //                if (!errorMessage.IsNullOrEmpty())
        //                {
        //                    BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
        //                }
        //        }
        //        catch (Exception ex)
        //        {

        //            BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());

        //        }

        //        return objDOGEN_Queue;
        //    }

        //    [Route("api/GetGpsMemberData/PostCMSTransaction")]
        //    [HttpPost]
        //    public IHttpActionResult PostCMSTransaction(JObject jsonResult)
        //    {
        //        HttpResponseMessage response = new HttpResponseMessage();
        //        try
        //        {
        //            CMSTransaction item = JsonConvert.DeserializeObject<CMSTransaction>(jsonResult.ToString());

        //            string errorMessage = string.Empty;
        //            //  dynamic jsonData = objData;
        //            string BirthDate = item.BirthDate;
        //            string CaseNumber = item.CaseNumber;
        //            string ContractNumber = item.ContractNumber;
        //            string EffectiveStartDate = item.EffectiveStartDate;
        //            string MedicareClaimNumber = item.MedicareClaimNumber;
        //            string LastName = item.LastName;
        //            string PbpNo = item.PbpNo;
        //            DOCMSPostTransaction(BirthDate, CaseNumber, ContractNumber, EffectiveStartDate, MedicareClaimNumber, LastName, PbpNo, out errorMessage);
        //            if (errorMessage == "")
        //            {
        //                var responseData = new { IsSuccess = true, data = errorMessage };
        //                //success with data
        //                return Content(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

        //            }
        //            else
        //            {
        //                var responseData = new { IsSuccess = false, data = errorMessage };
        //                //failure with data
        //                return Content(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            var responseData = new { IsSuccess = false, data = ex.Message };
        //            //failure with data
        //            BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
        //            return Content(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

        //        }         

        //        }
        //    [NonAction]
        //    private void DOCMSPostTransaction(string BirthDate, string CaseNumber, string ContractNumber, string EffectiveStartDate, string MedicareClaimNumber, string LastName, string PbpNo, out string errorMessage)
        //    {
        //        GPSServiceGetMethod objGPSServiceGetMethods = new GPSServiceGetMethod();
        //        DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
        //        errorMessage = string.Empty;
        //        DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();           
        //        try
        //        {
        //            //requestDetails.applicationDate = objDOGEN_GPSServiceRequestParameter.ApplicationDate;
        //            objDOGEN_GPSServiceRequestParameter.BirthDate = BirthDate;
        //            objDOGEN_GPSServiceRequestParameter.CaseNumber = CaseNumber;
        //            objDOGEN_GPSServiceRequestParameter.ContractNumber = ContractNumber;
        //            //requestDetails.effectiveEndDate = objDOGEN_GPSServiceRequestParameter.EffectiveEndDate;
        //            objDOGEN_GPSServiceRequestParameter.EffectiveStartDate = EffectiveStartDate;
        //            //requestDetails.electionType = objDOGEN_GPSServiceRequestParameter.ElectionType;
        //            objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber = MedicareClaimNumber;
        //            objDOGEN_GPSServiceRequestParameter.LastName = LastName;
        //            objDOGEN_GPSServiceRequestParameter.PbpNo = PbpNo;
        //            objDOGEN_GPSServiceRequestParameter.TransactionCode = ((long)CMSTransactionCode.TRR76).ToString();
        //            objGPSServiceGetMethods.CreateCMSTransactionService(objDOGEN_GPSServiceRequestParameter, out errorMessage);
        //        }
        //        catch (Exception ex)
        //        {
        //            BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
        //            throw ex;
        //        }
        //    }

        //    [Route("api/GetGpsMemberData/PostOutOfAreaService")]
        //    [HttpPost]
        //    public IHttpActionResult PostOutOfAreaService(JObject jsonResult)
        //    {
        //        //HttpResponseMessage response = new HttpResponseMessage();
        //        try
        //        {
        //            OutOfAreaAervice item = JsonConvert.DeserializeObject<OutOfAreaAervice>(jsonResult.ToString());
        //            string errorMessage = string.Empty;              
        //            string CaseNumber = item.CaseNumber;
        //            string HouseholdId = item.HouseholdId;
        //            string DisenrollmentDate = item.DisenrollmentDate;
        //            DOOutOfAreaServiceService(CaseNumber, HouseholdId, DisenrollmentDate, out errorMessage);
        //            if (errorMessage == "")
        //            {
        //                var responseData = new { IsSuccess = true, data = errorMessage };
        //                //success with data
        //                return Content(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

        //            }
        //            else
        //            {
        //                var responseData = new { IsSuccess = false, data = errorMessage };
        //                //failure with data
        //                return Content(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            var responseData = new { IsSuccess = false, data = ex.Message };
        //            //failure with data
        //            BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
        //            return Content(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);

        //        }

        //    }

        //    [NonAction]
        //    private void DOOutOfAreaServiceService(string caseNumber, string householdId, string DisenrollmentDate, out string errorMessage)
        //    {
        //        GPSServiceGetMethod objGPSServiceGetMethods = new GPSServiceGetMethod();
        //        DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
        //        errorMessage = string.Empty;
        //        DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
        //        try
        //        {
        //            //requestDetails.applicationDate = objDOGEN_GPSServiceRequestParameter.ApplicationDate;              
        //            objDOGEN_GPSServiceRequestParameter.CaseNumber = caseNumber;
        //            objDOGEN_GPSServiceRequestParameter.HouseholdId = householdId;
        //            objDOGEN_GPSServiceRequestParameter.OutOfAreaDisenrollmentDate = DisenrollmentDate;
        //            objGPSServiceGetMethods.MaintainOutOfAreaServiceService(objDOGEN_GPSServiceRequestParameter, out errorMessage);
        //        }
        //        catch (Exception ex)
        //        {
        //            BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
        //            throw ex;
        //        }
        //    }

        //    public HttpResponseMessage GetTRRDetails(string TransactionID)
        //    {
        //        HttpResponseMessage response = new HttpResponseMessage();
        //        try
        //        {
        //            DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems = new DOMQTRRWorkQueueItems();
        //            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();               
        //            string errorMessage = string.Empty;
        //            string a  = PGetTRRDetails(TransactionID, out errorMessage);

        //            //var TrrSummary = objDOGEN_Queue.lstTrrSummaryTransaction;
        //            //var BadHistory = objDOGEN_Queue.lstBadHistoryTransaction;
        //            //var BadPending = objDOGEN_Queue.lstBadPendingTransaction;
        //            if (errorMessage == "")
        //            {
        //                var responseData = new { IsSuccess = true, data =  a };
        //                //success with data
        //                response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //                return response;
        //            }
        //            else
        //            {
        //                var responseData = new { IsSuccess = false, data = errorMessage };
        //                //failure with data
        //                response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //                return response;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            var responseData = new { IsSuccess = false, data = ex.Message };
        //            //failure with data
        //            BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GetGPSServiceData, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
        //            response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
        //            return response;
        //        }

        //    }
        //    [NonAction]
        //    public string PGetTRRDetails(string TransactionID, out string errorMessage)
        //    {
        //        errorMessage = string.Empty;
        //        DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems = new DOMQTRRWorkQueueItems();
        //        GPSServiceGetMethod objGPSServiceGetMethods = new GPSServiceGetMethod();           
        //        DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
        //        string responc = string.Empty;
        //        try
        //        {               
        //            //web service call for TRR data
        //            objGPSServiceGetMethods.GetTRRDetails(TransactionID, ref responc, out errorMessage);
        //            if (!errorMessage.IsNullOrEmpty())
        //            {
        //                BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());

        //        }

        //        return responc;
        //    }


        //}

        //public class CMSTransaction
        //{
        //    public string BirthDate { get; set; }
        //    public string CaseNumber { get; set; }
        //    public string ContractNumber { get; set; }
        //    public string EffectiveStartDate { get; set; }
        //    public string MedicareClaimNumber { get; set; }
        //    public string LastName { get; set; }
        //    public string PbpNo { get; set; }
        //}
        //public class OutOfAreaAervice
        //{       
        //    public string CaseNumber { get; set; }
        //    public string HouseholdId { get; set; }
        //    public string DisenrollmentDate { get; set; }       

        //}
    }
}
