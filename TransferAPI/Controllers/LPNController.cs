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
    [Route("api/LPN")]
    public class LPNController : Controller
    {


        [HttpPost("ScanBarcode")]
        public IActionResult GetScanBarcode([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStatusGradeService();
                var Models = new SumQtyBinbalanceViewModel();
                Models = JsonConvert.DeserializeObject<SumQtyBinbalanceViewModel>(body.ToString());
                var result = service.ScanBarcode(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        

        //[HttpPost("Confirm")]
        //public IActionResult GetConfirmScan([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new TransferStatusGradeService();
        //        var Models = new TransferViewModel();
        //        Models = JsonConvert.DeserializeObject<TransferViewModel>(body.ToString());
        //        var result = service.ConfirmScan(Models);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex);
        //    }
        //}

        [HttpPost("SumQty")]
        public IActionResult PostSumQty([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStatusGradeService();
                var Models = new SumQtyBinbalanceViewModel();
                Models = JsonConvert.DeserializeObject<SumQtyBinbalanceViewModel>(body.ToString());
                var result = service.SumQty(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
