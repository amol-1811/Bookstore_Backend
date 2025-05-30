using System;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using RepositoryLayer.Services;

namespace Bookstore.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager _adminManager;
        public AdminController(IAdminManager adminManager)
        {
            _adminManager = adminManager;
        }

        [HttpPost]
        public  IActionResult Register([FromBody] AdminRegisterModel model)
        {
            try
            {
                var result = _adminManager.Register(model);
                if(result != null)
                {
                    return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "Admin registered successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Admin already exists" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AdminLoginModel model)
        {
            try
            {
                var result = _adminManager.Login(model, "Admin");
                if (result != null)
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Admin logged in successfully", Data = result });
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

                var result = _adminManager.ForgotPassword(email);
                if (result == null)
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Email Not Found" });
                }
                else
                {
                    SendEmail send = new SendEmail();
                    send.EmailSend(result.Email, result.Token);
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Reset password link Sent to Email" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                { IsSuccess = false, Message = "An internal error occurred", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("resetpassword")]

        public IActionResult AdminResetPassword(ResetPasswordModel model)
        {
            try
            {
                string Email = User.FindFirst("Email").Value;
                if (_adminManager.ResetPassword(Email, model))
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Password reset successfully" });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Password reset failed" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                { IsSuccess = false, Message = "An internal error occurred", Data = ex.Message });
            }
        }

    }
}
