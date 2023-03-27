using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client.API.Middlewares
{
    [Authorize]
    public class EncryptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly EncryptionService encryptionService;

        public EncryptionMiddleware(RequestDelegate next, EncryptionService encryptionService)
        {
            _next = next;
            this.encryptionService = encryptionService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            List<string> excludeURL = GetExcludeURLList();
            if (httpContext.User.Identity.IsAuthenticated && !excludeURL.Contains(httpContext.Request.Path.Value))
            {
                httpContext.Request.Body = encryptionService.DecryptStream(httpContext.Request.Body);
                if (httpContext.Request.QueryString.HasValue)
                {
                    string decryptedString = encryptionService.DecryptString(httpContext.Request.QueryString.Value.Substring(1));
                    httpContext.Request.QueryString = new QueryString($"?{decryptedString}");
                }
            }
            await _next(httpContext);
        }

        private List<string> GetExcludeURLList()
        {
            List<string> excludeURL = new List<string>();
            //excludeURL.Add("/api/example");
            return excludeURL;
        }
    }

}
