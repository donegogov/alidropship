﻿using AliexpressOpenPlatformAPI.Dto;
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
            dynamic promo = JsonConvert.DeserializeObject(response.Body);

            return Ok(promo.resp_result.result + " " + response.Body);
        }                                                                                                                                                                                                                                                                                                                                                                                                                            

        // GET api/<DropShippingApiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DropShippingApiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
