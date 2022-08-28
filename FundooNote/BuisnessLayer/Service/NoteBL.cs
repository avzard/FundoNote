using BuisnessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace BuisnessLayer.Service
{
    public class NoteBL: INoteBL
    {
        private readonly INoteRL NoteRL;
        public NoteBL(INoteRL NoteRL)
        {
            this.NoteRL = NoteRL;
        }

        public NotesEntity CreateNote(Note note, long userId)
        {
            try
            {
                return NoteRL.CreateNote(note, userId);
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
                return NoteRL.UpdateNote(note, userId);
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
                return NoteRL.DeleteNotes(note, userId);
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
                return NoteRL.ReadNotes(userId);
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
                return NoteRL.PinNotes(userId, noteId);
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
                return NoteRL.Archive(userId, noteId);
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
                return NoteRL.Trash(userId, noteId);
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
                return NoteRL.AddImage(image, noteID, userID);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public NotesEntity Color(long noteID, string color)
        {
            try
            {
                return NoteRL.Color(noteID, color);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
