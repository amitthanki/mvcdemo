using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using ERSBackgroundProcess.srvAERetrieveDemographics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERSBackgroundProcess
{
    public class MaskPhiData
    {
        long _lCurrentMasterUserId;
        public void MaskPHIData()
        {
            string notprocessed = "";
            try
            {
                List<DOGEN_Queue> lstGenQueues;
                BLCommon objBLCommon = new BLCommon();
                ExceptionTypes result = objBLCommon.GetCasesToMask(out lstGenQueues);
                if (result == ExceptionTypes.Success && lstGenQueues.Count > 0)
                {
                    //Parallel.ForEach(lstGenQueues, item =>
                    //{

                    //});
                    Console.WriteLine("Total Records : " + lstGenQueues.Count);
                    foreach (var item in lstGenQueues)
                    {
                        if (item.GEN_QueueId.HasValue && item.GEN_QueueId.Value != 0 && !item.MemberCurrentHICN.IsNullOrEmpty())
                        {
                            Console.WriteLine("Processing : Gen_QueueId - " + item.GEN_QueueId + ", HICN - " + item.MemberCurrentHICN);
                            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
                            GetMemberDemographicalDetails(new DOGEN_GPSServiceRequestParameter { MedicareClaimNumber = item.MemberCurrentHICN }, out List<DOGEN_GPSData> lstDOGEN_GPSData, out string errorMessage);

                            if (lstDOGEN_GPSData.Count > 0)
                            {
                                if (lstDOGEN_GPSData.Count > 1 && item.GPSHouseholdID != null)
                                {
                                    objDOGEN_GPSData = lstDOGEN_GPSData.FirstOrDefault(x => x.HouseholdId == item.GPSHouseholdID);
                                }
                                else
                                {
                                    objDOGEN_GPSData = lstDOGEN_GPSData.FirstOrDefault();
                                }

                                if (objDOGEN_GPSData != null)
                                {
                                    result = objBLCommon.MaskPHIData(item.GEN_QueueId.Value, objDOGEN_GPSData);
                                    Console.WriteLine("Gen_QueueId - " + item.GEN_QueueId.Value + " Result : Success");
                                }

                            }
                            else
                            {
                                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Not Masked GenQueueId : " + item.GEN_QueueId, "Logging not masked GenqueueId");
                            }
                        }
                        else
                        {
                            notprocessed = notprocessed + "," + item.GEN_QueueId;
                        }
                    }
                    //});

                }
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Not Masked GenQueueIds : " + notprocessed, "Logging not masked GenqueueIds");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "", ex.ToString());
            }
        }

        public void GetMemberDemographicalDetails(DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter, out List<DOGEN_GPSData> lstDOGEN_GPSData, out string errorMessage)
        {
            errorMessage = string.Empty;
            string responseData1 = string.Empty;
            string inputData = string.Empty;
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            objDOGEN_AEGPSServiceTrace.CreatedByRef = objDOGEN_GPSServiceRequestParameter.LoggedInUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetMemberDemographicalDetails;
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = objDOGEN_GPSServiceRequestParameter.ERSCaseId.ToInt64();


            srvAERetrieveDemographics.RetrieveDemographicsClient client = new srvAERetrieveDemographics.RetrieveDemographicsClient();
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAERetrieveDemographics.retrieveDemographicsRequest request = new srvAERetrieveDemographics.retrieveDemographicsRequest();
            srvAERetrieveDemographics.retrieveDemographicsResponse response = new srvAERetrieveDemographics.retrieveDemographicsResponse();
            srvAERetrieveDemographics.shipInformationType shipInformation = new srvAERetrieveDemographics.shipInformationType();
            srvAERetrieveDemographics.requestHeader reqHeader = new srvAERetrieveDemographics.requestHeader();
            srvAERetrieveDemographics.gpsSystemParametersType sysParameter = new srvAERetrieveDemographics.gpsSystemParametersType();
            srvAERetrieveDemographics.wesbSystemParametersType webSysParameters = new srvAERetrieveDemographics.wesbSystemParametersType();
            srvAERetrieveDemographics.controlModifiersType credentials = new srvAERetrieveDemographics.controlModifiersType();
            request.medicareClaimNumber = objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber;

            //As Per UPM
            sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
            sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
            reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
            reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
            //reqHeader.logLevel = srvAETrrDetails.logLevel.DEBUG;
            //reqHeader.externalId = "AELLOT-kZVPi";
            webSysParameters.environment = "test_sys1";
            webSysParameters.fromAddress = "https://portal.uhc.com/";
            webSysParameters.userName = "ocpuser";
            credentials.wesbSystemParameters = webSysParameters;
            credentials.gpsSystemParameters = sysParameter;
            request.businessType = "GOVT";
            request.shipInformation = shipInformation;
            request.controlModifiers = credentials;
            request.requestHeader = reqHeader;
            inputData = "medicareClaimNumber:||" + request.medicareClaimNumber;
            objDOGEN_AEGPSServiceTrace.RequestData = inputData;
            lstDOGEN_GPSData = new List<DOGEN_GPSData>();
            errorMessage = string.Empty;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Open();
            try
            {

                response = client.invokeService(request);
                if (response != null && !response.demographics.IsNull())
                {

                    var responseData = response.demographics;
                    if (!responseData.IsNull())
                    {
                        objDOGEN_AEGPSServiceTrace.ResponseData = responseData.ObjToJsonString();
                        objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                        MappingDemographicDetails(responseData, out lstDOGEN_GPSData);
                    }
                    else
                    {
                        errorMessage = response.responseHeader.statusMessages[0].statusMessage1.NullToString();
                        objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                        objDOGEN_AEGPSServiceTrace.ResponseData = responseData.ObjToJsonString();
                    }
                }
                else
                {
                    errorMessage = response.responseHeader.statusMessages[0].statusMessage1;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                    objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                }

            }
            catch (System.ServiceModel.FaultException ex)
            {
                errorMessage = ex.Message;
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                BLCommon.LogError(objDOGEN_GPSServiceRequestParameter.LoggedInUserId, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.ToString(), ex.ToString());
            }
            finally
            {
                objBLServiceRequestResponse.InsertAEGPSServiceTrace(objDOGEN_AEGPSServiceTrace);
                client.Close();
            }
        }

        private void MappingDemographicDetails(demographics[] responseData, out List<DOGEN_GPSData> lstDOGEN_GPSData)
        {
            lstDOGEN_GPSData = new List<DOGEN_GPSData>();
            try
            {

                for (int i = 0; i < responseData.Length; i++)
                {
                    DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
                    objDOGEN_GPSData.AddressLine1 = responseData[i].preference.address.postalAddress.addressLine1.NullToString();
                    objDOGEN_GPSData.AddressLine2 = responseData[i].preference.address.postalAddress.addressLine2.NullToString();
                    //objDOGEN_GPSData.ApplicationApprovedStatus = responseData
                    objDOGEN_GPSData.City = responseData[i].preference.address.postalAddress.city.NullToString();
                    //objDOGEN_GPSDatractNumber= responseData.businessType.
                    objDOGEN_GPSData.County = responseData[i].preference.address.countyName.NullToString();
                    objDOGEN_GPSData.State = responseData[i].preference.address.postalAddress.state.NullToString();
                    objDOGEN_GPSData.ZipCode = responseData[i].preference.address.postalAddress.zip5.NullToString();
                    objDOGEN_GPSData.ZipCode4 = responseData[i].preference.address.postalAddress.zip4.NullToString();
                    objDOGEN_GPSData.DOB = responseData[i].person.dateOfBirth.IsNull() ? (DateTime?)null : responseData[i].person.dateOfBirth.ToDateTime();
                    objDOGEN_GPSData.FirstName = responseData[i].person.personName.firstName.NullToString();
                    objDOGEN_GPSData.Gender = responseData[i].person.gender.NullToString();
                    objDOGEN_GPSData.LastName = responseData[i].person.personName.lastName.NullToString();
                    objDOGEN_GPSData.MiddleName = responseData[i].person.personName.middleName.NullToString();
                    //objDOGEN_GPSDaOOADisenrollmentDate= responseData.
                    objDOGEN_GPSData.HICN = responseData[i].person.medicareClaimNumber.NullToString();
                    objDOGEN_GPSData.MemberId = responseData[i].memberNumber.NullToString();
                    objDOGEN_GPSData.HouseholdId = responseData[i].accountId.NullToString();
                    objDOGEN_GPSData.IndividualId = responseData[i].individualId.ToInt64();
                    objDOGEN_GPSData.LOB = responseData[i].lob.NullToString();

                    lstDOGEN_GPSData.Add(objDOGEN_GPSData);
                }

            }
            catch (Exception ex)
            {

                BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
            }



        }

        public void updateMBIforHICN()
        {
            Console.WriteLine("Starting  : Update MBI Process");
            _lCurrentMasterUserId = 1;
            try
            {
                List<DOGEN_Queue> lstGenQueues;
                BLCommon objBLCommon = new BLCommon();
                ExceptionTypes result = objBLCommon.GetCasesToMask(out lstGenQueues);
                if (result == ExceptionTypes.Success && lstGenQueues.Count > 0)
                {
                    //Parallel.ForEach(lstGenQueues, item =>
                    //{

                    //});
                    Console.WriteLine("Total Records : " + lstGenQueues.Count);
                    foreach (var item in lstGenQueues)
                    {
                        if (item.GEN_QueueId.HasValue && item.GEN_QueueId.Value != 0 && !item.MemberCurrentHICN.IsNullOrEmpty())
                        {
                            Console.WriteLine("Processing : Gen_QueueId - " + item.GEN_QueueId + ", Household Id - " + item.GPSHouseholdID);
                            GetMemberEligibility(item.GPSHouseholdID, out string MBI);

                            if (!string.IsNullOrEmpty(MBI))
                            {
                                result = objBLCommon.UpdateMBIValue(item.GEN_QueueId.Value, MBI);
                                Console.WriteLine("Gen_QueueId - " + item.GEN_QueueId.Value + " Result : Success");
                            }
                        }
                    }
                    //});

                }
                else
                {
                    Console.WriteLine("Error : ");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
        }

        public ExceptionTypes GetMemberEligibility(string strAccountId, out string MBI)
        {
            ExceptionTypes result = ExceptionTypes.UnknownError;
            MBI = string.Empty;
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            srvAEMemberEligibility.ReadOvationsMemberEligibilityClient client = new srvAEMemberEligibility.ReadOvationsMemberEligibilityClient();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetMemberEligibilityService;
            objDOGEN_AEGPSServiceTrace.CreatedByRef = _lCurrentMasterUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = 0;
            objDOGEN_AEGPSServiceTrace.RequestData = "Account Id:||" + strAccountId;
            try
            {
                //Header
                client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
                srvAEMemberEligibility.readOvationsMemberEligibilityRequest request = new srvAEMemberEligibility.readOvationsMemberEligibilityRequest();
                srvAEMemberEligibility.readOvationsMemberEligibilityResponse response = new srvAEMemberEligibility.readOvationsMemberEligibilityResponse();
                srvAEMemberEligibility.employerIdSearchCriteriaType empSerachDetails = new srvAEMemberEligibility.employerIdSearchCriteriaType();
                srvAEMemberEligibility.accountIdSearchCriteriaType accSearchType = new srvAEMemberEligibility.accountIdSearchCriteriaType();
                srvAEMemberEligibility.requestHeader reqHeader = new srvAEMemberEligibility.requestHeader();
                srvAEMemberEligibility.gpsSystemParametersType sysParameter = new srvAEMemberEligibility.gpsSystemParametersType();
                srvAEMemberEligibility.controlModifiersType controlModifiers = new srvAEMemberEligibility.controlModifiersType();

                accSearchType.accountId = strAccountId;
                request.searchType = "ACCOUNT";
                request.householdSearchCriteria = accSearchType;

                sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
                sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
                reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
                reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
                controlModifiers.gpsSystemParameters = sysParameter;
                controlModifiers.getPlanProfileIndicator = true;
                controlModifiers.includeApplicationData = true;
                controlModifiers.includeApplicationDataSpecified = true;
                request.controlModifiers = controlModifiers;
                request.requestHeader = reqHeader;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                client.Open();

                response = client.invokeService(request);
                if (response != null)
                {

                    var responseOutput = response.account;
                    if (!responseOutput[0].IsNull())
                    {
                        var household = responseOutput[0].householdIndividual[0];
                        objDOGEN_AEGPSServiceTrace.ResponseData = response.ObjToJsonString();
                        objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;

                        MBI = household.medicareClaimNumber.NullToString();

                        result = ExceptionTypes.Success;
                    }
                    else
                    {
                        objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                        objDOGEN_AEGPSServiceTrace.ResponseData = response.ObjToJsonString();
                        result = ExceptionTypes.ZeroRecords;
                    }
                }
                else
                {
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                    objDOGEN_AEGPSServiceTrace.ResponseData = response.responseHeader.statusMessages[0].statusMessage1; ;
                    result = ExceptionTypes.RemoteCallException;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                objDOGEN_AEGPSServiceTrace.ResponseData = ex.Message;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Exception, ex.ToString(), ex.ToString());
                result = ExceptionTypes.Exception;
            }
            finally
            {
                objBLServiceRequestResponse.InsertAEGPSServiceTrace(objDOGEN_AEGPSServiceTrace);
                client.Close();
            }
            return result;
        }

    }
}
