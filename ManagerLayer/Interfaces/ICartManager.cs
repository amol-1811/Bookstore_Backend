using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface ICartManager
    {

        public CartModel getCartByUserId(int userId);

        public CartModel createCart(int userId);

        public CartModel addToCart(int userId, AddToCartModel addToCartModel);

        public ICollection<CartModel> getAllCarts();

        public CartModel removeFromCart(int userId, int cartItemId);

    }
}
