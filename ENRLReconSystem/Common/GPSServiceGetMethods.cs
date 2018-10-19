using System;
using System.Collections.Generic;
using System.Linq;
using ENRLReconSystem.DO;
using ENRLReconSystem.srvAEMemberDemographics;
using ENRLReconSystem.srvAEMemberEligibility;
using ENRLReconSystem.srvAETrrDetails;
using ENRLReconSystem.srvAETrrSummary;
using ENRLReconSystem.Utility;
using ENRLReconSystem.BL;
using System.Reflection;
using System.Net;
using ENRLReconSystem.srvAEEmployerSummary;

namespace ENRLReconSystem.Common
{
    
    public class GPSServiceGetMethods
    {


        public void GetTRRDetails(string transactionId = "")
        {
            srvAETrrDetails.SearchTrrdetailClient client = new srvAETrrDetails.SearchTrrdetailClient();
            //Header
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAETrrDetails.searchTrrDetailRequest request = new srvAETrrDetails.searchTrrDetailRequest();
            srvAETrrDetails.searchTrrDetailRequestSearch requestParameter = new srvAETrrDetails.searchTrrDetailRequestSearch();
            srvAETrrDetails.searchTrrDetailResponse response = new srvAETrrDetails.searchTrrDetailResponse();
            srvAETrrDetails.controlModifiersType credentials = new srvAETrrDetails.controlModifiersType();
            srvAETrrDetails.gpsSystemParametersType sysParameter = new srvAETrrDetails.gpsSystemParametersType();
            srvAETrrDetails.requestHeader reqHeader = new srvAETrrDetails.requestHeader();
            requestParameter.transactionId = transactionId;
            ////As Per UPM
            sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
            sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
            reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
            reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
            //reqHeader.logLevel = srvAETrrDetails.logLevel.DEBUG;
            //reqHeader.externalId = "AELLOT-kZVPi";
            credentials.gpsSystemParameters = sysParameter;
            requestParameter.controlModifier = credentials;
            request.requestHeader = reqHeader;
            request.search = requestParameter;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Open();
            try
            {
                string exception = string.Empty;
                response = client.invokeService(request);
                DOGEN_TRRData objDOGEN_TRRData = new DOGEN_TRRData();
                var responseOutput = response.trrDetail;
                if (!responseOutput.IsNull())
                {
                    TrrDetailsMapping(responseOutput, out objDOGEN_TRRData);
                }
                else
                {
                    exception = response.responseHeader.statusMessages[0].statusMessage1;
                }




            }
            catch (System.ServiceModel.FaultException ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
            }
            finally
            {
                client.Close();
            }

        }

        private void TrrDetailsMapping(trrDetail responseOutput, out DOGEN_TRRData objDOGEN_TRRData)
        {
            objDOGEN_TRRData = new DOGEN_TRRData();
            if (responseOutput.enrollment != null)
            {
                objDOGEN_TRRData.ApplicationDate = Convert.ToDateTime(responseOutput.transaction.applicationDate);
                objDOGEN_TRRData.ChangeInitiatorIndicator = responseOutput.transaction.changeInitiatorInd.NullToString();
                objDOGEN_TRRData.ChangeInitiatorIndicator = responseOutput.transaction.changeInitionOrgName;
                objDOGEN_TRRData.ContractNumber = responseOutput.transaction.plan.contractNo;
                //objDOGEN_TRRData.ContractYear=
                objDOGEN_TRRData.CoPayCategory = responseOutput.transaction.planCoverageInfo.coPay.ToString();
                //objDOGEN_TRRData.CoPayEffectiveDate=responseOutput.transaction.planCoverageInfo
                objDOGEN_TRRData.CreditableCoverageIndicator = responseOutput.transaction.planCoverageInfo.creditableCoverageInd;
                objDOGEN_TRRData.DistrictOffice = responseOutput.transaction.districtOffice;
                objDOGEN_TRRData.EffectiveDate = Convert.ToDateTime(responseOutput.transaction.effectiveDate);
                objDOGEN_TRRData.EGHBIndicator = responseOutput.transaction.planCoverageInfo.eghbInd;
                objDOGEN_TRRData.EmpSubsidyOverrideIndicator = responseOutput.transaction.planCoverageInfo.employerSubsidyOverrideInd;
                objDOGEN_TRRData.GPSTrackingNumber = responseOutput.transaction.gpsTrackingNumber;
                objDOGEN_TRRData.MedicareClaimNumber = responseOutput.transaction.medicareClaimNumber;
                objDOGEN_TRRData.NotCoveredMonths = responseOutput.transaction.planCoverageInfo.notCoveredMonths;
                objDOGEN_TRRData.PartCPremium = responseOutput.transaction.plan.partCBeneficiaryPremium;
                objDOGEN_TRRData.PartDPremium = responseOutput.transaction.plan.partDBeneficiaryPremium;
                objDOGEN_TRRData.PBPNumber = responseOutput.transaction.plan.pbpNo;
                //objDOGEN_TRRData.PlanId=responseOutput.transaction.plan.
                //objDOGEN_TRRData.PremiumAmountValue=responseOutput
                objDOGEN_TRRData.PremiumWitholdOption = responseOutput.transaction.planCoverageInfo.premiumWitholdOption;
                objDOGEN_TRRData.PreviousContractNumber = responseOutput.transaction.plan.previousContractPDP;
                objDOGEN_TRRData.PreviousPBP = responseOutput.transaction.plan.previousPBP;
                objDOGEN_TRRData.ProcessDate = Convert.ToDateTime(responseOutput.transaction.processDate);
                objDOGEN_TRRData.ProcessingTime = responseOutput.transaction.processingTime;
                //objDOGEN_TRRData.ProductCode =responseOutput.
                objDOGEN_TRRData.RecordType = responseOutput.transaction.recordType;
                objDOGEN_TRRData.RXBin = responseOutput.transaction.partDRx.rxBin;
                objDOGEN_TRRData.RXGroup = responseOutput.transaction.partDRx.rxGroup;
                objDOGEN_TRRData.RXId = responseOutput.transaction.partDRx.rxId;
                objDOGEN_TRRData.RXPCN = responseOutput.transaction.partDRx.rxPcn;
                //objDOGEN_TRRData.SNPFlag=responseOutput.transaction
                objDOGEN_TRRData.SourceIndicator = responseOutput.transaction.sourceIdentifier;
                objDOGEN_TRRData.SystemtrackingId = responseOutput.transaction.systemTrackingId;
                objDOGEN_TRRData.TransactionCode = responseOutput.transaction.transactionCode.ToString();
                objDOGEN_TRRData.TransactionCodeDescription = responseOutput.transaction.transactionCode.description;
                objDOGEN_TRRData.TransactionDate = Convert.ToDateTime(responseOutput.transaction.transactionDate);
                objDOGEN_TRRData.TransactionId = responseOutput.transaction.transactionId;
                objDOGEN_TRRData.TransactionReplyCode = responseOutput.transaction.transactionCode.replyCode;
                objDOGEN_TRRData.TransactionShortName = responseOutput.transaction.trcShortName;


            }
        }


