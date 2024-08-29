
using System.Linq;

using System.Configuration;

using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace ITAssetManagement.Filters
{
    public class ApiKeyAuthAttribute : ActionFilterAttribute
    {

        // The idea is to pass a header
        private const string ApiKeyHeaderName = "apiKey";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.Request.Headers.Contains(ApiKeyHeaderName))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { Message = "Missing API Key." });
                return;
            }

            var apiKey = actionContext.Request.Headers.GetValues(ApiKeyHeaderName).FirstOrDefault();
            var configuredApiKey = ConfigurationManager.AppSettings["ApiKey"]; //get the Key from the Webconfig file

            if (apiKey != configuredApiKey)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { Message = "Invalid API Key." });
            }

            base.OnActionExecuting(actionContext);
        }
    }
}




