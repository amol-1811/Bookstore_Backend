using System;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Models;
using RepositoryLayer.Services;

namespace Bookstore.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly TokenService _tokenService;

        public UserController(IUserManager userManager, TokenService tokenService)
        {
            this.userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        public IActionResult Register([FromBody] UserRegisterModel model)
        {
            try
            {
                var result = userManager.Register(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<UserResponseModel> { IsSuccess = true, Message = "User registered successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "User already exists" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel model)
        {
            try
            {
                var result = userManager.Login(model, "User");

                if (result != null)
                {
                    return Ok(new ResponseModel<UserLoginResponseModel> { IsSuccess = true, Message = "User logged in successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Invalid credentials" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("forgotpassword")]
        public IActionResult ForgetPassword(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Email is required" });
                ForgotPasswordModel result = userManager.ForgotPassword(email);

                if (result == null)
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Email not existed Please Enter valid E-mail" });
                }
                else
                {
                    SendEmail send = new SendEmail();
                    send.EmailSend(result.Email, result.Token);
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Reset Password link Sent to Email" });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new{ message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("resetpassword")]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {
                string Email = User.FindFirst("Email").Value;
                if (userManager.ResetPassword(Email, model))
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = Email + " - Password changed Success" });
                }
                return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = " - Password Not Changed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

