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
    [Route("api/TransferUnpack")]
    [ApiController]
    public class TransferUnpackController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public TransferUnpackController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region find
        [HttpGet("find/{id}")]
        public IActionResult find(Guid id)
        {
            try
            {
                var service = new TransferUnpackService();
                var result = service.find(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        [HttpPost("filter")]
        public IActionResult filter([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = new SearchGTModel();
                Models = JsonConvert.DeserializeObject<SearchGTModel>(body.ToString());
                var result = service.filter(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region FilterGoodsTransferUnPack
        [HttpPost("FilterGoodsTransferUnPack")]
        public IActionResult FilterGoodsTransferUnPack([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
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

        #region FilterGoodsTransferPack
        [HttpPost("FilterGoodsTransferPack")]
        public IActionResult FilterGoodsTransferPack([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = JsonConvert.DeserializeObject<ScanRefTransferNoViewModel>(body.ToString());
                var result = service.FilterGoodsTransferPack(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        [HttpPost("ScanLocation")]
        public IActionResult GetScanLocation([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
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

        [HttpPost("createGoodsTransferHeader")]
        public IActionResult CreateGoodsTransferHeader([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.CreateGoodsTransferHeader(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region pickProduct
        [HttpPost("pickProduct")]
        public IActionResult pickProduct([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = JsonConvert.DeserializeObject<PickbinbalanceViewModel>(body.ToString());
                var result = service.pickProduct(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region deletePickProduct
        [HttpPost("deletePickProduct")]
        public IActionResult deletePickProduct([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = JsonConvert.DeserializeObject<PickbinbalanceViewModel>(body.ToString());
                var result = service.deletePickProduct(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region DropdownDocumentType
        [HttpPost("dropdownDocumentType")]
        public IActionResult dropdownDocumentType([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = new DocumentTypeViewModel();
                Models = JsonConvert.DeserializeObject<DocumentTypeViewModel>(body.ToString());
                var result = service.dropdownDocumentType(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region DropdownStatus
        [HttpPost("dropdownStatus")]
        public IActionResult DropdownStatus([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = new ProcessStatusViewModel();
                Models = JsonConvert.DeserializeObject<ProcessStatusViewModel>(body.ToString());
                var result = service.dropdownStatus(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region UpdateDocument
        [HttpPost("updateDocument")]
        public IActionResult UpdateStatus([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.updateDocument(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region UpdateDocumentReturnTransNo
        [HttpPost("updateDocumentReturnTransNo")]
        public IActionResult UpdateDocumentReturnTransNo([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.updateDocumentReturnTransNo(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region ConfirmDocument
        [HttpPost("confirmDocument")]
        public IActionResult ConfirmStatus([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.confirmDocument(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region ConfirmTransfer
        [HttpPost("confirmTransfer")]
        public IActionResult ConfirmTransfer([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.confirmTransfer(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region get Tranfer location
        [HttpPost("get_Tranfer_location")]
        public IActionResult get_Tranfer_location([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                getTranferLocationViewModel Models = JsonConvert.DeserializeObject<getTranferLocationViewModel>(body.ToString());
                var result = service.get_Tranfer_location(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region ConfirmStatus
        [HttpPost("deleteDocument")]
        public IActionResult DeleteDocument([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.deleteDocument(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region ListPickProduct
        [HttpPost("ListPickProduct")]
        public IActionResult ListPickProduct([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = JsonConvert.DeserializeObject<ListPickbinbalanceViewModel>(body.ToString());
                var result = service.ListPickProduct(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region deletePickProductQI
        [HttpPost("deletePickProductQI")]
        public IActionResult deletePickProductQI([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = JsonConvert.DeserializeObject<PickbinbalanceViewModel>(body.ToString());
                var result = service.deletePickProductQI(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region GetReport
        [HttpPost("GetReport")]
        public IActionResult GetReport([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new TransferUnpackService();
                var Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                localFilePath = service.GetReport(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }
        #endregion

        [HttpPost("sendToSap")]
        public IActionResult sendToSap([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = new ListGoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<ListGoodsTransferViewModel>(body.ToString());
                var result = service.sentToSap(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("SentToSapGetJson/{id}")]
        public IActionResult SentToSapGetJson(string id)
        {
            try
            {
                var service = new TransferUnpackService();
                var result = service.SentToSapGetJson(id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        #region ListdeletePickProduct
        [HttpPost("ListdeletePickProduct")]
        public IActionResult ListdeletePickProduct([FromBody]JObject body)
        {
            try
            {
                var service = new TransferUnpackService();
                var Models = JsonConvert.DeserializeObject<ListPickbinbalanceViewModel>(body.ToString());
                var result = service.ListdeletePickProduct(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        [HttpPost("printTagPutawayTransfer")]
        public IActionResult ReportTagPutaway([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new TransferUnpackService();
                var Models = new TagPutawayTransferViewModel();
                Models = JsonConvert.DeserializeObject<TagPutawayTransferViewModel>(body.ToString());
                localFilePath = service.printTagPutawayTransferNew(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }

    }
}