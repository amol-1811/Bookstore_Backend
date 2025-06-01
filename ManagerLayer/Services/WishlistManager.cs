using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class WishlistManager : IWishlistManager
    {
        private readonly IWishlistRepo _wishlistRepo;

        public WishlistManager(IWishlistRepo wishlistRepo)
        {
            _wishlistRepo = wishlistRepo;
        }
        public BookModel AddToWishlist(int userId, int bookId)
        {
            return _wishlistRepo.AddToWishlist(userId, bookId);
        }

        public List<BookModel> GetWishlistByUserId(int userId)
        {
            return _wishlistRepo.GetWishlistByUserId(userId);
        }

        public BookModel RemoveFromWishlist(int userId, int bookId)
        {
            return _wishlistRepo.RemoveFromWishlist(userId, bookId);
        }
    }
}
