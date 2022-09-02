using BuisnessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Service
{
    public class CollaborationBL: ICollaborationBL
    {
        private readonly ICollaborationRL CollaborationRL;
        public CollaborationBL(ICollaborationRL CollaborationRL)
        {
            this.CollaborationRL = CollaborationRL;
        }

        public Collaboration AddCollab(long notesId, string Email)
        {
            try
            {

                return CollaborationRL.AddCollab(notesId, Email);
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

                return CollaborationRL.DeleteCollab(notesId, Email);

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

                return CollaborationRL.ReadCollaborate(Email);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
