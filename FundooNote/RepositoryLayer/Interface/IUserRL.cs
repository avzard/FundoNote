
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
       
        public UserEntity Register(UserRegistrationModel userRegistration);
        public string Login(UserLoginModel userLogin);
        public string ForgetPassword(string Email);
        public bool ResetPassword(string email, string password, string newPassword);


    }
}
