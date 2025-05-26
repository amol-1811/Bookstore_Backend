using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        public UserResponseModel Register(UserRegisterModel model);
        public UserLoginResponseModel Login(UserLoginModel model);
        public ForgotPasswordModel ForgotPassword(string email);
        public bool ResetPassword(string email, ResetPasswordModel model);
    }
}
