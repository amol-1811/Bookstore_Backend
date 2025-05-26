using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IAdminRepo
    {
        AdminEntity Register(AdminRegisterModel model);
        string Login(AdminLoginModel model);
        ForgotPasswordModel ForgotPassword(string email);
        bool ResetPassword(string email, ResetPasswordModel model);
    }
}
