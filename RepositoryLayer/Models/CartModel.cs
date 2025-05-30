using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class CartModel
    {
        private int id;

        private decimal total;

        private ICollection<CartItemModel> cartItems = new List<CartItemModel>();

        public int Id { get { return id; } set { id = value; } }

        public decimal Total { get { return total; } set { total = value; } }

        public ICollection<CartItemModel> CartItems { get { return cartItems; } set { cartItems = value; } }

    }
}
