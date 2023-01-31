using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferBusiness.Transfer;
using TransferBusiness.Transfer;

namespace TransferAPI.Controllers
{
    [Route("api/TransferCheckpallet")]
    public class TransferCheckpalletController : Controller
    {
       
        [HttpPost("checkpallet104")]
        public IActionResult checkpallet104([FromBody]JObject body)
        {
            try
            {
                var service = new Transfer104Service();
                Transfer104model Models = JsonConvert.DeserializeObject<Transfer104model>(body.ToString());
                var result = service.getlocation104(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("call104")]
        public IActionResult call104([FromBody]JObject body)
        {
            try
            {
                var service = new Transfer104Service();
                Transfer104model Models = JsonConvert.DeserializeObject<Transfer104model>(body.ToString());
                var result = service.call104(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("entercall104")]
        public IActionResult entercall104([FromBody]JObject body)
        {
            try
            {
                var service = new Transfer104Service();
                Transfer104model Models = JsonConvert.DeserializeObject<Transfer104model>(body.ToString());
                var result = service.entercall104(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
