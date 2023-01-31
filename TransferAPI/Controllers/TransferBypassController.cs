using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransferBusiness.ConfigModel;
using TransferBusiness.GoodsTransfer.ViewModel;
using TransferBusiness.Transfer;
using TransferBusiness.Transfer.Report;

namespace TransferAPI.Controllers
{
    [Route("api/TransferBypassController")]
    [ApiController]
    public class TransferBypassController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public TransferBypassController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("filter")]
        public IActionResult filter([FromBody]JObject body)
        {
            try
            {
                var service = new TransferBypassService();
                var result = service.filter();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("getlocation")]
        public IActionResult getlocation([FromBody]JObject body)
        {
            try
            {
                var service = new TransferBypassService();
                var result = service.getlocation(body?.ToString() ?? string.Empty);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region confirmTranferBypass
        [HttpPost("confirmTranferBypass")]
        public IActionResult confirmVendor([FromBody]JObject body)
        {
            try
            {
                TransferBypassService service = new TransferBypassService();
                var result = service.ConfirmBypass(body?.ToString() ?? string.Empty);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


    }
}