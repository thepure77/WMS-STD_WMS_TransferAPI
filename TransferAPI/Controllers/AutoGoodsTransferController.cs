using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using TransferBusiness.ConfigModel;
//using TransferBusiness.GoodsTransfer.ViewModel;
using TransferBusiness.Transfer;

namespace TransferAPI.Controllers
{
    [Route("api/AutoGoodsTransfer")]
    public class AutoGoodsTransferController : Controller
    {

        #region AutoOwnerfilter
        [HttpPost("autoOwnerfilter")]
        public IActionResult autoOwnerfilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsTransferService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoOwnerfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoWarehousefilter
        [HttpPost("autoWarehousefilter")]
        public IActionResult autoWarehousefilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsTransferService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoWarehousefilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoGoodsTransferNo
        [HttpPost("autoGoodsTransferNo")]
        public IActionResult autoGoodsTransferNo([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsTransferService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGoodsTransferNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoGoodsTransferNo2
        [HttpPost("autoGoodsTransferNo2")]
        public IActionResult autoGoodsTransferNo2([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsTransferService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGoodsTransferNo2(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
    }
}