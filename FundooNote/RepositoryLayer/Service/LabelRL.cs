using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class LabelRL : ILabelRL
    {
        private readonly FundoContext fundooContext;

        public LabelRL(FundoContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public LabelEntity AddLabel(long userId, long noteId, string labelName)
        {
            try
            {
                LabelEntity label = new LabelEntity();
                label.UserID = userId;
                label.NotesID = noteId;
                label.LabelName = labelName;
                fundooContext.Add(label);
                int res = fundooContext.SaveChanges();
                if (res > 0)
                {
                    return label;
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
        public string DeleteLabel(long labelId)
        {
            try
            {
                var noteResult = fundooContext.LabelTable.Where(x => x.LabelId == labelId).FirstOrDefault();
                if (noteResult != null)

                {
                    fundooContext.LabelTable.Remove(noteResult);
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
        public LabelEntity ReadLabel(long labelId, long userId)
        {
            try
            {
                var UserId = this.fundooContext.UserTable.Where(e => e.UserID == userId);
                if (UserId != null)
                {
                    return this.fundooContext.LabelTable.FirstOrDefault(e => e.LabelId == labelId);
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public LabelEntity UpdateLabel(long labelId, string labelname)
        {
            try
            {

                var label = fundooContext.LabelTable.Where(x => x.LabelId == labelId).FirstOrDefault();
                if (label != null)
                {

                    label.LabelName = labelname;


                    this.fundooContext.SaveChanges();
                    return label;
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
    }
}
