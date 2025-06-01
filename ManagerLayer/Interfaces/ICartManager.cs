using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;
using static RepositoryLayer.Models.UpdatecartModel;

namespace ManagerLayer.Interfaces
{
    public interface ICartManager
    {

        public CartModel getCartByUserId(int userId);

        public CartModel createCart(int userId);

        public CartModel addToCart(int userId, AddToCartModel addToCartModel);
        CartModel updateCart(int userId, UpdateCartModel updateCartModel);

        public ICollection<CartModel> getAllCarts();

        public CartModel removeFromCart(int userId, int cartItemId);

    }
}
