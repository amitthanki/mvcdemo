using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Converters;
using System.Globalization;
using ENRLReconSystem.WebAPI.Models;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using ENRLReconSystem.DO.DataObjects;
using ENRLReconSystem.Utility;
using ENRLReconSystem.BL;
using System.Data;
using ENRLReconSystem.DO;
using System.Reflection;


namespace ENRLReconSystem.WebAPI.Controllers
{
    //[Authorize]
    //[ERSMacroAuthencation(Roles = "MacroRole")]
    [RoutePrefix("api/Macro")]   
    public class MacroController : ApiController
    {      
        

        public static string PATH_AUTHORIZATIONSCHEMA = System.Web.Hosting.HostingEnvironment.MapPath("~/JsonSchema/PostAPIJson-schema.json");
        public static string PATH_AUTHORIZATIONSCHEMA1 = System.Web.Hosting.HostingEnvironment.MapPath("~/JsonSchema/json-schemaChildGenQueue.json");
        long _userid = 6;
        /// <summary>
        /// GetCasesForNOTMacro
        /// </summary>
        /// <param name="HouseholdID">HouseholdID</param>
        /// <param name="HICN">HICN</param>
        /// <param name="PlanContract">PlanContract</param>
        /// <param name="PlanPBP">PlanPBP</param>
        /// <param name="EffectiveDate">EffectiveDate</param>
        /// <param name="CaseType">CaseType</param>
        /// <returns></returns>

