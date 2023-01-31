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
    [Route("api/TransferPallet")]
    public class TransferPalletController : Controller
    {
        //[HttpPost("GroupProduct")]
        //public IActionResult GetGroupProduct([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new TransferPalletService();
        //        var Models = new listTransferItem();
        //        Models = JsonConvert.DeserializeObject<listTransferItem>(body.ToString());
        //        var result = service.GroupProduct(Models);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex);
        //    }
        //}

        //[HttpPost("CheckBinBalance")]
        //public IActionResult GetCheckBinBalance([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new TransferPalletService();
        //        var Models = new TransferViewModel();
        //        Models = JsonConvert.DeserializeObject<TransferViewModel>(body.ToString());
        //        var result = service.CheckBinBalance(Models);
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
                var service = new TransferPalletService();
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

        [HttpPost("Confirm")]
        public IActionResult GetConfirm([FromBody]JObject body)
        {
            try
            {
                var service = new TransferPalletService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.Confirm(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("filterTransferItem")]
        public IActionResult FilterTransferItem([FromBody]JObject body)
        {
            try
            {
                var service = new TransferPalletService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.FilterTransferItem(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("ScanLpnNo")]
        public IActionResult GetScanLpnNo([FromBody]JObject body)
        {
            try
            {
                var service = new TransferPalletService();
                var Models = new TransferViewModel();
                Models = JsonConvert.DeserializeObject<TransferViewModel>(body.ToString());
                var result = service.ScanLpnNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("ScanLpnNoStatusloaction")]
        public IActionResult ScanLpnNoStatusloaction([FromBody]JObject body)
        {
            try
            {
                var service = new TransferPalletService();
                var Models = new TransferViewModel();
                Models = JsonConvert.DeserializeObject<TransferViewModel>(body.ToString());
                var result = service.ScanLpnNoStatusloaction(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }


        [HttpPost("ScanLocation")]
        public IActionResult GetScanLocation([FromBody]JObject body)
        {
            try
            {
                var service = new TransferPalletService();
                var Models = new TransferViewModel();
                Models = JsonConvert.DeserializeObject<TransferViewModel>(body.ToString());
                var result = service.ScanLocation(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }
    }
}
