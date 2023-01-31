using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferBusiness.Transfer;

namespace TransferAPI.Controllers
{
    [Route("api/TransferStatusLocation")]
    public class TransferStatusLocationController : Controller
    {

        [HttpPost("Confirm")]
        public IActionResult GetConfirm([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStatusLocationService();
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


        [HttpPost("ConfirmPallet_Tranfer")]
        public IActionResult ConfirmPallet_Tranfer([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStatusLocationService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.ConfirmPallet_Tranfer(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("Confirm_Tranfer_PartialPallet")]
        public IActionResult Confirm_Tranfer_PartialPallet([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStatusLocationService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.Confirm_Tranfer_PartialPallet(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("Confirm_Pallet_Inspection")]
        public IActionResult GetConfirm_Pallet_Inspection([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStatusLocationService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.ConfirmPallet(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("testpalletinspection")]
        public IActionResult testpalletinspection([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStatusLocationService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.testpalletinspection(Models);
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
                var service = new TransferStatusLocationService();
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
                var service = new TransferStatusLocationService();
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

        [HttpPost("GetScanLpnNoPallet")]
        public IActionResult GetScanLpnNoPallet([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStatusLocationService();
                var Models = new TransferViewModel();
                Models = JsonConvert.DeserializeObject<TransferViewModel>(body.ToString());
                var result = service.ScanLpnNoPallet(Models);
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
                var service = new TransferStatusLocationService();
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
