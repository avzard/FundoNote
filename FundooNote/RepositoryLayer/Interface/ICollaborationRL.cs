using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICollaborationRL
    {
        public Collaboration AddCollab(long notesId, string Email);
        public string DeleteCollab(long notesId, string Email);
        public IEnumerable<Collaboration> ReadCollaborate(string Email);
    }
}
