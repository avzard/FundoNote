using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class CollaborationRL : ICollaborationRL
    {
        private readonly FundoContext fundooContext;
        Collaboration collaboration = new Collaboration();

        public CollaborationRL(FundoContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public Collaboration AddCollab(long notesId, string Email)
        {
            try
            {
                var noteResult = fundooContext.NotesTable.Where(x => x.NotesID == notesId).FirstOrDefault();
                var emailResult = fundooContext.UserTable.Where(x => x.Email == Email).FirstOrDefault();
                if (noteResult != null && emailResult != null)
                {
                    Collaboration collabEntity = new Collaboration();
                    collabEntity.NotesID = noteResult.NotesID;
                    collabEntity.CollabEmail = emailResult.Email;
                    collabEntity.UserID = emailResult.UserID;
                    fundooContext.Add(collabEntity);
                    fundooContext.SaveChanges();
                    return collabEntity;
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
        public string DeleteCollab(long notesId, string Email)
        {

            try
            {
                var noteResult = fundooContext.CollabTable.Where(x => x.NotesID == notesId && x.CollabEmail == Email).FirstOrDefault();
                if (noteResult != null)

                {
                    fundooContext.CollabTable.Remove(noteResult);
                    this.fundooContext.SaveChanges();
                    return "Delete successfully";
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
        
        public IEnumerable<Collaboration> ReadCollaborate(string Email)
        {
            try
            {
                var result = this.fundooContext.CollabTable.Where(x => x.CollabEmail == Email);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