        public void GetMemberEligibilityService(DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter, out List<DOGEN_GPSData> lstDOGEN_GPSData, out string errorMessage)
        {

            errorMessage = string.Empty;
            string responseData1 = string.Empty;
            string inputData = string.Empty;
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            objDOGEN_AEGPSServiceTrace.CreatedByRef = objDOGEN_GPSServiceRequestParameter.LoggedInUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetMemberEligibilityService;
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = objDOGEN_GPSServiceRequestParameter.ERSCaseId.ToInt64();
            lstDOGEN_GPSData = new List<DOGEN_GPSData>();


            srvAEMemberEligibility.ReadOvationsMemberEligibilityClient client = new srvAEMemberEligibility.ReadOvationsMemberEligibilityClient();
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAEMemberEligibility.readOvationsMemberEligibilityRequest request = new srvAEMemberEligibility.readOvationsMemberEligibilityRequest();
            srvAEMemberEligibility.readOvationsMemberEligibilityResponse response = new srvAEMemberEligibility.readOvationsMemberEligibilityResponse();
            srvAEMemberEligibility.employerIdSearchCriteriaType empSerachDetails = new srvAEMemberEligibility.employerIdSearchCriteriaType();
            srvAEMemberEligibility.accountIdSearchCriteriaType accSearchType = new srvAEMemberEligibility.accountIdSearchCriteriaType();
            srvAEMemberEligibility.requestHeader reqHeader = new srvAEMemberEligibility.requestHeader();
            srvAEMemberEligibility.gpsSystemParametersType sysParameter = new srvAEMemberEligibility.gpsSystemParametersType();
            srvAEMemberEligibility.controlModifiersType controlModifiers = new srvAEMemberEligibility.controlModifiersType();

            if (!objDOGEN_GPSServiceRequestParameter.HouseholdId.IsNullOrEmpty())
            {
                accSearchType.accountId = objDOGEN_GPSServiceRequestParameter.HouseholdId.NullToString();
                request.searchType = "ACCOUNT";
                request.householdSearchCriteria = accSearchType;
            }

            sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
            sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
            reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
            reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
            controlModifiers.gpsSystemParameters = sysParameter;
            controlModifiers.getPlanProfileIndicator = true;
            controlModifiers.includeApplicationData = true;
            controlModifiers.includeApplicationDataSpecified = true;
            controlModifiers.includePlanData = true;
            controlModifiers.includePlanDataSpecified = true;
            request.controlModifiers = controlModifiers;
            request.requestHeader = reqHeader;

            inputData = "GPSHouseHoldId:||" + objDOGEN_GPSServiceRequestParameter.HouseholdId.NullToString();
            objDOGEN_AEGPSServiceTrace.RequestData = inputData;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                client.Open();
                response = client.invokeService(request);
                if (response != null && !response.account.IsNull())
                {
                    var eligibiltyDetails = response.account;
                    objDOGEN_AEGPSServiceTrace.ResponseData = eligibiltyDetails.ObjToJsonString();
                    if (!eligibiltyDetails.IsNull())
                    {
                        objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                        MappingELigibilityInformation(eligibiltyDetails, out lstDOGEN_GPSData);
                    }
                    else
                    {
                        errorMessage = response.responseHeader.statusMessages[0].statusMessage1.NullToString();
                        objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;

                    }
                }
                else
                {
                    errorMessage = response.responseHeader.statusMessages[0].statusMessage1;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                    objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                }
            }
            catch (Exception ex)
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

