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
    [Route("api/TransferStockAdjustment")]
    public class TransferStockAdjustmentController : ControllerBase
    {


        [HttpGet("filterOwner")]
        public IActionResult Get()
        {
            try
            {
                var service = new TransferStockAdjustmentService();

                var result = service.filterOwner();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("filterWarehouse")]
        public IActionResult GetWarehouse()
        {
            try
            {
                var service = new TransferStockAdjustmentService();

                var result = service.filterWarehouse();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ScanLocation")]
        public IActionResult PostLocation([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStockAdjustmentService();
                var Models = new BinBalanceDocViewModel();
                Models = JsonConvert.DeserializeObject<BinBalanceDocViewModel>(body.ToString());
                var result = service.ScanLocation(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("SumQty")]
        public IActionResult PostSumQty([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStockAdjustmentService();
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


        //[HttpPost]
        //public IActionResult Post([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new TransferStockAdjustmentService();
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

        [HttpPost("ScanProduct")]
        public IActionResult PostProductBarcode([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStockAdjustmentService();
                var Models = new BinBalanceDocViewModel();
                Models = JsonConvert.DeserializeObject<BinBalanceDocViewModel>(body.ToString());
                var result = service.ScanBarCode(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("filterReasonCode")]
        public IActionResult GetReasonCode()
        {
            try
            {
                var service = new TransferStockAdjustmentService();

                var result = service.filterReasonCode();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("CheckLocation")]
        public IActionResult PostCheckLocation([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStockAdjustmentService();
                var Models = new BinBalanceDocViewModel();
                Models = JsonConvert.DeserializeObject<BinBalanceDocViewModel>(body.ToString());
                var result = service.CheckLocation(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
