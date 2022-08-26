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

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteBL noteBL;
        //Constructor
        public NotesController(INoteBL noteBL )
        {
            this.noteBL = noteBL;
            
           
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
        
        [HttpPut]
        [Route("Image")]
        public IActionResult AddImage(IFormFile image, long NoteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "UserID").Value);
                var result = noteBL.AddImage(image, NoteID, userID);
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

    }
}
