using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface IAdminManager
    {
        public AdminEntity Register(AdminModel model);
        public string Login(AdminModel model);
        public ForgotPasswordModel ForgotPassword(string email);
        public bool ResetPassword(string email, ResetPasswordModel model);
    }
}
