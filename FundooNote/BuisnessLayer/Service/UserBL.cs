
using BuisnessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Service
{
    public class UserBL  : IUserBL
    {
        private readonly IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }
        public string Login(UserLoginModel userLogin)
        {
            try
            {
                return userRL.Login(userLogin);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UserEntity Register(UserRegistrationModel userRegistration)
        {
            try
            {
                return userRL.Register(userRegistration); 
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string ForgetPassword(string Email)
        {
            try
            {
                return userRL.ForgetPassword(Email);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool ResetPassword(string email, string password, string newPassword)
        {
            try
            {
                return userRL.ResetPassword(email, password, newPassword);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
