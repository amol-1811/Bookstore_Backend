using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface IWishlistManager
    {
        public BookModel AddToWishlist(int userId, int bookId);
        public List<BookModel> GetWishlistByUserId(int userId);
        public BookModel RemoveFromWishlist(int userId, int bookId);
    }
}
