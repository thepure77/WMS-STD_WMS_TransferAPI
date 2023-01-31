using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBusiness.GoodIssue;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransferBusiness.Transfer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GIAPI.Controllers
{
    [Route("api/PopupGoodsTransfer")]
    [ApiController]
    public class PopupGoodsIssueController : ControllerBase
    {


        #region popupGoodsTransferfilter
        [HttpPost("popupGoodsTransferfilter")]
        public IActionResult popupGoodsIssuefilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsTransferService();
                var Models = new SearchGTModel();
                Models = JsonConvert.DeserializeObject<SearchGTModel>(body.ToString());
                var result = service.popupGoodsTransferfilter(Models);
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
