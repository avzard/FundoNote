using BuisnessLayer.Interface;
using BuisnessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Service;
using System.Linq;
using System;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace FundooNote.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LabelControler : Controller
    {
        private ILabelBL LabelBL;
        private readonly IMemoryCache memoryCache;
        private readonly FundoContext fundooContext;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<LabelControler> _logger;
        public LabelControler(ILabelBL LabelBL, IMemoryCache memoryCache, FundoContext fundooContext, IDistributedCache distributedCache, ILogger<LabelControler> logger)
        {
            this.LabelBL = LabelBL;
            this.memoryCache = memoryCache;
            this.fundooContext = fundooContext;
            this.distributedCache = distributedCache;
            this._logger = logger;
        }
        [Authorize]
        [HttpPost("Add")]
        public ActionResult AddLable(long userId, long notesId, string LabelName)
        {
            try
            {

                var result = LabelBL.AddLabel(userId,notesId, LabelName);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Label Created", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Label fail" });
                }


            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }

        }
        [Authorize]
        [HttpDelete]
        [Route("Delete")]
        public ActionResult DeleteLabel(long labelId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserID").Value);
                var result = LabelBL.DeleteLabel(labelId);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Label Deleted successfuly", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Label Deletion Failed" });
                }
            }
            catch(System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }


        }
        [Authorize]
        [HttpGet]
        [Route("Read")]
        public IActionResult ReadLabel(long labelId)
        {
            try
            {
                long UserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserID").Value);
                var result = LabelBL.ReadLabel(labelId, UserId);
                if (result != null)
                {
                    return Ok(new { success = true, message = "LABEL RECIEVED", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "LABEL RECIEVED FAILED" });
                }
            }
            catch (System.Exception ex )
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateLabel(long labelid, string labelname)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserID").Value);
                var result = LabelBL.UpdateLabel(labelid, labelname);
                if (result != null)
                {
                    return Ok(new { success = true, message = "LABEL UPDATE SUCCESSFULLY", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "LABEL UPDATE FAILED" });
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        [Authorize]
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
        {
            var cacheKey = "LabelList";
            string serializedLabelList;
            var LabelList = new List<LabelEntity>();
            var redisLabelList = await distributedCache.GetAsync(cacheKey);
            if (redisLabelList != null)
            {
                serializedLabelList = Encoding.UTF8.GetString(redisLabelList);
                LabelList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelList);
            }
            else
            {
                LabelList = await fundooContext.LabelTable.ToListAsync();
                serializedLabelList = JsonConvert.SerializeObject(LabelList);
                redisLabelList = Encoding.UTF8.GetBytes(serializedLabelList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisLabelList, options);
            }
            return Ok(LabelList);
        }
    }
}
