using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferBusiness.Transfer;
using TransferBusiness.Transfer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GRAPI.Controllers
{
    [Route("api/TransferStockAdjustmentLPN")]
    public class TransferStockAdjustmentLPNController : ControllerBase
    {


        [HttpGet("filterOwner")]
        public IActionResult Get()
        {
            try
            {
                var service = new TransferStockAdjustmentLPNService();

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
                var service = new TransferStockAdjustmentLPNService();

                var result = service.filterWarehouse();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ScanLPN")]
        public IActionResult PostLocation([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStockAdjustmentLPNService();
                var Models = new BinBalanceDocViewModel();
                Models = JsonConvert.DeserializeObject<BinBalanceDocViewModel>(body.ToString());
                var result = service.ScanLPN(Models);
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
        //        var service = new TransferStockAdjustmentLPNService();
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
                var service = new TransferStockAdjustmentLPNService();
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
        [HttpPost("SumQty")]
        public IActionResult PostSumQty([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStockAdjustmentLPNService();
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

        [HttpPost("CheckScanLPN")]
        public IActionResult PostCheckScanLPN([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStockAdjustmentLPNService();
                var Models = new BinBalanceDocViewModel();
                Models = JsonConvert.DeserializeObject<BinBalanceDocViewModel>(body.ToString());
                var result = service.CheckScanLPN(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