        [Route("GetCasesForNOTMacro/{HouseholdID?}/{HICN?}/{PlanContract?}/{PlanPBP?}/{EffectiveDate?}/{CaseType?}")]       
        [HttpGet]
        public HttpResponseMessage GetCasesForNOTMacro(string HouseholdID = null, string HICN = null, string PlanContract = null, string PlanPBP = null, string EffectiveDate = null, string CaseType = null)
        {
            DOGEN_MacroServiceTrace objDOGEN_MacroServiceTrace = new DOGEN_MacroServiceTrace();
            HttpContextBase abstractContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            var url = abstractContext.Request.Url;
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            BLMacro objBLMacro = new BLMacro();
            try
            {               
                 List<DOMacroData> lstDOMacroNotOpenData = new List<DOMacroData>();
                string errorMessage = string.Empty;
                lstDOMacroNotOpenData = objBLMacro.GetOpenNotMacro(HouseholdID, HICN, PlanContract, PlanPBP, EffectiveDate, CaseType,out errorMessage);
                if (lstDOMacroNotOpenData.Count > 0)
                {
                    var responseData = new { IsSuccess = true, Message = "", data = lstDOMacroNotOpenData };
                    objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                    //success with data
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);
                    var output = JsonConvert.SerializeObject(lstDOMacroNotOpenData);
                    objDOGEN_MacroServiceTrace.ResponseData = output.ToString();
                }
                else
                {
                    objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                    var responseData = new { IsSuccess = false, Message = "No records found for provided input", data = new List<string>() };                 
                    //success with no data
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);
                    objDOGEN_MacroServiceTrace.ResponseData = "No records found for provided input";
                }                        
            }
            catch (Exception ex)
            {
                var responseData = new { IsSuccess = false, Message = ex.Message, data = new List<string>() };
                BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Macro, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                responseMessage = Request.CreateResponse(HttpStatusCode.ExpectationFailed, responseData);
            }
            objDOGEN_MacroServiceTrace.MacroServiceMethodLkup = (long)MacroType.NOTMacro;
            objDOGEN_MacroServiceTrace.MacroServiceMethodName = MethodBase.GetCurrentMethod().Name;
            objDOGEN_MacroServiceTrace.RequestData = url.ToString();
            TraceWebApi(objDOGEN_MacroServiceTrace);
            return responseMessage;
        }

        /// <summary>
        /// GetCasesForTRC155Macro
        /// </summary>
        /// <param name="HouseholdID"></param>
        /// <param name="HICN"></param>
        /// <param name="PlanContract"></param>
        /// <param name="PlanPBP"></param>
        /// <param name="EffectiveDate"></param>
        /// <param name="CaseType"></param>
        /// <returns></returns>
        [Route("GetCasesForTRC155Macro/{HouseholdID?}/{HICN?}/{PlanContract?}/{PlanPBP?}/{EffectiveDate?}/{CaseType?}")]
        [HttpGet]
        public HttpResponseMessage GetCasesForTRC155Macro(string HouseholdID = null, string HICN = null, string PlanContract = null, string PlanPBP = null, string EffectiveDate = null, string CaseType = null)
        {
            DOGEN_MacroServiceTrace objDOGEN_MacroServiceTrace = new DOGEN_MacroServiceTrace();
            HttpContextBase abstractContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            var url = abstractContext.Request.Url;
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            BLMacro objBLMacro = new BLMacro();
            try
            {
                string errorMessage = string.Empty;
                List<DOMacroData> lstDOMacroTRC155Data = new List<DOMacroData>();
                lstDOMacroTRC155Data = objBLMacro.GetCasesForTRC155Macro(HouseholdID, HICN, PlanContract, PlanPBP, EffectiveDate, CaseType,out errorMessage);
                if (lstDOMacroTRC155Data.Count > 0)
                {
                    objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                    var responseData = new { IsSuccess = true, Message = "", data = lstDOMacroTRC155Data };
                    //success with data
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);
                    var output = JsonConvert.SerializeObject(lstDOMacroTRC155Data);
                    objDOGEN_MacroServiceTrace.ResponseData = output.ToString();
                }
                else
                {
                    objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                    var responseData = new { IsSuccess = false, Message = "No records found for provided input", data = new List<string>() };
                     //success with no data
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);
                    objDOGEN_MacroServiceTrace.ResponseData = "No records found for provided input";
                }               
            }
            catch (Exception ex)
            {
                var responseData = new { IsSuccess = false, Message = ex.Message, data = new List<string>() };
                BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Macro, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                responseMessage = Request.CreateResponse(HttpStatusCode.ExpectationFailed, responseData, Configuration.Formatters.JsonFormatter);
            }
            objDOGEN_MacroServiceTrace.MacroServiceMethodLkup = (long)MacroType.TRC155Macro;
            objDOGEN_MacroServiceTrace.MacroServiceMethodName = MethodBase.GetCurrentMethod().Name;
            objDOGEN_MacroServiceTrace.RequestData = url.ToString();
            TraceWebApi(objDOGEN_MacroServiceTrace);
            return responseMessage;
        }

        /// <summary>
        /// GetCasesForFTTMacro
        /// </summary>
        /// <param name="HouseholdID"></param>
        /// <param name="HICN"></param>
        /// <param name="PlanContract"></param>
        /// <param name="PlanPBP"></param>
        /// <param name="EffectiveDate"></param>
        /// <param name="CaseType"></param>
        /// <returns></returns>
        [Route("GetCasesForFTTMacro/{HouseholdID?}/{HICN?}/{PlanContract?}/{PlanPBP?}/{EffectiveDate?}/{CaseType?}")]
        [HttpGet]
        public HttpResponseMessage GetCasesForFTTMacro(string HouseholdID = null, string HICN = null, string PlanContract = null, string PlanPBP = null, string EffectiveDate = null, string CaseType = null)
        {
            DOGEN_MacroServiceTrace objDOGEN_MacroServiceTrace = new DOGEN_MacroServiceTrace();
            HttpContextBase abstractContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            var url = abstractContext.Request.Url;
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            BLMacro objBLMacro = new BLMacro();
            try
            {
                string errorMessage = string.Empty;
                List<DOMacroData> lstDOMacroFTTData = new List<DOMacroData>();
                lstDOMacroFTTData = objBLMacro.GetCasesForFTTMacro(HouseholdID, HICN, PlanContract, PlanPBP, EffectiveDate, CaseType,out errorMessage);
                if (lstDOMacroFTTData.Count > 0)
                {
                    objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                    var responseData = new { IsSuccess = true, Message = "", data = lstDOMacroFTTData };
                    //success with data
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);
                    var output = JsonConvert.SerializeObject(lstDOMacroFTTData);
                    objDOGEN_MacroServiceTrace.ResponseData = output.ToString();
                }
                else
                {
                    objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                    var responseData = new { IsSuccess = false, Message = "No records found for provided input", data = new List<string>() };
                    //success with no data
                     responseMessage = Request.CreateResponse(HttpStatusCode.OK, responseData, Configuration.Formatters.JsonFormatter);
                    objDOGEN_MacroServiceTrace.ResponseData = "No records found for provided input";
                }               
            }
            catch (Exception ex)
            {
                var responseData = new { IsSuccess = false, Message = ex.Message, data = new List<string>() };
                BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Macro, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                responseMessage = Request.CreateResponse(HttpStatusCode.ExpectationFailed, responseData, Configuration.Formatters.JsonFormatter);
            }
            objDOGEN_MacroServiceTrace.MacroServiceMethodLkup = (long)MacroType.FTTMacro;
            objDOGEN_MacroServiceTrace.MacroServiceMethodName = MethodBase.GetCurrentMethod().Name;
            objDOGEN_MacroServiceTrace.RequestData = url.ToString();
            TraceWebApi(objDOGEN_MacroServiceTrace);
            return responseMessage;
        }

        /// <summary>
        /// UpdateCaseForNOTMacro
        /// </summary>
        /// <returns></returns>
        [Route("UpdateCaseForNOTMacro")]
        [HttpPost]
        public HttpResponseMessage UpdateCaseForNOTMacro(JObject jObj)
        {
         
            HttpResponseMessage response = new HttpResponseMessage();
            if (jObj != null && jObj.Count > 0)
            {
                DOGEN_MacroServiceTrace objDOGEN_MacroServiceTrace = new DOGEN_MacroServiceTrace();
                DOMacroUpdate item = JsonConvert.DeserializeObject<DOMacroUpdate>(jObj.ToString());
                objDOGEN_MacroServiceTrace.GEN_QueueRef = item.GEN_QueueId;
                long userid = _userid;
                string error = string.Empty;
                try
                {

                    DOMacroUpdate inputModelPostData = ValidateByBaseSchema(jObj.ToString(), PATH_AUTHORIZATIONSCHEMA);
                    if (inputModelPostData != null && inputModelPostData.IsValid)
                    {

                        BLMacro objBLMacro = new BLMacro();
                        long result = objBLMacro.UpdateOpenNotMacro(inputModelPostData, userid, out error);
                        if (result == 0 && string.IsNullOrEmpty(error))
                        {
                            objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                            var responseData = new { IsSuccess = true, Message = "NOT Macro updated for Gen_Queue id -  " + inputModelPostData.GEN_QueueId };
                            objDOGEN_MacroServiceTrace.ResponseData = responseData.ToString();
                            response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                        }
                        else
                        {
                            objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                            var responseData = new { IsSuccess = true, Message = error + "  Gen_Queue id -  " + inputModelPostData.GEN_QueueId };
                            objDOGEN_MacroServiceTrace.ResponseData = responseData.ToString();
                            response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                        }
                    }
                    else
                    {
                        objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                        var responseData = new { IsSuccess = true, Message = inputModelPostData.ValidationMessage };
                        objDOGEN_MacroServiceTrace.ResponseData = responseData.ToString();
                        response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                    }
                    objDOGEN_MacroServiceTrace.MacroServiceMethodLkup = (long)MacroType.NOTMacro;
                    objDOGEN_MacroServiceTrace.MacroServiceMethodName = MethodBase.GetCurrentMethod().Name;
                    objDOGEN_MacroServiceTrace.RequestData = jObj.ToString();                                  
                    TraceWebApi(objDOGEN_MacroServiceTrace);
                }
                catch (Exception ex)
                {
                    BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Macro, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                    response = Request.CreateResponse(HttpStatusCode.NotFound, ex.Message, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                }
            }
            return response;
        }


        public static DOMacroUpdate ValidateByBaseSchema(string InputJsonString, string JsonSchemaPath)
        {
            DOMacroUpdate iModel = null;
            JsonSerializer serializer = new JsonSerializer();
            IList<string> messages;
            try
            {
                using (TextReader Schemareader = File.OpenText(JsonSchemaPath))
                {
                    iModel = new DOMacroUpdate();
                    JObject InputJson = JObject.Parse(InputJsonString);
                    JsonSchema Inputschema = JsonSchema.Read(new JsonTextReader(Schemareader));
                    JsonTextReader reader = new JsonTextReader(new StringReader(InputJsonString));
                    JsonValidatingReader validatingReader = new JsonValidatingReader(reader);
                    validatingReader.Schema = Inputschema;
                    bool IsValid = InputJson.IsValid(Inputschema, out messages);
                    if (IsValid)
                    {
                        iModel = serializer.Deserialize<DOMacroUpdate>(validatingReader);
                        iModel.IsValid = true;
                    }
                    else
                    {
                        iModel.IsValid = false;
                        iModel.ValidationMessage = String.Join(",", messages.ToArray());               
                    }
                }
                return iModel;
            }
            catch (Exception ex)
            {
                iModel.IsValid = false;
                iModel.ValidationMessage = ex.Message;
                return iModel;
            }
        }

        //public static DOMacroUpdate ValidateByChildSchema(string InputJsonString, string JsonSchemaPath)
        //{
        //    DOMacroUpdate iModel = null;
        //    JsonSerializer serializer = new JsonSerializer();
        //    IList<string> messages;
        //    try
        //    {
        //        using (TextReader Schemareader = File.OpenText(JsonSchemaPath))
        //        {
        //            iModel = new DOMacroUpdate();
        //            JObject InputJson = JObject.Parse(InputJsonString);
        //            JsonSchema Inputschema = JsonSchema.Read(new JsonTextReader(Schemareader));
        //            JsonTextReader reader = new JsonTextReader(new StringReader(InputJsonString));
        //            JsonValidatingReader validatingReader = new JsonValidatingReader(reader);
        //            validatingReader.Schema = Inputschema;
        //            bool IsValid = InputJson.IsValid(Inputschema, out messages);
        //            if (IsValid)
        //            {
        //                iModel = serializer.Deserialize<DOMacroUpdate>(validatingReader);
        //                iModel.IsValid = true;
        //            }
        //            else
        //            {              
        //                    iModel.IsValid = false;
        //                    iModel.ValidationMessage = String.Join(",", messages.ToArray());                       
        //            }
        //        }
        //        return iModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        iModel.IsValid = false;
        //        iModel.ValidationMessage = ex.Message;
        //        return iModel;
        //    }
        //}
        /// <summary>
        /// UpdateCaseForFTTMacro
        /// </summary>
        /// <returns></returns>
        [Route("UpdateCaseForFTTMacro")]
        [HttpPost]
        public HttpResponseMessage UpdateCaseForFTTMacro(JObject jObj)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            if (jObj != null && jObj.Count > 0)
            {
                DOGEN_MacroServiceTrace objDOGEN_MacroServiceTrace = new DOGEN_MacroServiceTrace();
                DOMacroUpdate item = JsonConvert.DeserializeObject<DOMacroUpdate>(jObj.ToString());
                objDOGEN_MacroServiceTrace.GEN_QueueRef = item.GEN_QueueId;
                long userid = _userid;
                string error = string.Empty;
                try
                {
                    DOMacroUpdate inputModelPostData = ValidateByBaseSchema(jObj.ToString(), PATH_AUTHORIZATIONSCHEMA);
                    if (inputModelPostData != null && inputModelPostData.IsValid)
                    {

                        BLMacro objBLMacro = new BLMacro();
                        long result = objBLMacro.UpdateFTTMacro(inputModelPostData, userid, out error);
                        if (result == 0 && string.IsNullOrEmpty(error))
                        {
                            objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                            var responseData = new { IsSuccess = true, Message = "FTT Macro updated for Gen_Queue id -  " + inputModelPostData.GEN_QueueId };
                            objDOGEN_MacroServiceTrace.ResponseData = responseData.ToString();
                            response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                        }
                        else
                        {
                            objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                            var responseData = new { IsSuccess = true, Message = error + "  Gen_Queue id -  " + inputModelPostData.GEN_QueueId };
                            objDOGEN_MacroServiceTrace.ResponseData = responseData.ToString();
                            response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                        }
                    }
                    else
                    {
                        objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                        var responseData = new { IsSuccess = true, Message = inputModelPostData.ValidationMessage };
                        objDOGEN_MacroServiceTrace.ResponseData = responseData.ToString();
                        response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                    }
                    objDOGEN_MacroServiceTrace.MacroServiceMethodLkup = (long)MacroType.FTTMacro;
                    objDOGEN_MacroServiceTrace.MacroServiceMethodName = MethodBase.GetCurrentMethod().Name;
                    objDOGEN_MacroServiceTrace.RequestData = jObj.ToString();                  
                    TraceWebApi(objDOGEN_MacroServiceTrace);
                }
                catch (Exception ex)
                {
                    BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Macro, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                    response = Request.CreateResponse(HttpStatusCode.NotFound, ex.Message, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                }
            }
            return response;
        }

        /// <summary>
        /// UpdateCaseForTRC155Macro
        /// </summary>
        /// <param name="jObj"></param>
        /// <returns></returns>
        [Route("UpdateCaseForTRC155Macro")]
        [HttpPost]
        public HttpResponseMessage UpdateCaseForTRC155Macro(JObject jObj)
        {          
                HttpResponseMessage response = new HttpResponseMessage();
            if (jObj != null && jObj.Count>0)
            {
                DOGEN_MacroServiceTrace objDOGEN_MacroServiceTrace = new DOGEN_MacroServiceTrace();
                DOMacroUpdate item = JsonConvert.DeserializeObject<DOMacroUpdate>(jObj.ToString());
                objDOGEN_MacroServiceTrace.GEN_QueueRef = item.GEN_QueueId;
                long userid = _userid;
                string error = string.Empty;
                try
                {
                    DOMacroUpdate inputModelPostData = ValidateByBaseSchema(jObj.ToString(), PATH_AUTHORIZATIONSCHEMA);
                    if (inputModelPostData != null && inputModelPostData.IsValid)
                    {
                        BLMacro objBLMacro = new BLMacro();
                        long result = objBLMacro.UpdateCaseForTRC155Macro(inputModelPostData, userid, out error);
                        if (result == 0 && string.IsNullOrEmpty(error))
                        {
                            objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                            var responseData = new { IsSuccess = true, Message = "TRC155 Macro updated for Gen_Queue id -  " + inputModelPostData.GEN_QueueId };
                            objDOGEN_MacroServiceTrace.ResponseData = responseData.ToString();
                            response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                        }
                        else
                        {
                            objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                            var responseData = new { IsSuccess = true, Message = error + "  Gen_Queue id -  " + inputModelPostData.GEN_QueueId };
                            objDOGEN_MacroServiceTrace.ResponseData = responseData.ToString();
                            response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                        }
                    }
                    else
                    {
                        objDOGEN_MacroServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                        var responseData = new { IsSuccess = true, Message = inputModelPostData.ValidationMessage };
                        objDOGEN_MacroServiceTrace.ResponseData = responseData.ToString();
                        response = Request.CreateResponse(HttpStatusCode.OK, responseData, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                    }
                    objDOGEN_MacroServiceTrace.GEN_QueueRef = inputModelPostData.GEN_QueueId;
                    objDOGEN_MacroServiceTrace.MacroServiceMethodLkup = (long)MacroType.TRC155Macro;
                    objDOGEN_MacroServiceTrace.MacroServiceMethodName = MethodBase.GetCurrentMethod().Name;
                    objDOGEN_MacroServiceTrace.RequestData = jObj.ToString();                  
                    TraceWebApi(objDOGEN_MacroServiceTrace);
                }
                catch (Exception ex)
                {
                    BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Macro, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                    response = Request.CreateResponse(HttpStatusCode.NotFound, ex.Message, (MediaTypeFormatter)Configuration.Formatters.JsonFormatter);
                }
            }
            return response;
        }

        public void TraceWebApi(DOGEN_MacroServiceTrace objDOGEN_MacroServiceTrace)
        {
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            objBLServiceRequestResponse.InsertMacroServiceTrace(objDOGEN_MacroServiceTrace);
        }

    }
}