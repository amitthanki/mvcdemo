using Microsoft.Web.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Web;
using System.Xml;
namespace ENRLReconSystem.WebAPI.Models
{
    public class GPSHeaderService
    {
        /// <summary>
        /// Represents a custom message header.
        /// </summary>


        public class SoapSecurityHeader : MessageHeader
        {
            private readonly string _password, _username, _nonce;
            private readonly DateTime _createdDate;


            public SoapSecurityHeader()
            {
                _password = System.Configuration.ConfigurationManager.AppSettings["AEConnectUidPwd"].ToString();
                _username = System.Configuration.ConfigurationManager.AppSettings["AEConnectUid"].ToString();
                _nonce = getNonce().ToString();
                _createdDate = DateTime.Now;
                this.Id = Guid.NewGuid().ToString();
            }

            public string Id { get; set; }

            public override string Name
            {
                get { return "Security"; }
            }

            public override string Namespace
            {
                get { return "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"; }
            }

            private static object getNonce()
            {
                string phrase = Guid.NewGuid().ToString();
                return phrase;
            }


            protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
            {
                writer.WriteStartElement("wsse", Name, Namespace);
                writer.WriteXmlnsAttribute("wsse", Namespace);
            }

            protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
            {
                var nonce = getNonce();
                string nonceToSend = Convert.ToBase64String(Encoding.UTF8.GetBytes(nonce.ToString()));
                UserNameSecurityToken userToken;
                userToken = new UserNameSecurityToken();

                IntPtr accountToken = WindowsIdentity.GetCurrent().Token;


                writer.WriteStartElement("wsse", "UsernameToken", Namespace);
                writer.WriteAttributeString("Id", userToken.Id.ToString());
                //writer.WriteAttributeString("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");

                writer.WriteStartElement("wsse", "Username", Namespace);
                writer.WriteValue(_username);
                writer.WriteEndElement();

                writer.WriteStartElement("wsse", "Password", Namespace);
                writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
                writer.WriteValue(_password);
                writer.WriteEndElement();

                writer.WriteStartElement("wsse", "Nonce", Namespace);
                writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
                writer.WriteValue(_nonce);
                writer.WriteEndElement();

                writer.WriteStartElement("wsse", "Created", Namespace);
                writer.WriteValue(_createdDate.ToString("YYYY-MM-DDThh:mm:ss"));
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }


        /// <summary>
        /// Represents a message inspector object that can be added to the <c>MessageInspectors</c> collection to view or modify messages.
        /// </summary>
        public class ClientMessageInspector : IClientMessageInspector
        {
            /// <summary>
            /// Enables inspection or modification of a message before a request message is sent to a service.
            /// </summary>
            /// <param name="request">The message to be sent to the service.</param>
            /// <param name="channel">The WCF client object channel.</param>
            /// <returns>
            /// The object that is returned as the <paramref name="correlationState " /> argument of
            /// the <see cref="M:System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)" /> method.
            /// This is null if no correlation state is used.The best practice is to make this a <see cref="T:System.Guid" /> to ensure that no two
            /// <paramref name="correlationState" /> objects are the same.
            /// </returns>
            public object BeforeSendRequest(ref Message request, IClientChannel channel)
            {
                SoapSecurityHeader header = new SoapSecurityHeader();

                request.Headers.Add(header);
                int headerIndexOfAction = request.Headers.FindHeader("Action", "http://schemas.microsoft.com/ws/2005/05/addressing/none");
                request.Headers.RemoveAt(headerIndexOfAction);

                return header.Id;
            }

            /// <summary>
            /// Enables inspection or modification of a message after a reply message is received but prior to passing it back to the client application.
            /// </summary>
            /// <param name="reply">The message to be transformed into types and handed back to the client application.</param>
            /// <param name="correlationState">Correlation state data.</param>
            public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
            {


            }
        }

        /// <summary>
        /// Represents a run-time behavior extension for a client endpoint.
        /// </summary>
        public class CustomEndpointBehavior : IEndpointBehavior
        {
            /// <summary>
            /// Implements a modification or extension of the client across an endpoint.
            /// </summary>
            /// <param name="endpoint">The endpoint that is to be customized.</param>
            /// <param name="clientRuntime">The client runtime to be customized.</param>
            public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
                clientRuntime.MessageInspectors.Add(new ClientMessageInspector());
            }

            /// <summary>
            /// Implement to pass data at runtime to bindings to support custom behavior.
            /// </summary>
            /// <param name="endpoint">The endpoint to modify.</param>
            /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
            public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
            {
                // Nothing special here
            }

            /// <summary>
            /// Implements a modification or extension of the service across an endpoint.
            /// </summary>
            /// <param name="endpoint">The endpoint that exposes the contract.</param>
            /// <param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
            public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
            {
                // Nothing special here
            }

            /// <summary>
            /// Implement to confirm that the endpoint meets some intended criteria.
            /// </summary>
            /// <param name="endpoint">The endpoint to validate.</param>
            public void Validate(ServiceEndpoint endpoint)
            {
                // Nothing special here
            }
        }

        public class UserNameSecurityToken : SecurityToken
        {
            public override AuthenticationKey AuthenticationKey { get { throw new NotImplementedException(); } }

            public override DecryptionKey DecryptionKey { get { throw new NotImplementedException(); } }


            public override EncryptionKey EncryptionKey { get { throw new NotImplementedException(); } }

            public override SignatureKey SignatureKey { get { throw new NotImplementedException(); } }

            public override bool SupportsDataEncryption { get { throw new NotImplementedException(); } }

            public override bool SupportsDigitalSignature { get { throw new NotImplementedException(); } }

            public override XmlElement GetXml(XmlDocument document)
            {
                throw new NotImplementedException();
            }

            public override void LoadXml(XmlElement element)
            {
                throw new NotImplementedException();
            }

            public override void Verify()
            {
                throw new NotImplementedException();
            }
        }

    }
}