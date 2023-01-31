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
    [Route("api/ScanRefTransferNoExport")]
    public class ScanRefTransferNoExportController : Controller
    {
        #region FilterTaskTransferExport
        [HttpPost("FilterTaskTransferExport")]
        public IActionResult FilterTaskTransferExport([FromBody]JObject body)
        {
            try
            {
                var service = new ScanRefTransferNoExportService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoExportViewModel>(body.ToString());
                var result = service.FilterScanTransferExport(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterGoodsTransferExport
        [HttpPost("FilterGoodsTransferExport")]
        public IActionResult FilterGoodsTransferExport([FromBody]JObject body)
        {
            try
            {
                var service = new ScanRefTransferNoExportService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoExportViewModel>(body.ToString());
                var result = service.FilterGoodsTransferExport(Models);
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
                var service = new ScanRefTransferNoExportService();
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
                var service = new ScanRefTransferNoExportService();
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
                var service = new ScanRefTransferNoExportService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoExportViewModel>(body.ToString());
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
                var service = new ScanRefTransferNoExportService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoExportViewModel>(body.ToString());
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
                var service = new ScanRefTransferNoExportService();
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
                var service = new ScanRefTransferNoExportService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoExportViewModel>(body.ToString());
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
