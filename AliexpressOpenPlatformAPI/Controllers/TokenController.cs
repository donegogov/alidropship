using AliexpressOpenPlatformAPI.Data;
using AliexpressOpenPlatformAPI.Helpers;
using Iop.Api;
using Iop.Api.Util;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AliexpressOpenPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string AliApiURL { get; set; }
        private readonly DataContext _context;
        public TokenController(DataContext context) 
        {
            AppKey = Environment.GetEnvironmentVariable("APP_KEY");
            AppSecret = Environment.GetEnvironmentVariable("APP_SECRET");
            AliApiURL = Environment.GetEnvironmentVariable("ALI_API_URL");
            _context = context;
        }

        [Route("ali-dropship-url")]
        [HttpGet]
        public async Task<IActionResult> GetAlidropshipURL([FromQuery] string storeUrl)
        {
            var store = await _context.AliExpressDropshipUsers.FirstOrDefaultAsync(x => x.StoreURL == storeUrl);
            string AuthorizeURL = "";
            if (store != null)
            {
                string serverUrl = "https://api-sg.aliexpress.com/oauth/authorize?";
                string responseType = "response_type=code";
                string forceAuth = "force_auth=true";
                string redirectUri = "redirect_uri=https://alidropship.azurewebsites.net/api/Token/redirect-uri?storeUrl=" + storeUrl;
                string clientId = "client_id=" + AppKey;
                string uuId = "uuid=" + storeUrl;
                AuthorizeURL = serverUrl +
                responseType + "&" +
                forceAuth + "&" +
                redirectUri + "&" +
                clientId + "&" +
                uuId;
            }
            else if(store == null)
            {
                return BadRequest("You don't have a valid licence");
            }
            
            return Ok(AuthorizeURL);
        }
        //https://api-sg.aliexpress.com/oauth/authorize?response_type=code&force_auth=true&redirect_uri=https://alidropship.azurewebsites.net/api/Token/redirect_uristoreUrl=google.com&client_id=33615924&uuid=google.com

        [Route("redirect-uri")]
        [HttpGet]
        public IActionResult GetRedirectUri([FromQuery] string code, [FromQuery] string storeUrl)
        {
            IIopClient client = new IopClient(AliApiURL, AppKey, AppSecret);
            IopRequest request = new IopRequest();
            request.SetApiName("/auth/token/create");
            request.AddApiParameter("uuid", storeUrl);
            request.AddApiParameter("code", code);
            IopResponse response = client.Execute(request, GopProtocolEnum.GOP);
            return Ok(response.IsError() + " " + response.Body + " code = " + code + " response= " + response.ToString());
        }
    }
}
