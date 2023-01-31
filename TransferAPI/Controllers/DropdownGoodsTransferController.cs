using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransferBusiness.ConfigModel;
//using TransferBusiness.ConfigModel;
//using TransferBusiness.GoodsTransfer.ViewModel;
using TransferBusiness.Transfer;

namespace TransferAPI.Controllers
{
    [Route("api/DropdownGoodsTransfer")]
    public class DropdownGoodsTransferController : Controller
    {
        #region DropdownStatus
        [HttpPost("dropdownStatus")]
        public IActionResult DropdownStatus([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsTransferService();
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

        #region dropdownItemStatus
        [HttpPost("dropdownItemStatus")]
        public IActionResult dropdownItemStatus([FromBody]JObject body)
        {
            try
            {
                var service = new TransferStatusLocationService();
                var Models = new ItemStatusDocViewModel();
                Models = JsonConvert.DeserializeObject<ItemStatusDocViewModel>(body.ToString());
                var result = service.dropdownItemStatus(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion
    }
}