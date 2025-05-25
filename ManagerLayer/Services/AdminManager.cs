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
        public AdminEntity Register(AdminModel model)
        {
            return _adminRepo.Register(model);
        }
        public string Login(AdminModel model)
        {
            return _adminRepo.Login(model);
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
