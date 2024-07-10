using DTBOAuthLoginService.Database;
using DTBOAuthLoginService.Database.Model;
using Duende.IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DTBOAuthLoginService
{
    public static class AuthorizationResources
    {
        /// <summary>
        /// only use at server's initialization
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static Client[] GetClients(IConfigurationManager configuration)
        {
            var contextOptions = new DbContextOptionsBuilder<LAuthenticationDbContext>()
                 .UseSqlServer(configuration.GetConnectionString(DbUltilities.DatabaseName))
                 .Options;

            //var context = new MyClientDbServices(new LAuthenticationDbContext(contextOptions, configuration), configuration);
            //if (context.IsTableEmpty())
            //{
            //    var newClient = new MyClient()
            //    {
            //        ClientId = "WebApplication_forLearning",
            //        AllowedGrantTypes = GrantTypes.ClientCredentials,
            //        RedirectUris = { "https://localhost:5015/signin-oidc" },
            //        PostLogoutRedirectUris = { "https://localhost:5015/signout-callback-oidc" },
            //        FrontChannelLogoutUri = "https://localhost:5015/signout-oidc",
            //        // will add when has api
            //        AllowedScopes = { "" }
            //    };

            //    context.AddMany([newClient]);
            //}

            var context = new CustomClientDbServices(new LAuthenticationDbContext(contextOptions, null));
            if (context.IsTableEmpty())
            {
                var newClient = new CustomClient()
                {
                    ClientId = "WebApplication_forLearning",
                    AllowedGrantTypes = new List<string>(GrantTypes.ClientCredentials),
                    RedirectUris = { "https://localhost:5015/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5015/signout-callback-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5015/signout-oidc",
                    // will add when has api
                    AllowedScopes = { "" }
                };

                context.AddMany([newClient]);
            }

            var dbClients = context.GetAll().Select<CustomClient, Client>(mc => new Client()
            {
                ClientId = mc.ClientId,
                AllowedGrantTypes = new HashSet<string>(mc.AllowedGrantTypes),
                RedirectUris = new HashSet<string>(mc.RedirectUris),
                PostLogoutRedirectUris = new HashSet<string>(mc.PostLogoutRedirectUris),
                FrontChannelLogoutUri = mc.FrontChannelLogoutUri,
                // will add when has api
                AllowedScopes = new HashSet<string>(mc.AllowedScopes)
            }).ToArray();

            #region obsolete
            //var clients = new Client[]
            //{
            //    //new Client()
            //    //{
            //    //    ClientId = "OAuthDesktopApp",
            //    //    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
            //    //    RedirectUris = { "https://localhost:5015/signin-oidc" },
            //    //    PostLogoutRedirectUris = { "https://localhost:5015/signout-callback-oidc" },
            //    //    FrontChannelLogoutUri = "https://localhost:5015/signout-oidc",
            //    //    AllowedScopes = { "openid", "profile", "email", "phone" }
            //    //},
            //    new Client()
            //    {
            //        ClientId = "WebApplication_forLearning",
            //        AllowedGrantTypes = GrantTypes.ClientCredentials,
            //        RedirectUris = { "https://localhost:5015/signin-oidc" },
            //        PostLogoutRedirectUris = { "https://localhost:5015/signout-callback-oidc" },
            //        FrontChannelLogoutUri = "https://localhost:5015/signout-oidc",
            //        // will add when has api
            //        AllowedScopes = { "" }
            //    }
            //};
            #endregion

            return dbClients;
        }
    }

    /// <summary>
    /// TODO: will delete
    /// </summary>
    public static class RNGCryptoServicesUltilities
    {
        // rfc 7636 impliment
        public static void GetMitigateAttackMethod()
        {
            string status = RandomStringGeneratingWithLength(32);
            string code_verifier = RandomStringGeneratingWithLength(32);
            string code_challenge = Base64UrlEncodeNoPadding(code_verifier.WithSHA265());
            string code_challenge_method = "S256";
        }

        public static string RandomStringGeneratingWithLength(int length)
        {
            RNGCryptoServiceProvider strGenerator = new RNGCryptoServiceProvider();
            byte[] arr = new byte[length];
            strGenerator.GetBytes(arr, 0, length);

            return Base64UrlEncodeNoPadding(arr);
        }

        private static string Base64UrlEncodeNoPadding(byte[] str)
        {
            string base64 = Convert.ToBase64String(str);

            // convert base64 to base64url
            base64.Replace("+","-");
            base64.Replace("/", "_");

            // strip padding
            base64.Replace("=", "");

            return base64;
        }

        private static byte[] WithSHA265(this string str)
        {
            byte[] newByteArr = Encoding.ASCII.GetBytes(str);
            SHA256Managed sha256 = new SHA256Managed();
            return sha256.ComputeHash(newByteArr);
        }
    }
}
