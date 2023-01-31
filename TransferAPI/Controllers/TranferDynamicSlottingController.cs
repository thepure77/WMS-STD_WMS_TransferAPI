using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferBusiness.TranferDynamicSlottingService;
using TransferBusiness.Transfer;
using TransferBusiness.Transfer;

namespace TransferAPI.Controllers
{
    [Route("api/TranferDynamicSlotting")]
    public class TranferDynamicSlottingController : Controller
    {

        [HttpPost("GenDynamicSlotting")]
        public IActionResult GenDynamicSlotting([FromBody]JObject body)
        {
            try
            {
                var service = new TranferDynamicSlottingService();
                TranferDynamicSlottingViewModel Models = JsonConvert.DeserializeObject<TranferDynamicSlottingViewModel>(body.ToString());
                var result = service.GenDynamicSlotting(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
    }
}
