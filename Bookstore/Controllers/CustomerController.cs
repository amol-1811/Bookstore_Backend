using System;
using System.Collections.Generic;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerManager customerManager;

        public CustomerController(ICustomerManager customerManager)
        {
            this.customerManager = customerManager;
        }

        [HttpPost]
        public IActionResult AddCustomerDetails(CustomerDetailModel model)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;

                if (role != null && (role == "Admin" || role == "User"))
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(new ResponseModel<string>
                        {
                            IsSuccess = false,
                            Message = "Invalid input. Please check the entered details.",
                            Data = null
                        });
                    }

                    var customer = customerManager.AddCustomerDetails(userId, model);

                    return Ok(new ResponseModel<CustomerEntity>
                    {
                        IsSuccess = true,
                        Message = "Customer details added successfully.",
                        Data = customer
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Message = "Access denied: Only authenticated users can add customer details."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding customer details.",
                    Data = ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult GetAllCustomerDetails()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;

                if (role != null && (role == "Admin" || role == "User"))
                {
                    var customerList = customerManager.GetAllCustomerDetails();

                    if (customerList == null || customerList.Count == 0)
                    {
                        return NotFound(new ResponseModel<string>
                        {
                            IsSuccess = false,
                            Message = "No customer details found.",
                            Data = null
                        });
                    }

                    return Ok(new ResponseModel<List<CustomerResponseModel>>
                    {
                        IsSuccess = true,
                        Message = "Customer details fetched successfully.",
                        Data = customerList
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Message = "Access denied: Only authenticated users can view customer details.",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving customer details.",
                    Data = ex.Message
                });
            }
        }
    }
}
