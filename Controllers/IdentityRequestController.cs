using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApplication_forLearning.Services;
using ControllerNameAttribute = WebApplication_forLearning.Controllers.Convention.ControllerNameAttribute;

namespace WebApplication_forLearning.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [ControllerName("oauth2")]
    public class IdentityRequestController : ControllerBase
    {
        private readonly ILogger<IdentityRequestController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfigurationManager _configurationManager;

        public IdentityRequestController(IHttpClientFactory httpClientFactory, IConfigurationManager configuration, UserManager, ILogger<IdentityRequestController> logger)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configurationManager = configuration;
        }

        [HttpPost("v{version:apiVersion}/authentication/google")]
        public async Task<ActionResult> GoogleAuthenticating()
        {
            AuthorizationCodeFlowModel authorizationModel = new AuthorizationCodeFlowModel();
            using (var variable = new HttpRequestStreamReader(HttpContext.Request.Body, System.Text.Encoding.UTF8))
            {
                var issueProviderAuthenticationResponse = variable.ReadToEndAsync().Result;

                authorizationModel.SetAuthorizationCodeFlowModelValue(issueProviderAuthenticationResponse);
            }

            var identityServerBaseAddress = _configurationManager.GetValue<string>(ServicesUtil.IdentityServer_https);
            if (!string.IsNullOrEmpty(identityServerBaseAddress))
            {
                var aspClient = _httpClientFactory.CreateClient(identityServerBaseAddress); 

                // TODO: do sth
            }

            return new StatusCodeResult(200);
        }

        [HttpPost("v{version:apiVersion}/authentication/basicAccess")]
        public ActionResult LoginWithUserNameAndPassword()
        {
            // TODO: will update this method

            return new StatusCodeResult(200);
        }
    }

    public static class Ultilities
    {
        private static Dictionary<string, Action<string>> CallbackMethodsToSetPropertyValue(AuthorizationCodeFlowModel model)
        {
            Dictionary<string, Action<string>> actionsCallback = new Dictionary<string, Action<string>>(
                    model.GetType().GetProperties()
                    .Select(property => new KeyValuePair<string, Action<string>>(property.Name, (value) =>
                    {
                        // with current logic, value can be null but can still be empty, I will need to check outside this method
                        //if (!string.IsNullOrEmpty(value))
                        property.SetValue(model, value);
                    })));

            return actionsCallback;
        }

        public static bool SetAuthorizationCodeFlowModelValue(this AuthorizationCodeFlowModel model, string issueProviderAuthenticationResponse)
        {
            bool isSuccess = false;
            try
            {
                var actionsDictionary = CallbackMethodsToSetPropertyValue(model);

                var temp = issueProviderAuthenticationResponse.Split("&");
                for(int i = 0; i < temp.Length; i++)
                {
                    // propertyAndValue[0] is propertyName, and propertyAndValue[1] is its value
                    var propertyAndValue = temp[i].Split("=");

                    // depend on response's struct, so assume there's nothing wrong in this running moment.
                    if (actionsDictionary.ContainsKey(propertyAndValue[0]))
                    {
                        actionsDictionary[propertyAndValue[0]].Invoke(propertyAndValue[1]);
                    }
                }

                isSuccess = true;
            }
            catch (Exception)
            {
                isSuccess = false;
                // TODO: need to log exception
            }

            return isSuccess;
        }
    }

    public class AuthorizationCodeFlowModel
    {
        public string Code { get; private set; } = "";
        /// <summary>
        /// this property belong to server to add
        /// </summary>
        public string ClientId { get; private set; } = "";
        public string GrantType { get; private set; } = "";
        public string CodeVerifier { get; private set; } = "";
        public string RedirectUri { get; private set; } = "";
        /// <summary>
        /// this property belong to server to add
        /// </summary>
        public string ClientSecret { get; private set; } = "";

        //private MemberInfo GetMemberInfo(Expression expression)
        //{
        //    var lambda = (LambdaExpression)expression;

        //    MemberExpression memberExpression;
        //    if (lambda.Body is UnaryExpression)
        //    {
        //        var unaryExpression = lambda.Body as UnaryExpression;
        //        memberExpression = unaryExpression.Operand as MemberExpression;
        //    }
        //    else
        //        memberExpression = (MemberExpression)lambda.Body;

        //    return memberExpression.Member;
        //}
    }
}
