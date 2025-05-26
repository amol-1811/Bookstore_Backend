using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepo _userRepo;

        public UserManager(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public UserResponseModel Register(UserRegisterModel model)
        {
            return _userRepo.Register(model);
        }

        public UserLoginResponseModel Login(UserLoginModel model)
        {
            return _userRepo.Login(model);
        }

        public ForgotPasswordModel ForgotPassword(string email)
        {
            return _userRepo.ForgotPassword(email);
        }

        public bool ResetPassword(string email, ResetPasswordModel model)
        {
            return _userRepo.ResetPassword(email, model);
        }
    }
}
