using BuisnessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Context;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteBL noteBL;
        private readonly IMemoryCache memoryCache;
        private readonly FundoContext fundooContext;
        private readonly IDistributedCache distributedCache;
       
        public NotesController(INoteBL noteBL, IMemoryCache memoryCache, FundoContext fundooContext, IDistributedCache distributedCache)
        {
            this.noteBL = noteBL;
            this.memoryCache = memoryCache;
            this.fundooContext = fundooContext;
            this.distributedCache = distributedCache;

        }
        private long GetTokenId()
        {
            return Convert.ToInt64(User.FindFirst("Id").Value);
        }
        //Create a Note
        [Authorize]
        [HttpPost("Create")]
        public ActionResult CreateNote(Note note)
        {
            
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserID").Value);
                var result = noteBL.CreateNote(note, userId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Notes created successful", data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes not created " });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut("Update")]
        public ActionResult UpdateNote(Note note, long userId)
        {
            try
            { 
                var result = noteBL.UpdateNote(note, userId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Notes Updated successful", data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes did not Update " });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpDelete("Delete")]
        public IActionResult DeleteNotes(Note note, long userId)
        {
            try
            {
                if (noteBL.DeleteNotes(note, userId))
                {
                    return this.Ok(new { Success = true, message = "Deleted successful", data = noteBL.DeleteNotes(note, userId) });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes not deleted " });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpGet]
        [Route("Read")]
        public IActionResult ReadNotes()
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserID").Value);
                var result = noteBL.ReadNotes(userId);
                if (result != null)
                {
                    return Ok(new { success = true, message = "NOTES RECIEVED", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "NOTES RECIEVED FAILED" });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut]
        [Route("Pin")]
        public IActionResult PinNotes(long noteId)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserID").Value);
                var result = noteBL.PinNotes(userID, noteId);
                if (result != false)
                {
                    return Ok(new { success = true, message = "NOTE PIN" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "NOTE CANNOT PIN" });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut]
        [Route("Archive")]
        public IActionResult Archive(long noteId)
        {

            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserID").Value);
                var result = noteBL.Archive(noteId, userID);

                if (result == true)
                {
                    return Ok(new { success = true, message = "NOTE ARCHIVE SUCCESSFULL!" });
                }
                else if (result == false)
                {
                    return Ok(new { success = true, message = "NOTE ARCHIVE FAIL!" });
                }
                return BadRequest(new { success = false, message = "Operation Fail." });
            }
            catch (System.Exception)
            {
                throw;
            }

        }
        [Authorize]
        [HttpPut]
        [Route("Trash")]
        public IActionResult Trash(long noteId)
        {

            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserID").Value);
                var result = noteBL.Trash(noteId, userID);

                if (result == true)
                {
                    return Ok(new { success = true, message = "NOTE TRANSH SUCCESSFULL!" });
                }
                else if (result == false)
                {
                    return Ok(new { success = true, message = "NOTE TRANSH FAIL!" });
                }
                return BadRequest(new { success = false, message = "Operation Fail." });
            }
            catch (System.Exception)
            {
                throw;
            }

        }
        [Authorize]
        [HttpPut]
        [Route("Image")]
        public IActionResult AddImage(IFormFile image, long noteId)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "UserID").Value);
                var result = noteBL.AddImage(image, noteId, userID);
                if (result != null)
                {
                    return Ok(new { success = true, message = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cannot upload image." });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut]
        [Route("Color")]
        public IActionResult Color(long noteId, string color)
        {

            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "UserID").Value);
                var result = noteBL.Color(noteId, color);

                if (result != null)
                {
                    return Ok(new { success = true, message = "Color changed successfully to "+color });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Color not changed." });
                }
            }
            catch (System.Exception)
            {
                throw;
            }

        }
        [Authorize]
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
        {
            var cacheKey = "NoteList";
            string serializedNoteList;
            var NoteList = new List<NotesEntity>();
            var redisNoteList = await distributedCache.GetAsync(cacheKey);
            if (redisNoteList != null)
            {
                serializedNoteList = Encoding.UTF8.GetString(redisNoteList);
                NoteList = JsonConvert.DeserializeObject<List<NotesEntity>>(serializedNoteList);
            }
            else
            {
                NoteList = await fundooContext.NotesTable.ToListAsync();
                serializedNoteList = JsonConvert.SerializeObject(NoteList);
                redisNoteList = Encoding.UTF8.GetBytes(serializedNoteList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNoteList, options);
            }
            return Ok(NoteList);
        }
    }
}
