using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class LabelModel
    {
        public long LabelId { get; set; }
        public string LabelName { get; set; }
        public long NotesID { get; set; }

        public long UserID { get; set; }
    }
}
