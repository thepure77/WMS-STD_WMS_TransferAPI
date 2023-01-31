using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBusiness.GoodIssue;
using MasterDataBusiness.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransferBusiness.GoodIssue;
using TransferBusiness.Transfer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TransferAPI.Controllers
{
    [Route("api/AssignJob")]
    [ApiController]
    public class AssignJobController : ControllerBase
    {
        #region Assign
        [HttpPost("assign")]
        public IActionResult autobasicSuggestion([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new AssignJobViewModel();
                Models = JsonConvert.DeserializeObject<AssignJobViewModel>(body.ToString());
                var result = service.assign(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region Assign
        [HttpPost("assignByLocation")]
        public IActionResult assignByLocation([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new View_AssignJobLocViewModel();
                Models = JsonConvert.DeserializeObject<View_AssignJobLocViewModel>(body.ToString());
                var result = service.assignByLoc(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region taskfilter
        [HttpPost("taskfilter")]
        public IActionResult taskfilter([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new TaskfilterViewModel();
                Models = JsonConvert.DeserializeObject<TaskfilterViewModel>(body.ToString());
                var result = service.taskfilter(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region confirmTask
        [HttpPost("confirmTask")]
        public IActionResult confirmTask([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new TaskfilterViewModel();
                Models = JsonConvert.DeserializeObject<TaskfilterViewModel>(body.ToString());
                var result = service.confirmTask(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region taskPopup
        [HttpPost("taskPopup")]
        public IActionResult taskPopup([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new GoodsTransferViewModel();
                Models = JsonConvert.DeserializeObject<GoodsTransferViewModel>(body.ToString());
                var result = service.taskPopup(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region autoGoodTaskTransferNo
        [HttpPost("autoGoodTaskTransferNo")]
        public IActionResult autoGoodIssueNo([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGoodTaskTransferNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region autoGoodTransferNo
        [HttpPost("autoGoodTransferNo")]
        public IActionResult autoGoodTransferNo([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGoodTransferNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region dropdownUser
        [HttpPost("dropdownUser")]
        public IActionResult dropdownUser([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new UserViewModel();
                Models = JsonConvert.DeserializeObject<UserViewModel>(body.ToString());
                var result = service.dropdownUser(Models);
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
