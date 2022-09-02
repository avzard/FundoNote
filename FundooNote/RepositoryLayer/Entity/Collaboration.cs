using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class Collaboration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CollabId { get; set; }
        public string CollabEmail { get; set; }

        [ForeignKey("user")]
        public long UserID { get; set; }
        public virtual UserEntity user { get; set; }

        [ForeignKey("notes")]
        public long NotesID { get; set; }

        public virtual NotesEntity notes { get; set; }


    }
}