
using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Service
{
    public class UserRL: IUserRL
    {
        private readonly FundoContext fundoContext;
        private readonly IConfiguration _Appsettings;
        public UserRL(FundoContext fundoContext, IConfiguration _Appsettings)
        {
            this.fundoContext = fundoContext;
            this._Appsettings = _Appsettings;
        }
        public UserEntity Register(UserRegistrationModel userRegistration)
        {
            try
            {
                UserEntity user = new UserEntity();
                user.FirstName=userRegistration.FirstName;
                user.LastName=userRegistration.LastName;
                user.Email=userRegistration.Email;
                user.Password=userRegistration.Password;
                fundoContext.UserTable.Add(user);
                int res = fundoContext.SaveChanges();
                if (res != 0)
                {
                    return user;
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
        public string Login(UserLoginModel userLogin  )
        {
            try
            {
                var LoginDetails = fundoContext.UserTable.Where(x => x.Email == userLogin.Email && x.Password == userLogin.Password).FirstOrDefault();
                if (LoginDetails != null)
                {
                    var token = GenerateSecurityToken(LoginDetails.Email, LoginDetails.UserID);
                    return token;
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
        private string GenerateSecurityToken(string Email, long UserID)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._Appsettings[("JWT:key")]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, Email),
                    new Claim("UserID", UserID.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        public string ForgetPassword(string Email)
        {
            try
            {
                var EmailCheck = fundoContext.UserTable.FirstOrDefault(x => x.Email == Email);
                if (EmailCheck != null)
                {
                    string token = GenerateSecurityToken(EmailCheck.Email, EmailCheck.UserID);
                    MSMQModel msm = new MSMQModel();
                    msm.sendData2Queue(token);
                    return "Mail Sent";
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
        public bool ResetPassword(string email, string password, string newPassword)
        {
            try
            {

                if (password.Equals(newPassword))
                {
                    var user = this.fundoContext.UserTable.Where(E => E.Email == email).FirstOrDefault();
                    user.Password = newPassword;
                    fundoContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
