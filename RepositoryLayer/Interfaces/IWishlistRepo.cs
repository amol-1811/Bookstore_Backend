using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IWishlistRepo
    {
        public BookModel AddToWishlist(int userId, int bookId);
        public List<BookModel> GetWishlistByUserId(int userId);
        public BookModel RemoveFromWishlist(int userId, int bookId);
    }
}