        public void GetTRRSummaryInfoService(DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter, ref DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            string responseData1 = string.Empty;
            string inputData = string.Empty;
            errorMessage = string.Empty;
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            objDOGEN_AEGPSServiceTrace.CreatedByRef = objDOGEN_GPSServiceRequestParameter.LoggedInUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetTRRSummaryInfoService;
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = objDOGEN_GPSServiceRequestParameter.ERSCaseId.ToInt64();

            srvAETrrSummary.SearchTrrSummaryClient client = new srvAETrrSummary.SearchTrrSummaryClient();
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAETrrSummary.searchTrrSummaryRequest request = new srvAETrrSummary.searchTrrSummaryRequest();
            srvAETrrSummary.searchTrrSummaryResponse response = new srvAETrrSummary.searchTrrSummaryResponse();
            srvAETrrSummary.searchTrrSummaryRequestSearch searchRequest = new srvAETrrSummary.searchTrrSummaryRequestSearch();
            srvAETrrSummary.requestHeader reqHeader = new srvAETrrSummary.requestHeader();
            srvAETrrSummary.gpsSystemParametersType sysParameter = new srvAETrrSummary.gpsSystemParametersType();
            srvAETrrSummary.controlModifiersType credentials = new srvAETrrSummary.controlModifiersType();
            searchRequest.individualId = objDOGEN_GPSServiceRequestParameter.IndividualId;
            inputData = "individualId:" + searchRequest.individualId;
            objDOGEN_AEGPSServiceTrace.RequestData = inputData;
            //As Per UPM
            sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
            sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
            reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
            reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
            //reqHeader.logLevel = srvAETrrDetails.logLevel.DEBUG;
            //reqHeader.externalId = "AELLOT-kZVPi";
            credentials.gpsSystemParameters = sysParameter;
            searchRequest.controlModifier = credentials;
            request.search = searchRequest;
            request.requestHeader = reqHeader;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Open();
            List<DOGEN_TRRData> objsummaryTransaction = new List<DOGEN_TRRData>();
            List<DOGEN_TRRData> objbadHistoryTransaction = new List<DOGEN_TRRData>();
            List<DOGEN_TRRData> objbadPendingTransaction = new List<DOGEN_TRRData>();
            try
            {

                response = client.invokeService(request);
                if (response != null)
                {
                    var responseData = response.trrsummary;
                    responseData1 = "response:||" + response.responseHeader.serviceName;
                    objDOGEN_AEGPSServiceTrace.ResponseData = responseData1;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;

                    if (!responseData.summaryTransaction.IsNull())
                    {
                        MappingTRRSummary(responseData.summaryTransaction, out objsummaryTransaction);
                    }
                    if (!responseData.badHistoryTransaction.IsNull())
                    {
                        MapBadHistoryTransaction(responseData.badHistoryTransaction, out objbadHistoryTransaction);
                    }
                    if (!responseData.badPendingTransaction.IsNull())
                    {
                        MapBadPendingTransaction(responseData.badPendingTransaction, out objbadPendingTransaction);
                    }
                    objDOGEN_Queue.lstTrrSummaryTransaction = objsummaryTransaction;
                    objDOGEN_Queue.lstBadHistoryTransaction = objbadHistoryTransaction;
                    objDOGEN_Queue.lstBadPendingTransaction = objbadPendingTransaction;
                }
                else
                {
                    responseData1 = "response:||" + response.responseHeader.serviceName + "||reponseHeader:||" + response.responseHeader.statusMessages[0].statusMessage1.NullToString();
                    errorMessage = response.responseHeader.statusMessages[0].statusMessage1.NullToString();
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                }

            }
            catch (System.ServiceModel.FaultException ex)
            {
                errorMessage = ex.Message;
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                BLCommon.LogError(objDOGEN_Queue.LoginUserId.ToInt64(), MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.ToString(), ex.ToString());

                //BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.Application, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
            }
            finally
            {
                objBLServiceRequestResponse.InsertAEGPSServiceTrace(objDOGEN_AEGPSServiceTrace);
                client.Close();
            }
        }


        private void MapBadHistoryTransaction(srvAETrrSummary.transactionType[] badHistoryTransaction, out List<DOGEN_TRRData> lstDOGEN_TRRData)
        {
            lstDOGEN_TRRData = new List<DOGEN_TRRData>();

            try
            {

                lstDOGEN_TRRData = badHistoryTransaction.AsEnumerable().Select(item => new DOGEN_TRRData
                {

                    MedicareClaimNumber = item.medicareClaimNumber.NullToString(),
                    EffectiveDate = (item.effectiveDate.IsNull()) ? (DateTime?)null : item.effectiveDate.ToDateTime(),
                    ContractNumber = item.contract.IsNull() ? string.Empty : item.contract.contractNumber.NullToString(),
                    PBPNumber = item.contract.IsNull() ? string.Empty : item.contract.planBenefitPackage.NullToString(),
                    TransactionCode = item.transactionCode.IsNull() ? string.Empty : item.transactionCode.code.NullToString(),
                    TransactionCodeDescription = item.transactionCode.IsNull() ? string.Empty : item.transactionCode.description.NullToString(),
                    TransactionReplyCode = item.transactionCode.IsNull() ? string.Empty : item.transactionCode.replyCode.NullToString(),
                    TransactionDate = (item.transactionDate.IsNull()) ? (DateTime?)null : item.transactionDate.ToDateTime(),
                    TransactionId = item.transactionId.NullToString(),
                    RXBin = item.partDRx.IsNull() ? string.Empty : item.partDRx.rxBin.NullToString(),
                    RXGroup = item.partDRx.IsNull() ? string.Empty : item.partDRx.rxGroup.NullToString(),
                    RXId = item.partDRx.IsNull() ? string.Empty : item.partDRx.rxId.NullToString(),
                    RXPCN = item.partDRx.IsNull() ? string.Empty : item.partDRx.rxPcn.NullToString()
                }).ToList();


            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
            }
        }

