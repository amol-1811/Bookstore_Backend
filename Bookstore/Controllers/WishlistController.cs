using System;
using System.Collections.Generic;
using System.Linq;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Models;

namespace Bookstore.Controllers
{
    [Route("api/wishlist")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistManager wishListManager;
        public WishlistController(IWishlistManager wishListManager)
        {
            this.wishListManager = wishListManager;
        }

        [HttpPost("{bookId}")]
        public IActionResult AddToWishlist(int bookId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var result = wishListManager.AddToWishlist(userId, bookId);

                if (result != null)
                {
                    return Ok(new ResponseModel<BookModel>
                    {
                        IsSuccess = true,
                        Message = "Book added to wishlist successfully.",
                        Data = result
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Message = "Book or user not found."
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding the book to wishlist.",
                    Data = ex.Message
                });
            }
        }

        [HttpGet("/get-wishlist")]
        public IActionResult GetWishlist()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var wishlist = wishListManager.GetWishlistByUserId(userId);

                if (wishlist == null || !wishlist.Any())
                {
                    return Ok(new ResponseModel<string>
                    {
                        IsSuccess = true,
                        Message = "Your wishlist is empty.",
                        Data = null
                    });
                }

                return Ok(new ResponseModel<List<BookModel>>
                {
                    IsSuccess = true,
                    Message = "Wishlist fetched successfully.",
                    Data = wishlist
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while fetching the wishlist.",
                    Data = ex.Message
                });
            }
        }

        [HttpDelete("{bookId}")]
        public IActionResult RemoveFromWishlist(int bookId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var removedBook = wishListManager.RemoveFromWishlist(userId, bookId);

                if (removedBook != null)
                {
                    return Ok(new ResponseModel<BookModel>
                    {
                        IsSuccess = true,
                        Message = "Book removed from wishlist.",
                        Data = removedBook
                    });
                }

                return NotFound(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "Wishlist item not found."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "Error removing from wishlist.",
                    Data = ex.Message
                });
            }
        }

    }
}
