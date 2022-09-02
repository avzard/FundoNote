using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Interface
{
    public interface ILabelBL
    {
        public LabelEntity AddLabel(long userId, long noteId, string labelName);
        public string DeleteLabel(long labelId);
        public LabelEntity ReadLabel(long labelId, long userId);
        public LabelEntity UpdateLabel(long labelId, string labelname);
    }
}
