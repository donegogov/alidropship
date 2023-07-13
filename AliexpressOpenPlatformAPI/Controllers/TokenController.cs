using Iop.Api;
using Iop.Api.Util;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AliexpressOpenPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        /*[HttpGet]
        public IActionResult GetToken()
        {
            WebUtils webUtils = new WebUtils();
            IDictionary<string, string> pout = new Dictionary<string, string>();
            pout.Add("grant_type", "authorization_code");
            pout.Add("client_id", "33615924");
            pout.Add("client_secret", "5cb2e1ecd4a10a670d7c90bf759ee0cf");
            pout.Add("sp", "ae");
            pout.Add("code", "test");
            pout.Add("redirect_uri", "https://91.5.27.136:5031/api/token/redirect_uri");
            string output = webUtils.DoPost("https://oauth.aliexpress.com/token", pout);

            return Ok(output);
        }*/
        //https://api-sg.aliexpress.com/oauth/authorize?response_type=code&force_auth=true&redirect_uri=https://aliexpress.hopto.org/api/Token/redirect_uri&client_id=33615924&uuid=AliExpressDropshippingCode1212

        [Route("redirect_uri")]
        [HttpGet]
        public IActionResult GetRedirectUri([FromQuery] string code)
        {
            //Console.WriteLine("Code ==========GET===================" + code);
            //return Ok("Code ==========GET===================" + code);
            /*WebUtils webUtils = new WebUtils();
            IDictionary<string, string> pout = new Dictionary<string, string>();
            pout.Add("app_key", "33615924");
            pout.Add("app_secret", "5cb2e1ecd4a10a670d7c90bf759ee0cf");
            pout.Add("code", code);
            string output = webUtils.DoPost("https://api-sg.aliexpress.com/auth/token/create", pout);*/

            IIopClient client = new IopClient("https://api-sg.aliexpress.com", "33615924", "5cb2e1ecd4a10a670d7c90bf759ee0cf");
            IopRequest request = new IopRequest();
            request.SetApiName("/auth/token/create");
            request.AddApiParameter("uuid", "AliExpressDropshippingCode1212");
            request.AddApiParameter("code", code);
            Console.WriteLine("code = " + code);
            IopResponse response = client.Execute(request, GopProtocolEnum.GOP);
            Console.WriteLine(response.IsError());
            Console.WriteLine(response.Body);
            //var json = JsonObject.Parse(response) as JsonResult;
            return Ok(response.IsError() + " " + response.Body + " code = " + code + " response= " + response.ToString());
        }
    }
}
