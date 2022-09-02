using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class LabelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LabelId { get; set; }
        public string LabelName { get; set; }
        [ForeignKey("user")]
        public long UserID { get; set; }
        public virtual UserEntity user { get; set; }

        [ForeignKey("notes")]
        public long NotesID { get; set; }

        public virtual NotesEntity notes { get; set; }
    }
}
