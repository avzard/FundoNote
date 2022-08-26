using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Interface
{
    public interface INoteBL
    {
        public NotesEntity CreateNote(Note note, long userId);
        public NotesEntity UpdateNote(Note note, long userId);
        public bool DeleteNotes(Note note, long userId);
        public IEnumerable<NotesEntity> ReadNotes(long userId);
        public bool PinNotes(long noteId, long userId);
        public bool Archive(long noteId, long userId);
        public bool Trash(long noteId, long userId);
        public string AddImage(IFormFile image, long noteID, long userID);
    }
}
