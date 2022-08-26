using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        private readonly FundoContext fundooContext;
        private IConfiguration _config;
        private readonly IConfiguration cloudinaryEntity;

        public NoteRL(FundoContext fundooContext, IConfiguration configuration, IConfiguration cloudinaryEntity)
        {
            this.fundooContext = fundooContext;
            this._config = configuration;
            this.cloudinaryEntity = cloudinaryEntity;
        }
        public NotesEntity CreateNote(Note note, long userId)
        {
            try
            {
                NotesEntity newNotes = new NotesEntity();
                var result = fundooContext.NotesTable.Where(e => e.UserID == userId);
                if (result != null)
                {
                    newNotes.UserID = userId;
                    newNotes.Title = note.Title;
                    newNotes.Description = note.Description;
                    newNotes.Color = note.Color;
                    newNotes.Image = note.Image;
                    newNotes.Archive = note.Archive;
                    newNotes.Trash = note.Trash;
                    newNotes.Pin = note.Pin;
                    newNotes.ModifiedTime = note.ModifiedTime;
                    newNotes.CreatedTime = note.CreatedTime;
                    newNotes.Reminder = note.Reminder; 
                    
                    fundooContext.NotesTable.Add(newNotes);
                    fundooContext.SaveChanges();
                    return newNotes;
                }
                else
                {
                    return null;
                }
    
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity UpdateNote(Note note, long userId)
        {
            try
            {
                var Notes = fundooContext.NotesTable.Where(update => update.NotesID == userId).FirstOrDefault();
                if (note != null)
                {
                    Notes.Title = note.Title;
                    Notes.Description = note.Description;
                    Notes.Color = note.Color;
                    Notes.Image = note.Image;
                    Notes.ModifiedTime = note.ModifiedTime;
                    Notes.UserID = userId;
                    fundooContext.NotesTable.Update(Notes);
                    int result = fundooContext.SaveChanges();
                    return Notes;
                }

                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteNotes(Note note, long userId)
        {
            try
            {
                var result = fundooContext.NotesTable.Where(e => e.UserID == userId ).FirstOrDefault();

                if (result != null)
                {
                    fundooContext.NotesTable.Remove(result);
                    fundooContext.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<NotesEntity> ReadNotes(long userId)
        {
            try
            {
                var result = this.fundooContext.NotesTable.Where(x => x.UserID == userId);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool PinNotes(long noteId, long userId)
        {
            try
            {
                var result = fundooContext.NotesTable.Where(x => x.UserID == userId && x.NotesID == noteId).FirstOrDefault();

                if (result.Pin == true)
                {
                    result.Pin = false;
                    fundooContext.SaveChanges();
                    return false;
                }
                else
                {
                    result.Pin = true;
                    fundooContext.SaveChanges();
                    return true;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Archive(long noteId, long userId)
        {
            try
            {
                var result = fundooContext.NotesTable.Where(x => x.UserID == userId && x.NotesID == noteId).FirstOrDefault();

                if (result.Archive == false)
                {
                    result.Archive = true;
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    result.Archive = false;
                    fundooContext.SaveChanges();
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Trash(long noteId, long userId)
        {
            try
            {
                var result = fundooContext.NotesTable.Where(x => x.UserID == userId && x.NotesID == noteId).FirstOrDefault();

                if (result.Trash == false)
                {
                    result.Trash = true;
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    result.Trash = false;
                    fundooContext.SaveChanges();
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string AddImage(IFormFile image, long noteID, long userID)
        {
            try
            {
                var result = fundooContext.NotesTable.Where(x => x.UserID == userID && x.NotesID == noteID).FirstOrDefault();
                if (result != null)
                {
                    Account cloudaccount = new Account(
                        cloudinaryEntity["CloudinarySettings:cloud_name"],
                        cloudinaryEntity["CloudinarySettings:api_key"],
                        cloudinaryEntity["CloudinarySettings:api_secret"]
                        );
                    Cloudinary cloudinary = new Cloudinary(cloudaccount);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(image.FileName, image.OpenReadStream()),
                    };
                    var uploadResult = cloudinary.Upload(uploadParams);
                    string imagePath = uploadResult.Url.ToString();
                    result.Image = imagePath;
                    fundooContext.SaveChanges();
                    return "Image uploaded successfully";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

       
    }
}
