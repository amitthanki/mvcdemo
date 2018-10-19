using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ENRLReconSystem
{
    
    public class USPSService
    {
        //Base URL for USPS Address and Zip Code validation API.
        private string _baseURL = string.Empty;
        //User ID obtained from USPS.
        public string USPS_UserID = string.Empty;
        private WebClient wsClient;
        public USPSService(string New_UserID)
        {
            USPS_UserID = New_UserID;
        }


        public string AddressValidateRequest(string Address1, string Address2, string City, string State, string Zip5, string Zip4)
        {
            try
            {
                _baseURL = "http://production.shippingapis.com/ShippingAPI.dll?API=Verify";
                //http://production.shippingapis.com/ShippingAPI.dll?API=Verify
                //&XML=<AddressValidateRequest USERID="641UNITE1062">
                //<Address>
                //<Address1></Address1> 
                //<Address2>6406 Ivy Lane</Address2> 
                //<City>Greenbelt</City> 
                //<State>MD</State> 
                //<Zip5></Zip5> 
                //<Zip4></Zip4> 
                //</Address> 
                //</AddressValidateRequest>
                string strResponse = "", strUSPS = "";
                strUSPS = _baseURL + "&XML=<AddressValidateRequest USERID=\"" + USPS_UserID + "\">";
                strUSPS += "<Address ID=\"0\">";
                strUSPS += "<Address1>" + Address1 + "</Address1>";
                strUSPS += "<Address2>" + Address2 + "</Address2>";
                strUSPS += "<City>" + City + "</City>";
                strUSPS += "<State>" + State + "</State>";
                strUSPS += "<Zip5>" + Zip5 + "</Zip5>";
                strUSPS += "<Zip4>" + Zip4 + "</Zip4>";
                strUSPS += "</Address></AddressValidateRequest>";
                //Send the request to USPS.
                strResponse = GetDataFromSite(strUSPS);
                return strResponse;

            }
            catch (Exception)
            {
                throw;
                //need to handle
            }
        }
        public string CityStateLookupRequest(string ZipCode)
        {
            try
            {
                // http://production.shippingapis.com/ShippingAPI.dll?API=CityStateLookup
                //&XML=<CityStateLookupRequest%20USERID="641UNITE1062">
                //<ZipCode ID= "0"> 
                //<Zip5>90210</Zip5> 
                //</ZipCode> 
                //</CityStateLookupRequest> 

                _baseURL = "http://production.shippingapis.com/ShippingAPI.dll?API=CityStateLookup";
                string strResponse = "", strUSPS = "";
                strUSPS = _baseURL + "&XML=<CityStateLookupRequest USERID=\"" + USPS_UserID + "\">";
                strUSPS += "<ZipCode ID=\"0\">";
                strUSPS += "<Zip5>" + ZipCode + "</Zip5>";
                strUSPS += "</ZipCode></CityStateLookupRequest>";
                //Send the request to USPS.
                strResponse = GetDataFromSite(strUSPS);
                return strResponse;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ZipcodeLookupRequest(string Address1, string Address2, string City, string State)
        {
            try
            {
                _baseURL = "http://production.shippingapis.com/ShippingAPI.dll?API=ZipCodeLookup";
                string strResponse = "", strUSPS = "";
                strUSPS = _baseURL + "&XML=<ZipCodeLookupRequest USERID=\"" + USPS_UserID + "\">";
                strUSPS += "<Address>";
                strUSPS += "<Address1>" + Address1 + "</Address1>";
                strUSPS += "<Address2>" + Address2 + "</Address2>";
                strUSPS += "<City>" + City + "</City>";
                strUSPS += "<State>" + State + "</State>";
                strUSPS += "</Address></ZipCodeLookupRequest>";
                //Send the request to USPS.
                strResponse = GetDataFromSite(strUSPS);
                return strResponse;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="USPS_Request"></param>
        /// <returns></returns>
        private string GetDataFromSite(string USPS_Request)
        {
            try
            {
                string strResponse = "";
                wsClient = new WebClient();
                //Send the request to USPS.
                byte[] ResponseData = wsClient.DownloadData(USPS_Request);
                //Convert byte stream to string data.
                foreach (byte oItem in ResponseData)
                {
                    strResponse += (char)oItem;
                }
                return strResponse;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}