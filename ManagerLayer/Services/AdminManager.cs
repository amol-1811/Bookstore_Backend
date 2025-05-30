using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class AdminManager : IAdminManager
    {
        private readonly IAdminRepo _adminRepo;
        public AdminManager(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }
        public UserEntity Register(AdminRegisterModel model)
        {
            return _adminRepo.Register(model);
        }
        public string Login(AdminLoginModel model, string role)
        {
            return _adminRepo.Login(model, role);
        }
        public ForgotPasswordModel ForgotPassword(string email)
        {
            return _adminRepo.ForgotPassword(email);
        }
        public bool ResetPassword(string email, ResetPasswordModel model)
        {
            return _adminRepo.ResetPassword(email, model);
        }
    }
}