        private void MapBadPendingTransaction(srvAETrrSummary.transactionType[] badHistoryTransaction, out List<DOGEN_TRRData> lstDOGEN_TRRData)
        {
            lstDOGEN_TRRData = new List<DOGEN_TRRData>();
            try
            {
                lstDOGEN_TRRData = badHistoryTransaction.AsEnumerable().Select(item => new DOGEN_TRRData
                {
                    MedicareClaimNumber = item.medicareClaimNumber.NullToString(),
                    EffectiveDate = (item.effectiveDate.IsNull()) ? (DateTime?)null : item.effectiveDate.ToDateTime(),
                    ContractNumber = item.contract.IsNull() ? string.Empty : item.contract.contractNumber.NullToString(),
                    PBPNumber = item.contract.IsNull() ? string.Empty : item.contract.planBenefitPackage.NullToString(),
                    TransactionCode = item.transactionCode.IsNull() ? string.Empty : item.transactionCode.code.NullToString(),
                    TransactionCodeDescription = item.transactionCode.IsNull() ? string.Empty : item.transactionCode.description.NullToString(),
                    TransactionReplyCode = item.transactionCode.IsNull() ? string.Empty : item.transactionCode.replyCode.NullToString(),
                    TransactionDate = (item.transactionDate.IsNull()) ? (DateTime?)null : item.transactionDate.ToDateTime(),
                    TransactionId = item.transactionId.NullToString()
                }).ToList();

            }
            catch (Exception ex)
            {

                BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
            }
        }

        private void MappingTRRSummary(transactionSummaryType[] summaryTransaction, out List<DOGEN_TRRData> lstDOGEN_TRRData)
        {
            lstDOGEN_TRRData = new List<DOGEN_TRRData>();
            try
            {
                lstDOGEN_TRRData = summaryTransaction.AsEnumerable().Select(item => new DOGEN_TRRData
                {
                    MedicareClaimNumber = item.medicareClaimNumber.NullToString(),
                    EffectiveDate = (item.effectiveDate.IsNull()) ? (DateTime?)null : item.effectiveDate.ToDateTime(),
                    ContractNumber = item.contract.IsNull() ? string.Empty : item.contract.contractNumber.NullToString(),
                    PBPNumber = item.contract.IsNull() ? string.Empty : item.contract.planBenefitPackage.NullToString(),
                    ProcessDate = (item.processDate.IsNull()) ? (DateTime?)null : item.processDate.ToDateTime(),
                    TransactionCode = item.transactionCode.IsNull() ? string.Empty : item.transactionCode.code.NullToString(),
                    TransactionCodeDescription = item.transactionCode.IsNull() ? string.Empty : item.transactionCode.description.NullToString(),
                    TransactionReplyCode = item.transactionCode.IsNull() ? string.Empty : item.transactionCode.replyCode.NullToString(),
                    TransactionDate = (item.transactionDate.IsNull()) ? (DateTime?)null : item.transactionDate.ToDateTime(),
                    TransactionId = item.transactionId.NullToString()
                }).ToList();

            }
            catch (Exception ex)
            {

                BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
            }
        }


        public void GetMemberDemographicalDetails(DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter, out DOGEN_GPSData objDOGEN_GPSData, out string errorMessage)
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


