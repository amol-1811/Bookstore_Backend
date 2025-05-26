using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.EncodePassword;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;   
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class AdminRepo : IAdminRepo
    {
        private readonly BookstoreDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public AdminRepo(IConfiguration _configuration, BookstoreDBContext _dbContext, TokenService _tokenService)
        {
            this._dbContext = _dbContext;
            this._configuration = _configuration;
            this._tokenService = _tokenService;
        }
        
        public AdminEntity Register(AdminRegisterModel model)
        {
            var admin = new AdminEntity
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = EncryptPass.EncodePasswordToBase64(model.Password),
                PhoneNumber = model.PhoneNumber,
                Role = "Admin"
            };

            _dbContext.Admins.Add(admin);
            _dbContext.SaveChanges();
            return admin;
        }

        public string Login(AdminLoginModel model)
        {
            var admin = _dbContext.Admins.FirstOrDefault(x => x.Email == model.Email && x.Password == EncryptPass.EncodePasswordToBase64(model.Password));
            if (admin != null)
            {
                var token = _tokenService.GenerateToken(admin.Email, admin.AdminId, admin.Role);
                return token;
            }
            else
            {
                return null;
            }
        }
        public ForgotPasswordModel ForgotPassword(string email)
        {
            var admin = _dbContext.Admins.ToList().Find(admin => admin.Email == email);
            if (admin == null)
            {
                return null;
            }
            else
            {
                var token = _tokenService.GenerateToken(admin.Email, admin.AdminId, admin.Role);
                ForgotPasswordModel adminForgotPasswordModel = new ForgotPasswordModel();
                adminForgotPasswordModel.Email = email;
                adminForgotPasswordModel.Token = token;
                return adminForgotPasswordModel;
            }
        }

        public bool ResetPassword(string email, ResetPasswordModel model)
        {
            var admin = _dbContext.Admins.ToList().Find(admin => admin.Email == email);
            if (admin == null)
            {
                return false;
            }
            else
            {
                admin.Password = EncryptPass.EncodePasswordToBase64(model.ConfirmPassword);
                _dbContext.Admins.Update(admin);
                _dbContext.SaveChanges();
                return true;
            }
        }


    }
}
