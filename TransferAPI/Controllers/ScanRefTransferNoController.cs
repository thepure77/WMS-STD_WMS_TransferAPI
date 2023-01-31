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
    [Route("api/ScanRefTransferNo")]
    public class ScanRefTransferNoController : Controller
    {
        #region FilterTaskTransfer
        [HttpPost("FilterTaskTransfer")]
        public IActionResult FilterTaskTransfer([FromBody]JObject body)
        {
            try
            {
                var service = new ScanRefTransferNoService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoViewModel>(body.ToString());
                var result = service.FilterScanTransfer(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterGoodsTransfer
        [HttpPost("FilterGoodsTransfer")]
        public IActionResult FilterGoodsTransfer([FromBody]JObject body)
        {
            try
            {
                var service = new ScanRefTransferNoService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoViewModel>(body.ToString());
                var result = service.FilterGoodsTransfer(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterGoodsTransfer
        [HttpPost("FilterGoodsTransferReplenish")]
        public IActionResult FilterGoodsTransferReplenish([FromBody]JObject body)
        {
            try
            {
                var service = new ScanRefTransferNoService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoViewModel>(body.ToString());
                var result = service.FilterGoodsTransferReplenish(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterGoodsTransferUnPack
        [HttpPost("FilterGoodsTransferUnPack")]
        public IActionResult FilterGoodsTransferUnPack([FromBody]JObject body)
        {
            try
            {
                var service = new ScanRefTransferNoService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoViewModel>(body.ToString());
                var result = service.FilterGoodsTransferUnPack(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        [HttpGet("ScanTransfer/{transferNo}/{user}")]
        public IActionResult ScanRefTransferNo(string transferNo,string user)
        {
            try
            {
                var service = new ScanRefTransferNoService();
                var result = service.ScanTransfer(transferNo, user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpGet("GetTransferItem/{transfer_Index}/{tasktransfer_Index}")]
        public IActionResult GetTransferItem(Guid transfer_Index,Guid tasktransfer_Index)
        {
            try
            {
                var service = new ScanRefTransferNoService();
                var result = service.GetTransferItem(transfer_Index, tasktransfer_Index);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("Confirm")]
        public IActionResult Confirm([FromBody]JObject body)
        {
            try
            {
                var service = new ScanRefTransferNoService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoViewModel>(body.ToString());
                var result = service.Confirm(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("ConfirmTaskTransfer")]
        public IActionResult ConfirmTaskTransfer([FromBody]JObject body)
        {
            try
            {
                var service = new ScanRefTransferNoService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoViewModel>(body.ToString());
                var result = service.ConfirmTaskTransfer(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("Bypass_confirmTaskTransfer")]
        public IActionResult Bypass_confirmTaskTransfer([FromBody]JObject body)
        {
            try
            {
                var service = new ScanRefTransferNoService();
                var Models = JsonConvert.DeserializeObject<ConfirmTranferViewModel>(body.ToString());
                var result = service.Bypass_confirmTaskTransfer(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("UpdateRePutaway")]
        public IActionResult UpdateRePutaway([FromBody]JObject body)
        {
            try
            {
                var service = new ScanRefTransferNoService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoViewModel>(body.ToString());
                var result = service.UpdateRePutaway(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }
    }
}
