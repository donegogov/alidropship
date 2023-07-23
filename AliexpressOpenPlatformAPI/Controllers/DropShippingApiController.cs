using AliexpressOpenPlatformAPI.Dto;
using AliexpressOpenPlatformAPI.Services;
using FastJSON;
using Iop.Api;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AliexpressOpenPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropShippingApiController : ControllerBase
    {
        private readonly IDropShippingApiService _dropShippingApiService;
        public DropShippingApiController(IDropShippingApiService dropShippingApiService)
        {
            _dropShippingApiService = dropShippingApiService;
        }

        [Route("feed-name")]
        [HttpPost]
        public IActionResult GetFeedName([FromBody] AliApiDataDto aliApiDataDto)
        {
            IopResponse response = _dropShippingApiService.ApiGetFeedName(aliApiDataDto.AccessToken);
            dynamic promo = JsonConvert.DeserializeObject<ExpandoObject>(response.Body);

            return Ok(promo.resp_result.result.promos);
        }

        [Route("category")]
        [HttpPost]
        public IActionResult GetCategory([FromBody] AliApiDataDto aliApiDataDto)
        {
            IopResponse response = _dropShippingApiService.ApiGetCategory(aliApiDataDto.AccessToken);
            dynamic promo = JsonConvert.DeserializeObject<ExpandoObject>(response.Body);

            return Ok(promo.resp_result.result.categories);
        }

        [Route("products")]
        [HttpPost]
        public IActionResult GetProducts([FromBody] string accessToken,
            string country,
            string targetCurrency,
            string targetLanguage,
            string pageSize,
            string sort,
            string pageNumber,
            string categoryId,
            string feedName)
        {
            IopResponse response = _dropShippingApiService.ApiGetAliexpressProducts(accessToken, country, targetCurrency, targetLanguage, pageSize, sort, pageNumber, categoryId, feedName);
            dynamic products = JsonConvert.DeserializeObject<ExpandoObject>(response.Body);

            return Ok(response.Body);
        }

        // PUT api/<DropShippingApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DropShippingApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
