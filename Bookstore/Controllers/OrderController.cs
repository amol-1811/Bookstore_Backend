﻿using System;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager orderManager;
        public OrderController(IOrderManager orderManager)
        {
            this.orderManager = orderManager;
        }

        [HttpPost]
        public IActionResult PlaceOrderFromCart()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);

                var orders = orderManager.PlaceOrderFromCart(userId);

                if (orders == null || orders.Count == 0)
                {
                    return NotFound(new { IsSuccess = false, Message = "No items in the cart to place an order." });
                }

                return Ok(new { IsSuccess = true, Message = "Order placed successfully.", Data = orders });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = "An error occurred while placing the order.", Error = ex.Message });
            }
        }

        [HttpGet("get-order-details")]
        public IActionResult GetOrderDetails()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);

                var orderDetails = orderManager.GetOrderDetails(userId);

                if (orderDetails == null || orderDetails.Count == 0)
                {
                    return NotFound(new
                    {
                        IsSuccess = false,
                        Message = "No orders found for the user."
                    });
                }

                return Ok(new
                {
                    IsSuccess = true,
                    Message = "Order details fetched successfully.",
                    Data = orderDetails
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    IsSuccess = false,
                    Message = "An error occurred while fetching order details.",
                    Error = ex.Message
                });
            }
        }
    }
}
