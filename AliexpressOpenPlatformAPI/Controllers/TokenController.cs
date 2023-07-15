using AliexpressOpenPlatformAPI.Data;
using AliexpressOpenPlatformAPI.Entities;
using AliexpressOpenPlatformAPI.Helpers;
using Iop.Api;
using Iop.Api.Util;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
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
        private static readonly HttpClient _client = new HttpClient();
        public TokenController(DataContext context) 
        {
            AppKey = Environment.GetEnvironmentVariable("APP_KEY");
            AppSecret = Environment.GetEnvironmentVariable("APP_SECRET");
            AliApiURL = Environment.GetEnvironmentVariable("ALI_API_URL");
            _context = context;
        }

        [Route("connection-string")]
        [HttpGet]
        public async Task<IActionResult> GetConnectionString()
        {
            var connStr = Environment.GetEnvironmentVariable("MYSQLCONNSTR_mysql_nopaliexpressdropshipping_connectionstring");
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            return Ok(connStr + " " + directory);
        }

            [Route("ali-dropship-url")]
        [HttpGet]
        public async Task<IActionResult> GetAlidropshipURL([FromQuery] string storeUrl)
        {
            var store = await _context.AliExpressDropshipUsers.FirstOrDefaultAsync(x => x.StoreURL == storeUrl);
            string AuthorizeURL = "";
            if (store == null)
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
            else if(store != null)
            {
                return Ok("You have a valid licence");
            }
            var json = JsonConvert.SerializeObject(new { AuthorizeURL = AuthorizeURL });

            return Ok(json);
        }
        //https://api-sg.aliexpress.com/oauth/authorize?response_type=code&force_auth=true&redirect_uri=https://alidropship.azurewebsites.net/api/Token/redirect_uristoreUrl=google.com&client_id=33615924&uuid=google.com

        [Route("redirect-uri")]
        [HttpGet]
        public async Task<IActionResult> GetAuthorizationRedirectUriAsync([FromQuery] string code, [FromQuery] string storeUrl)
        {
            IIopClient client = new IopClient(AliApiURL, AppKey, AppSecret);
            IopRequest request = new IopRequest();
            request.SetApiName("/auth/token/create");
            request.AddApiParameter("uuid", storeUrl);
            request.AddApiParameter("code", code);
            IopResponse response = client.Execute(request, GopProtocolEnum.GOP);
            if (!response.IsError())
            {
                HttpContent httpContent = new StringContent(response.Body, Encoding.UTF8, "application/json");
                using HttpResponseMessage responseStore = await _client.PostAsync(storeUrl + "api/ApiAliExpressDropshipping/aliexpress-authorization-data", httpContent);
                if (responseStore.IsSuccessStatusCode)
                {
                    return Ok("Authorization successful, now you can close this windows and go back to the store");
                } else
                {
                    return BadRequest(responseStore.Content);
                }
            } else
            {
                return BadRequest("There was a error somewhere, error msg \"" + response.Message + "\" if you have a licence and again have issues getting authorization please contact admin of this plugin");
            }
            //return Ok(response.IsError() + " " + response.Body + " code = " + code + " response= " + response.ToString());
        }

        [Route("licence")]
        [HttpPost]
        public async Task<IActionResult> AddLicence([FromForm] string websiteUrl)
        {
            var store = await _context.AliExpressDropshipUsers.FirstOrDefaultAsync(x => x.StoreURL == websiteUrl);

            if(store == null)
            {
                AliExpressDropshipUser aliExpressDropshipUser = new AliExpressDropshipUser();
                aliExpressDropshipUser.StoreURL = websiteUrl;
                await _context.AliExpressDropshipUsers.AddAsync(aliExpressDropshipUser);
                await _context.SaveChangesAsync();
            }
            else if(store != null) 
            {
                return Ok("This website has a licence");
            }
            return Ok("Successfuly added licence to the website " + websiteUrl);
        }
    }
}
