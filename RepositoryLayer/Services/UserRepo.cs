using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.EncodePassword;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly BookstoreDBContext _dbContext;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;

        public UserRepo(BookstoreDBContext dbContext, TokenService tokenService, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _configuration = configuration;
        }
        public bool CheckEmail(string email)
        {
            var result = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            if (result == null)
            {
                return false;
            }
            return true;
        }
        public UserResponseModel Register(UserRegisterModel model)
        {
            var user = new UserEntity
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = EncryptPass.EncodePasswordToBase64(model.Password),
                PhoneNumber = model.PhoneNumber,
                Role = "User"
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return new UserResponseModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public UserLoginResponseModel Login(UserLoginModel model)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == EncryptPass.EncodePasswordToBase64(model.Password));
            if (user == null)
            {
                return null;
            }
                _dbContext.SaveChanges();

                return new UserLoginResponseModel
                {
                    Token = _tokenService.GenerateToken(user.Email, user.UserId, user.Role),
                    FullName = user.FullName,
                    Email = user.Email,
                };
        }

        public ForgotPasswordModel ForgotPassword(string email)
        {
            var user = _dbContext.Users.ToList().Find(user => user.Email == email);
            if (user == null)
            {
                return null;
            }
            else
            {
                var token = _tokenService.GenerateToken(user.Email, user.UserId, user.Role);
                ForgotPasswordModel forgotPassword = new ForgotPasswordModel();
                forgotPassword.Email = email;
                forgotPassword.Token = token;
                return forgotPassword;
            }
        }

        public bool ResetPassword(string email, ResetPasswordModel model)
        {
            var user = _dbContext.Users.ToList().Find(user => user.Email == email);
            if (user == null)
            {
                return false;
            }
            else
            {
                user.Password = EncryptPass.EncodePasswordToBase64(model.ConfirmPassword);
                _dbContext.SaveChanges();
                return true;
            }
        }

    }
}