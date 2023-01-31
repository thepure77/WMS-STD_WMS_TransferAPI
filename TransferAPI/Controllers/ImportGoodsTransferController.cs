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
    [Route("api/ImportGoodsTransfer")]
    [ApiController]
    public class ImportGoodsTransferController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ImportGoodsTransferController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region validateImportTranfer
        [HttpPost("validateImportTranfer")]
        public IActionResult validateVendor([FromBody]JObject body)
        {
            try
            {
                ImportGoodsTranferService service = new ImportGoodsTranferService();
                var result = service.validateImportTranfer(body?.ToString() ?? string.Empty);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region confirmImportTranfer
        [HttpPost("confirmImportTranfer")]
        public IActionResult confirmVendor([FromBody]JObject body)
        {
            try
            {
                ImportGoodsTranferService service = new ImportGoodsTranferService();
                var result = service.ConfirmImport(body?.ToString() ?? string.Empty);
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