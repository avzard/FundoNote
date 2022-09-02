using BuisnessLayer.Interface;
using BuisnessLayer.Service;
using CommonLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundooNote.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CollaborationController : ControllerBase
    {
        private ICollaborationBL collaborationBL;
        private readonly IMemoryCache memoryCache;
        private readonly FundoContext fundooContext;
        private readonly IDistributedCache distributedCache;
        public CollaborationController(ICollaborationBL collaborationBL, IMemoryCache memoryCache, FundoContext fundooContext, IDistributedCache distributedCache)
        {
            this.collaborationBL = collaborationBL;
            this.memoryCache = memoryCache;
            this.fundooContext = fundooContext;
            this.distributedCache = distributedCache;
        }
        [HttpPost("Add")]
        public ActionResult AddCollab(long notesId, string Email)
        {
            try
            {

                var result = collaborationBL.AddCollab(notesId, Email);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Colab Succesful", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Colab Fail" });
                }


            }
            catch (System.Exception)
            {

                throw;
            }

        }
        [HttpDelete]
        [Route("Delete")]
        public ActionResult DeleteCollab(long notesId, string Email)
        {

            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserID").Value);
            var result = collaborationBL.DeleteCollab(notesId, Email);
            if (result != null)
            {
                return Ok(new { success = true, message = "Collaborater Email deleted", data = result });
            }
            else
            {
                return BadRequest(new { success = false, message = "Collaborater Email not deleted" });
            }


        }
        [HttpGet]
        [Route("Read")]
        public IActionResult ReadCollaborate()
        {
            try
            {

                string noteId = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var result = collaborationBL.ReadCollaborate(noteId);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Colab Succesful", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Colab Succesful" });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
        {
            var cacheKey = "ColabList";
            string serializedColabList;
            var ColabList = new List<Collaboration>();
            var redisColabList = await distributedCache.GetAsync(cacheKey);
            if (redisColabList != null)
            {
                serializedColabList = Encoding.UTF8.GetString(redisColabList);
                ColabList = JsonConvert.DeserializeObject<List<Collaboration>>(serializedColabList);
            }
            else
            {
                ColabList = await fundooContext.CollabTable.ToListAsync();
                serializedColabList = JsonConvert.SerializeObject(ColabList);
                redisColabList = Encoding.UTF8.GetBytes(serializedColabList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisColabList, options);
            }
            return Ok(ColabList);
        }
    }
}
