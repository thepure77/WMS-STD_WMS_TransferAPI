using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferBusiness.Transfer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace TransferAPI.Controllers
{
    [Route("api/TransferCarton")]
    public class TransferCartonController : Controller
    {
        [HttpPost("scanCartonNo")]
        public IActionResult GetScanCartonNo([FromBody]JObject body)
        {
            try
            {
                var service = new TransferCartonService();
                var Models = new TransferViewModel();
                Models = JsonConvert.DeserializeObject<TransferViewModel>(body.ToString());
                var result = service.ScanCartonNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("ScanTagNo/{id}")]
        public IActionResult GetScanTagNo(string id)
        {
            try
            {
                TransferCartonService service = new TransferCartonService();
                var result = service.ScanTagNo(id);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("CheckCarton")]
        public IActionResult CheckCarton([FromBody]JObject body)
        {
            try
            {
                var service = new TransferCartonService();
                var Models = new TransferViewModel();
                Models = JsonConvert.DeserializeObject<TransferViewModel>(body.ToString());
                var result = service.CheckCartonList(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("SumQty")]
        public IActionResult PostSumQty([FromBody]JObject body)
        {
            try
            {
                var service = new TransferCartonService();
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

        //[HttpPost("SaveData")]
        //public IActionResult Post([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new TransferCartonService();
        //        var Models = new TransferStockAdjustmentDocViewModel();
        //        Models = JsonConvert.DeserializeObject<TransferStockAdjustmentDocViewModel>(body.ToString());
        //        var result = service.SaveChanges(Models);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
    }
}
