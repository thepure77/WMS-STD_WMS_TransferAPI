using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferBusiness.Transfer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransferBusiness.Transfer;

namespace TransferAPI.Controllers
{
    [Route("api/TransferItem")]
    public class GoodsTransferItemController : Controller
    {

        [HttpGet("getGoodsTransferItem/{id}/{ischkQI}")]
        public IActionResult GetGoodsTransferItem(string id,bool ischkQI)
        {
            try
            {
                GoodsTransferItemService service = new GoodsTransferItemService();
                var result = service.GoodsTransferItem(id, ischkQI);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("addNewLocation")]
        public IActionResult AddNewLocation([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsTransferItemService();
                var Models = new GoodsTransferItemViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferItemViewModel>(body.ToString());
                var result = service.AddNewLocation(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ScanTagNo")]
        public IActionResult GetScanTagNo([FromBody]JObject body)
        {
            try
            {
                var service = new TransferItemService();
                var Models = new listTransferItem();
                Models = JsonConvert.DeserializeObject<listTransferItem>(body.ToString());
                var result = service.ScanTagNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }
        [HttpPost("CheckProductList/{id}")]
        public IActionResult GetCheckProductList(string id)
        {
            try
            {
                TransferItemService service = new TransferItemService();
                var result = service.CheckProductList(id);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }
        [HttpPost("ScanTagNoReserve")]
        public IActionResult GetScanTagNoReserve([FromBody]JObject body)
        {
            try
            {
                var service = new TransferItemService();
                var Models = new TransferViewModel();
                Models = JsonConvert.DeserializeObject<TransferViewModel>(body.ToString());
                var result = service.ScanTagNoReserve(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }
        [HttpPost("SumQty")]
        public IActionResult PostSumQty([FromBody]JObject body)
        {
            try
            {
                var service = new TransferItemService();
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
        [HttpPost("CheckBinBalance")]
        public IActionResult GetCheckBinBalance([FromBody]JObject body)
        {
            try
            {
                var service = new TransferItemService();
                var Models = new TransferViewModel();
                Models = JsonConvert.DeserializeObject<TransferViewModel>(body.ToString());
                var result = service.CheckBinBalance(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        //[HttpPost("SaveData")]
        //public IActionResult GetSaveData([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new TransferItemService();
        //        var Models = new TransferViewModel();
        //        Models = JsonConvert.DeserializeObject<TransferViewModel>(body.ToString());
        //        var result = service.SaveData(Models);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex);
        //    }
        //}
        //[HttpPost("SaveDataRelocation")]
        //public IActionResult SaveDataRelocation([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new TransferItemService();
        //        var Models = new listTransferItem();
        //        Models = JsonConvert.DeserializeObject<listTransferItem>(body.ToString());
        //        var result = service.SaveDataRelocation(Models);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex);
        //    }
        //}
    }
}
