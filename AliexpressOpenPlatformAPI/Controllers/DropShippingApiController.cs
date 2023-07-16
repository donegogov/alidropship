﻿using AliexpressOpenPlatformAPI.Dto;
using AliexpressOpenPlatformAPI.Services;
using Iop.Api;
using Microsoft.AspNetCore.Mvc;

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

            return Ok(response.Body);
            //return Ok(aliApiDataDto.AccessToken + " " + aliApiDataDto.StoreUrl);
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
