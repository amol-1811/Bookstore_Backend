using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class WishlistRepo : IWishlistRepo
    {
        private readonly BookstoreDBContext context;
        private readonly IConfiguration configuration;

        public WishlistRepo(BookstoreDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public BookModel AddToWishlist(int userId, int bookId)
        {
            try
            {
                var user = context.Users.Find(userId);
                var book = context.Books.Find(bookId);

                if (user == null || book == null)
                {
                    throw new Exception("Invalid user or book ID.");
                }

                var existingWishlist = context.Wishlist
                    .FirstOrDefault(w => w.UserId == userId && w.BookId == bookId);

                if (existingWishlist != null)
                {
                    throw new Exception("Book already exists in wishlist.");
                }

                var wishlist = new WishlistEntity
                {
                    UserId = userId,
                    BookId = bookId
                };

                context.Wishlist.Add(wishlist);
                context.SaveChanges();

                return new BookModel
                {
                    BookName = book.BookName,
                    Author = book.Author,
                    Description = book.Description,
                    DiscountPrice = book.DiscountPrice,
                    Price = book.Price
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding to wishlist: " + ex.Message);
            }
        }

        public List<BookModel> GetWishlistByUserId(int userId)
        {
            try
            {
                var wishlistItems = context.Wishlist
                    .Include(w => w.Book)
                    .Where(w => w.UserId == userId)
                    .ToList();

                if (wishlistItems == null || !wishlistItems.Any())
                {
                    return new List<BookModel>();
                }

                return wishlistItems.Select(w => new BookModel
                {
                    BookId = w.Book.BookId,
                    BookName = w.Book.BookName,
                    Author = w.Book.Author,
                    Description = w.Book.Description,
                    DiscountPrice = w.Book.DiscountPrice,
                    Price = w.Book.Price
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving wishlist: " + ex.Message);
            }
        }

        public BookModel RemoveFromWishlist(int userId, int bookId)
        {
            try
            {
                var item = context.Wishlist
                    .Include(w => w.Book)
                    .FirstOrDefault(w => w.UserId == userId && w.BookId == bookId);

                if (item == null)
                    return null;

                var book = item.Book;
                context.Wishlist.Remove(item);
                context.SaveChanges();

                return new BookModel
                {
                    BookId = book.BookId,
                    BookName = book.BookName,
                    Author = book.Author,
                    Description = book.Description,
                    DiscountPrice = book.DiscountPrice,
                    Price = book.Price
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while removing wishlist: " + ex.Message);
            }
        }
    }
}
