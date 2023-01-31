using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferBusiness.Transfer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TransferAPI.Controllers
{
    [Route("api/RePrintTag")]
    public class RePrintTagController : Controller
    {
        [HttpPost("RePrintTagSearch")]
        public IActionResult RePrintTagSearch([FromBody]JObject body)
        {
            try
            {
                var service = new RePrintTagService();
                var Models = new RePrintTagSearchViewModel();
                Models = JsonConvert.DeserializeObject<RePrintTagSearchViewModel>(body.ToString());
                var result = service.RePrintTagSearch(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("autoTagFilter")]
        public IActionResult autoTagFilter([FromBody]JObject body)
        {
            try
            {
                var service = new RePrintTagService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoTagFilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("autoLocationFilter")]
        public IActionResult autoLocationFilter([FromBody]JObject body)
        {
            try
            {
                var service = new RePrintTagService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoLocationFilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("autoGoodsTransferFilter")]
        public IActionResult autoGoodsTransferFilter([FromBody]JObject body)
        {
            try
            {
                var service = new RePrintTagService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGoodsTransferFilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