            srvAEMemberDemographics.RetrieveDemographicsClient client = new srvAEMemberDemographics.RetrieveDemographicsClient();
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAEMemberDemographics.retrieveDemographicsRequest request = new srvAEMemberDemographics.retrieveDemographicsRequest();
            srvAEMemberDemographics.retrieveDemographicsResponse response = new srvAEMemberDemographics.retrieveDemographicsResponse();
            srvAEMemberDemographics.shipInformationType shipInformation = new srvAEMemberDemographics.shipInformationType();
            srvAEMemberDemographics.requestHeader reqHeader = new srvAEMemberDemographics.requestHeader();
            srvAEMemberDemographics.gpsSystemParametersType sysParameter = new srvAEMemberDemographics.gpsSystemParametersType();
            srvAEMemberDemographics.wesbSystemParametersType webSysParameters = new srvAEMemberDemographics.wesbSystemParametersType();
            srvAEMemberDemographics.controlModifiersType credentials = new srvAEMemberDemographics.controlModifiersType();
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
            objDOGEN_GPSData = new DOGEN_GPSData();
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
                        MappingDemographicDetails(responseData, out objDOGEN_GPSData);
                    }
                    else
                    {
                        errorMessage = response.responseHeader.statusMessages[0].statusMessage1.NullToString();
                        objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                        objDOGEN_AEGPSServiceTrace.ResponseData = responseData.ObjToJsonString(); ;
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


            srvAEMemberDemographics.RetrieveDemographicsClient client = new srvAEMemberDemographics.RetrieveDemographicsClient();
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAEMemberDemographics.retrieveDemographicsRequest request = new srvAEMemberDemographics.retrieveDemographicsRequest();
            srvAEMemberDemographics.retrieveDemographicsResponse response = new srvAEMemberDemographics.retrieveDemographicsResponse();
            srvAEMemberDemographics.shipInformationType shipInformation = new srvAEMemberDemographics.shipInformationType();
            srvAEMemberDemographics.requestHeader reqHeader = new srvAEMemberDemographics.requestHeader();
            srvAEMemberDemographics.gpsSystemParametersType sysParameter = new srvAEMemberDemographics.gpsSystemParametersType();
            srvAEMemberDemographics.wesbSystemParametersType webSysParameters = new srvAEMemberDemographics.wesbSystemParametersType();
            srvAEMemberDemographics.controlModifiersType credentials = new srvAEMemberDemographics.controlModifiersType();
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
                        objDOGEN_AEGPSServiceTrace.ResponseData = responseData.ObjToJsonString(); ;
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

        private void MappingDemographicDetails(demographics[] responseData, out DOGEN_GPSData objDOGEN_GPSData)
        {
            objDOGEN_GPSData = new DOGEN_GPSData();
            try
            {
                for (int i = 0; i < responseData.Length; i++)
                {
                    objDOGEN_GPSData.AddressLine1 = responseData[i].preference.address.postalAddress.addressLine1.NullToString();
                    objDOGEN_GPSData.AddressLine2 = responseData[i].preference.address.postalAddress.addressLine2.NullToString();
                    //objDOGEN_GPSData.ApplicationApprovedStatus =responseData
                    objDOGEN_GPSData.City = responseData[i].preference.address.postalAddress.city.NullToString();
                    //objDOGEN_GPSData.ContractNumber= responseData.businessType.
                    objDOGEN_GPSData.County = responseData[i].preference.address.countyName.NullToString();
                    objDOGEN_GPSData.State = responseData[i].preference.address.postalAddress.state.NullToString();
                    objDOGEN_GPSData.ZipCode = responseData[i].preference.address.postalAddress.zip5.NullToString();
                    objDOGEN_GPSData.ZipCode4 = responseData[i].preference.address.postalAddress.zip4.NullToString();
                    objDOGEN_GPSData.DOB = responseData[i].person.dateOfBirth.IsNull() ? (DateTime?)null : responseData[i].person.dateOfBirth.ToDateTime();
                    objDOGEN_GPSData.FirstName = responseData[i].person.personName.firstName.NullToString();
                    objDOGEN_GPSData.Gender = responseData[i].person.gender.NullToString();
                    objDOGEN_GPSData.LastName = responseData[i].person.personName.lastName.NullToString();
                    objDOGEN_GPSData.MiddleName = responseData[i].person.personName.middleName.NullToString();
                    //objDOGEN_GPSData.GPSOOADisenrollmentDate= responseData.
                    objDOGEN_GPSData.HICN = responseData[i].person.medicareClaimNumber.NullToString();
                    objDOGEN_GPSData.MemberId = responseData[i].memberNumber.NullToString();
                    objDOGEN_GPSData.HouseholdId = responseData[i].accountId.NullToString();
                    objDOGEN_GPSData.IndividualId = responseData[i].individualId.ToInt64();
                    objDOGEN_GPSData.LOB = responseData[i].lob.NullToString();
                }

            }
            catch (Exception ex)
            {

                BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
            }



        }
        /// <summary>
        /// mapping data for GPS HouseholdID
        /// </summary>
        /// <param name="eligibiltyDetails"></param>
        /// <param name="lstDOGEN_GPSData"></param>
        private void MappingELigibilityInformation(accountType[] eligibiltyDetails, out List<DOGEN_GPSData> lstDOGEN_GPSData)
        {
            lstDOGEN_GPSData = new List<DOGEN_GPSData>();
            DOGEN_GPSData doGEN_GPSData;
            for (int i = 0; i < eligibiltyDetails.Count(); i++)
            {
                doGEN_GPSData = new DOGEN_GPSData();
                var eligDetails = eligibiltyDetails[0].householdIndividual[i];
                doGEN_GPSData.HouseholdId = eligibiltyDetails[0].accountId.NullToString();
                doGEN_GPSData.IndividualId = eligDetails.individualId.IsNullOrEmpty() ? (long?)null : eligDetails.individualId.ToInt64();
                doGEN_GPSData.DOB = eligDetails.dateOfBirth.IsNull() ? (DateTime?)null : Convert.ToDateTime(eligDetails.dateOfBirth);
                doGEN_GPSData.HICN = eligDetails.medicareClaimNumber.NullToString();
                doGEN_GPSData.Gender = eligDetails.gender.NullToString();
                doGEN_GPSData.OOAIndicator = eligDetails.outOfAreaIndicator ? "YES" : "NO";
                doGEN_GPSData.GPSOOADisenrollmentDate = eligDetails.outOfAreaDisenrollmentDate.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(eligDetails.outOfAreaDisenrollmentDate);

                if (!eligDetails.name.IsNull())
                {
                    doGEN_GPSData.FirstName = eligDetails.name.firstName.NullToString();
                    doGEN_GPSData.MiddleName = eligDetails.name.middleName.NullToString();
                    doGEN_GPSData.LastName = eligDetails.name.lastName.NullToString();
                }

                if (!eligDetails.memberProfile.IsNull() && eligDetails.memberProfile.Length > 0)
                {
                    doGEN_GPSData.MemberId = eligDetails.memberProfile[0].memberNumber.NullToString();
                }

                if (!eligDetails.insuredPlan.IsNull() && eligDetails.insuredPlan.Length > 0)
                {
                    insuredPlanType currentPLan = eligDetails.insuredPlan.OrderByDescending(x => x.effectiveStartDate.ToDateTime()).FirstOrDefault();
                    if (!currentPLan.IsNull())
                    {
                        doGEN_GPSData.ContractNumber = currentPLan.contractNumber.NullToString();
                        doGEN_GPSData.PBP = currentPLan.planBenefitNumber.NullToString();
                        doGEN_GPSData.PlanEffectiveDate = currentPLan.effectiveStartDate.IsNull() ? (DateTime?)null : Convert.ToDateTime(currentPLan.effectiveStartDate);
                        doGEN_GPSData.PlanTermDate = currentPLan.effectiveStopDate.IsNull() ? (DateTime?)null : Convert.ToDateTime(currentPLan.effectiveStopDate);
                        doGEN_GPSData.LOB = currentPLan.businessSegment.NullToString();

                        if (!currentPLan.enrollmentSourceCode.IsNullOrEmpty())
                        {
                            char enrollmentSourceCode = currentPLan.enrollmentSourceCode.ToCharArray()[0];
                            if (enrollmentSourceCode == 'A' || enrollmentSourceCode == 'C' || enrollmentSourceCode == 'H')
                                doGEN_GPSData.PDPAutoEntrolleeIndicator = 1;
                            else
                                doGEN_GPSData.PDPAutoEntrolleeIndicator = 0;
                        }

                        if (!eligDetails.application.IsNull() && eligDetails.application.Length > 0)
                        {
                            applicationType application = eligDetails.application.Where(x => x.applicationId == currentPLan.applicationId).FirstOrDefault();
                            if (!application.IsNull())
                                doGEN_GPSData.ApplicationApprovedStatus = application.status.NullToString();
                        }
                    }
                }

                if (!eligDetails.individualAddress.IsNull() && eligDetails.individualAddress.Length > 0)
                {
                    ovationsAddressType address = eligDetails.individualAddress.Where(x => x.addressType.Contains("PERMANENT")).FirstOrDefault();
                    if (!address.address.IsNull())
                    {
                        doGEN_GPSData.AddressLine1 = address.address.street1.NullToString();
                        doGEN_GPSData.AddressLine2 = address.address.street2.NullToString();
                        doGEN_GPSData.City = address.address.city.NullToString();
                        doGEN_GPSData.State = address.address.state.NullToString();
                        doGEN_GPSData.ZipCode4 = address.address.zip4.NullToString();
                        doGEN_GPSData.ZipCode = address.address.zip.NullToString();
                        doGEN_GPSData.County = address.address.county.NullToString();
                    }
                    doGEN_GPSData.SCCCode = address.cmsStateCode.NullToString() + address.cmsCountyCode.NullToString();
                    doGEN_GPSData.SCCEffectiveDate = address.startDate.IsNull() ? (DateTime?)null : Convert.ToDateTime(address.startDate);
                    doGEN_GPSData.SCCEndDate = address.stopDate.IsNull() ? (DateTime?)null : Convert.ToDateTime(address.stopDate);
                }

                lstDOGEN_GPSData.Add(doGEN_GPSData);
            }
        }

        public void CreateCMSTransactionService(DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter, out string errorMessage)
        {
            string responseData = string.Empty;
            string inputData = string.Empty;
            errorMessage = string.Empty;
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            objDOGEN_AEGPSServiceTrace.CreatedByRef = objDOGEN_GPSServiceRequestParameter.LoggedInUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.CreateCMSTransactionService;
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = objDOGEN_GPSServiceRequestParameter.CaseNumber.IsNull() ? 0 : objDOGEN_GPSServiceRequestParameter.CaseNumber.ToInt64();


            srvAECMSTransaction.PostCmstransactionClient client = new srvAECMSTransaction.PostCmstransactionClient();
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAECMSTransaction.postCmstransactionRequest request = new srvAECMSTransaction.postCmstransactionRequest();
            srvAECMSTransaction.postCmstransactionResponse response = new srvAECMSTransaction.postCmstransactionResponse();
            srvAECMSTransaction.requestHeader reqHeader = new srvAECMSTransaction.requestHeader();
            srvAECMSTransaction.gpsSystemParametersType sysParameter = new srvAECMSTransaction.gpsSystemParametersType();
            srvAECMSTransaction.controlModifiersType credentials = new srvAECMSTransaction.controlModifiersType();
            srvAECMSTransaction.cmstransaction requestDetails = new srvAECMSTransaction.cmstransaction();

            //requestDetails.applicationDate = objDOGEN_GPSServiceRequestParameter.ApplicationDate;
            requestDetails.birthDate = objDOGEN_GPSServiceRequestParameter.BirthDate;
            requestDetails.caseNumber = objDOGEN_GPSServiceRequestParameter.CaseNumber;
            requestDetails.contractNumber = objDOGEN_GPSServiceRequestParameter.ContractNumber;
            if (objDOGEN_GPSServiceRequestParameter.ActionLkup == (long)PWActionsEnum.SendSCCDeletiontoCMS)
            {
                requestDetails.effectiveEndDate = objDOGEN_GPSServiceRequestParameter.EffectiveEndDate;
            }
            requestDetails.effectiveStartDate = objDOGEN_GPSServiceRequestParameter.EffectiveStartDate;
            //requestDetails.electionType = objDOGEN_GPSServiceRequestParameter.ElectionType;
            requestDetails.medicareClaimNumber = objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber;
            requestDetails.lastName = objDOGEN_GPSServiceRequestParameter.LastName;
            requestDetails.pbpNo = objDOGEN_GPSServiceRequestParameter.PbpNo;
            requestDetails.transactionCode = objDOGEN_GPSServiceRequestParameter.TransactionCode;
            inputData = "birthDate:||" + requestDetails.birthDate + "||caseNumber:||" + requestDetails.caseNumber + "||contractNumber:||" + requestDetails.contractNumber + "||effectiveStartDate:||" + requestDetails.effectiveStartDate + "||medicareClaimNumber:||" + requestDetails.medicareClaimNumber + "||lastName:||" + requestDetails.lastName + "||pbpNo:||" + requestDetails.pbpNo + "||transactionCode:||" + requestDetails.transactionCode;
            objDOGEN_AEGPSServiceTrace.RequestData = inputData;
            //As Per UPM
            sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
            sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
            reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
            reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
            //reqHeader.logLevel = srvAETrrDetails.logLevel.DEBUG;
            //reqHeader.externalId = "AELLOT-kZVPi";
            credentials.gpsSystemParameters = sysParameter;
            request.controlModifiers = credentials;
            request.requestHeader = reqHeader;
            request.cmstransaction = requestDetails;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Open();
            try
            {
                response = client.invokeService(request);
                if (!response.result.IsNull())
                {

                    responseData = "caseNumber:||" + response.caseNumber + "||medicareClaimNumber:||" + response.medicareClaimNumber + "||result:||" + response.result + "||reponseHeader:||" + "Accept";
                    objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                }
                else
                {
                    responseData = "caseNumber:||" + response.caseNumber + "||medicareClaimNumber:||" + response.medicareClaimNumber + "||result:||" + response.result + "||reponseHeader:||" + response.responseHeader.statusMessages[0].statusMessage1.NullToString();
                    errorMessage = response.responseHeader.statusMessages[0].statusMessage1.NullToString();
                    objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
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

        public void MaintainOutOfAreaServiceService(DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter, out string errorMessage)
        {
            errorMessage = string.Empty;
            string responseData = string.Empty;
            string inputData = string.Empty;
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            objDOGEN_AEGPSServiceTrace.CreatedByRef = objDOGEN_GPSServiceRequestParameter.LoggedInUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.MaintainOutOfAreaServiceService;
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = objDOGEN_GPSServiceRequestParameter.CaseNumber.IsNull() ? 0 : objDOGEN_GPSServiceRequestParameter.CaseNumber.ToInt64();

            srvAEOutOfAreaOption.PutOutofareaoptionClient client = new srvAEOutOfAreaOption.PutOutofareaoptionClient();
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAEOutOfAreaOption.putOutOfAreaOptionRequest request = new srvAEOutOfAreaOption.putOutOfAreaOptionRequest();
            srvAEOutOfAreaOption.putOutOfAreaOptionResponse response = new srvAEOutOfAreaOption.putOutOfAreaOptionResponse();
            srvAEOutOfAreaOption.requestHeader reqHeader = new srvAEOutOfAreaOption.requestHeader();

            srvAEOutOfAreaOption.gpsSystemParametersType sysParameter = new srvAEOutOfAreaOption.gpsSystemParametersType();
            srvAEOutOfAreaOption.controlModifiersType credentials = new srvAEOutOfAreaOption.controlModifiersType();
            srvAEOutOfAreaOption.putOutOfAreaOptionRequestOutOfAreaOptionRequest reqParameter = new srvAEOutOfAreaOption.putOutOfAreaOptionRequestOutOfAreaOptionRequest();
            reqParameter.caseNumber = objDOGEN_GPSServiceRequestParameter.CaseNumber;
            reqParameter.householdId = objDOGEN_GPSServiceRequestParameter.HouseholdId;
            reqParameter.outOfAreaDisenrollmentDate = objDOGEN_GPSServiceRequestParameter.OutOfAreaDisenrollmentDate;
            reqParameter.sendFulfillmentInd = "Y";
            reqParameter.outOfAreaInd = "Y";
            request.outOfAreaOptionRequest = reqParameter;
            inputData = "caseNumber:||" + reqParameter.caseNumber + "||householdId:||" + reqParameter.householdId + "||outOfAreaDisenrollmentDate:||" + reqParameter.outOfAreaDisenrollmentDate + "||sendFulfillmentInd:||" + reqParameter.sendFulfillmentInd + "||outOfAreaInd:||" + reqParameter.outOfAreaInd;
            objDOGEN_AEGPSServiceTrace.RequestData = inputData;

            //As Per UPM
            sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
            sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
            reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
            reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
            //reqHeader.logLevel = srvAETrrDetails.logLevel.DEBUG;
            //reqHeader.externalId = "AELLOT-kZVPi";
            credentials.gpsSystemParameters = sysParameter;
            request.outOfAreaOptionRequest.controlModifiers = credentials;
            request.requestHeader = reqHeader;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Open();
            try
            {

                response = client.invokeService(request);
                if (!response.outOfAreaOption.IsNull() && response.outOfAreaOption.Length > 0 && response.outOfAreaOption[0].description == "SUCCESS")
                {

                    responseData = "outOfAreaOption:||" + response.outOfAreaOption + "||Description:||" + response.outOfAreaOption[0].description;
                    objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                }
                else
                {
                    string statusMessage = (!response.outOfAreaOption.IsNull() && response.outOfAreaOption.Length > 0) ? response.outOfAreaOption[0].description : null;
                    statusMessage = statusMessage == null && (response.responseHeader != null && response.responseHeader.statusMessages != null && response.responseHeader.statusMessages.Length > 0) ? response.responseHeader.statusMessages[0].statusMessage1 : statusMessage;
                    responseData = "outOfAreaOption:||" + response.outOfAreaOption + "||reponseHeader:||" + statusMessage;
                    errorMessage = statusMessage != null ? statusMessage : "An error occured during web service call";
                    objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
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

        public ExceptionTypes GetEmployerSummary(DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter, out DOGEN_GPSData objDOGEN_GPSData, out string errorMessage)
        {
            ExceptionTypes result;
            errorMessage = string.Empty;
            string responseData1 = string.Empty;
            string inputData = string.Empty;
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            objDOGEN_AEGPSServiceTrace.CreatedByRef = objDOGEN_GPSServiceRequestParameter.LoggedInUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetEmployerSummary;
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = objDOGEN_GPSServiceRequestParameter.CaseNumber.IsNull() ? 0 : objDOGEN_GPSServiceRequestParameter.CaseNumber.ToInt64();
            objDOGEN_GPSData = new DOGEN_GPSData();


            srvAEEmployerSummary.ReadEmployerClient client = new srvAEEmployerSummary.ReadEmployerClient();
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAEEmployerSummary.readEmployerRequest request = new srvAEEmployerSummary.readEmployerRequest();
            srvAEEmployerSummary.readEmployerResponse response = new srvAEEmployerSummary.readEmployerResponse();
            srvAEEmployerSummary.requestHeader reqHeader = new srvAEEmployerSummary.requestHeader();
            srvAEEmployerSummary.gpsSystemParametersType sysParameter = new srvAEEmployerSummary.gpsSystemParametersType();
            srvAEEmployerSummary.readInputMetaType readInputMeta = new srvAEEmployerSummary.readInputMetaType();
            srvAEEmployerSummary.readEmployerRequestReadInput readInput = new srvAEEmployerSummary.readEmployerRequestReadInput();

            if (!objDOGEN_GPSServiceRequestParameter.HouseholdId.IsNullOrEmpty())
            {
                readInput.householdId = objDOGEN_GPSServiceRequestParameter.HouseholdId.NullToString();
            }

            sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
            sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
            reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
            reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
            readInputMeta.gpsSystemParameters = sysParameter;
            readInput.readInputMeta = readInputMeta;
            request.readInput = readInput;
            request.requestHeader = reqHeader;

            inputData = "GPSHouseHoldId:||" + objDOGEN_GPSServiceRequestParameter.HouseholdId.NullToString();
            objDOGEN_AEGPSServiceTrace.RequestData = inputData;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                client.Open();
                response = client.invokeService(request);
                objDOGEN_AEGPSServiceTrace.ResponseData = response.ObjToJsonString();
                if (response != null && !response.readOutput.IsNull() && !response.readOutput.employer.IsNull() && !response.readOutput.employer.employerSummary.IsNull())
                {
                    employerSummaryType employerSummary = response.readOutput.employer.employerSummary;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                    MappingEmployerInformation(employerSummary, out objDOGEN_GPSData);
                    result = ExceptionTypes.Success;
                }
                else
                {
                    errorMessage = response.responseHeader.statusMessages[0].statusMessage1;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                    result = ExceptionTypes.ZeroRecords;
                }
            }
            catch (System.ServiceModel.FaultException ex)
            {
                errorMessage = ex.Message;
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                BLCommon.LogError(objDOGEN_GPSServiceRequestParameter.LoggedInUserId, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.ToString(), ex.ToString());
                result = ExceptionTypes.Exception;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                BLCommon.LogError(objDOGEN_GPSServiceRequestParameter.LoggedInUserId, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.ToString(), ex.ToString());
                result = ExceptionTypes.Exception;
            }
            finally
            {
                objBLServiceRequestResponse.InsertAEGPSServiceTrace(objDOGEN_AEGPSServiceTrace);
                client.Close();
            }
            return result;
        }

        private void MappingEmployerInformation(employerSummaryType employerSummary, out DOGEN_GPSData objDOGEN_GPSData)
        {
            objDOGEN_GPSData = new DOGEN_GPSData();
            try
            {
                objDOGEN_GPSData.EmployerEffectiveDate = employerSummary.employerEffectiveDate.IsNullOrEmpty() ? (DateTime?)null : DateTime.Parse(employerSummary.employerEffectiveDate);
                objDOGEN_GPSData.EmployerCloseDate = employerSummary.employerCloseDate.IsNullOrEmpty() ? (DateTime?)null : DateTime.Parse(employerSummary.employerCloseDate);
                objDOGEN_GPSData.EmployerId = employerSummary.employerId;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
            }
        }
    }
}