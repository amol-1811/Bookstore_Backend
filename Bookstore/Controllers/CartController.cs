using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Migrations;
using RepositoryLayer.Models;
using static RepositoryLayer.Models.UpdatecartModel;

namespace Bookstore.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        
        private readonly ICartManager _manager;

        public CartController(ICartManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public IActionResult createCart()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && (role == "Admin" || role == "User"))
                {
                    CartModel cartModel = _manager.createCart(userId);

                    return Ok(new ResponseModel<CartModel> { IsSuccess= true, Data = cartModel });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to create cart because => {ex.Message}");
            }

        }

        [HttpGet("/get-cart")]
        public IActionResult getCart()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && (role == "Admin" || role == "User"))
                {
                    CartModel cartModel = _manager.getCartByUserId(userId);

                    return Ok(new ResponseModel<CartModel> { IsSuccess=true, Data = cartModel });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"unable to get card for the user. {ex.Message}");
            }
        }


        [HttpPost("/add-to-cart")]
        public IActionResult addToCart([FromBody] AddToCartModel addToCartModel)
        {
            CartModel cartModel;
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && (role == "Admin" || role == "User"))
                {
                    cartModel = _manager.addToCart(userId, addToCartModel);
                    return Ok(new ResponseModel<CartModel> { IsSuccess = true, Data = cartModel });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("/update-cart")]
        public IActionResult updateCart([FromBody] UpdateCartModel updateCartModel)
        {
            CartModel cartModel;
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && (role == "Admin" || role == "User"))
                {
                    cartModel = _manager.updateCart(userId, updateCartModel);
                    return Ok(new ResponseModel<CartModel> { IsSuccess = true, Data = cartModel });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/remove-from-cart/{cartItemId}")]
        public IActionResult removeFromCart(int cartItemId)
        {
            CartModel cartModel;
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;

                if (role != null && (role == "Admin" || role == "User"))
                {
                    cartModel = _manager.removeFromCart(userId, cartItemId);

                    return Ok(new ResponseModel<CartModel> { IsSuccess = true, Data = cartModel });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("/get-all-carts")]
        public IActionResult getAllCarts()
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "Admin")
                {
                    ICollection<CartModel> allCarts = _manager.getAllCarts();
                    return Ok(new ResponseModel<ICollection<CartModel>> { IsSuccess = true, Data = allCarts });
                }
                else
                {
                    return Unauthorized("Only Admin can access all carts");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to fetch all carts: {ex.Message}");
            }
        }
    }
}
