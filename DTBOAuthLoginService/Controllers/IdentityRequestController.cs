using DTBOAuthLoginService.Controllers.Conventions;
using DTBOAuthLoginService.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ControllerNameAttribute = DTBOAuthLoginService.Controllers.Conventions.ControllerNameAttribute;

namespace DTBOAuthLoginService.Controllers
{
    /// <summary>
    /// Use for authorization request from client
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [ControllerName("authentication")]
    internal class IdentityRequestController : ControllerBase
    {
        private ICustomClientDbServices _clientDbContext;
        public IdentityRequestController(ICustomClientDbServices customDbServices) 
        {
            _clientDbContext = customDbServices;
        }

        [HttpPost("{version:apiVersion}/basicAccess")]
        public void BasicAccess()
        {

        }

        ///// <summary>
        ///// Intent to use for client as application
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("v{version:apiVersion}/register")]
        //public string RegisterRequestHandler()
        //{
        //    return "OK!";
        //}

        [HttpPost("v{version:apiVersion}/oauth/google")]
        public string GoogleAuthentication()
        {
            // get user's infor from database
            // if it does existed, if authorization_code is not expired
            //      then return to user current authorization_code
            // else 
            //      then return new authorization code
            // if user does not exist
            //      create new user, with authorization code and its timeout
            //      add new user to InMemoryClients of duende identityserver
            //      save to database
            //      return to client authorization code

            return "";
        }

        private void _GetGoogleIdToken()
        {

        }

        private void _CreateLoginSession()
        {

        }
    }
}
