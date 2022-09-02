using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Interface
{
    public interface ICollaborationBL
    {
        public Collaboration AddCollab(long notesId, string Email);
        public string DeleteCollab(long notesId, string Email);
        public IEnumerable<Collaboration> ReadCollaborate(string Email);
    }
}
