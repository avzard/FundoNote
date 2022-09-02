using BuisnessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace BuisnessLayer.Service
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL LabelRL;
        public LabelBL(ILabelRL LabelRL)
        {
            this.LabelRL = LabelRL;
        }
        public LabelEntity AddLabel(long userId, long noteId, string labelName)
        {
            try
			{
                return LabelRL.AddLabel(userId,noteId, labelName);
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
                return LabelRL.DeleteLabel(labelId);
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
                return LabelRL.ReadLabel(labelId, userId);
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
                return LabelRL.UpdateLabel(labelId, labelname);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
