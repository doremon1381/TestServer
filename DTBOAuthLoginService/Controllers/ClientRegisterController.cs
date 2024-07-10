using DTBOAuthLoginService.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using ControllerNameAttribute = DTBOAuthLoginService.Controllers.Conventions.ControllerNameAttribute;

namespace DTBOAuthLoginService.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[Controller]")]
    [ApiVersion("1.0")]
    [ControllerName("ClientRegister")]
    public class ClientRegisterController : ControllerBase
    {
        private LAuthenticationDbContext _dbContext;
        public ClientRegisterController(LAuthenticationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("{version:apiVersion}/basicAccess")]
        public void BasicAccess()
        {

        }
    }
}
