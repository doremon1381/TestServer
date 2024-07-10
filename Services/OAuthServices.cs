using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Nodes;

namespace WebApplication_forLearning.Services
{
    public class OAuthServices : IOAuthServices
    {
        private string _baseOAuthServerURI_Https;
        private string _baseOAuthServerURI_Http;
        private IConfiguration _configuration;

        public OAuthServices(IConfiguration configuration)
        {
            _configuration = configuration;

            _baseOAuthServerURI_Http = _configuration.GetValue<string>("OAuthServer:baseHttp");
            _baseOAuthServerURI_Https = _configuration.GetValue<string>("OAuthServer:baseHttps");
        }
        public string GetTokenToAccessApi()
        {
            // TODO: will modify this function
            return _baseOAuthServerURI_Https;
        }

        private string GetRequestToOauthServer()
        {
            int apiVersion = 1;
            string tokenRequestURI = _baseOAuthServerURI_Https + $"v{apiVersion}/oauth/register";
            string tokenRequestBody = "";

            // learn to use httpwebrequest before httpclient
            HttpWebRequest tokenRequest = (HttpWebRequest)HttpWebRequest.Create(tokenRequestURI);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.ContentLength = tokenRequestBody.Length;
            tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            // TODO: for not causing intellisense error.
            return "";
        }
    }

    /// <summary>
    /// To OAuthServer, application as client will need to get authorization code to access database when starting of this one.
    /// </summary>
    public interface IOAuthServices
    {
        string GetTokenToAccessApi();
    }

    /// <summary>
    /// for more info: https://hl7.org/fhir/us/udap-security/registration.html
    /// To register dynamically, the client application first constructs a software statement as per section 2 of UDAP Dynamic Client Registration.
    /// </summary>
    public class ClientCredentialRegistrationModel
    {
        //issuer of JWT -- unique clientCredential url
        public string iss { get; set; }
        //same as iss, in typical use, the client will not yet have client id from authentication server
        public string sub { get; set; }
        //authentication server url
        public string aud { get; set; }
        // expiration time interger for this software statement, 
        // the exp shall be no more than 5 minutes after the value of iat claim
        public string exp { get; set; }
        // issued time interger for this software statement
        public string iat { get; set; }

        public string ToJsonObject()
        {
            //return new JsonObject(IEnumerable<KeyValuePair, string>);
            return "";
        }
    }
}
