using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class CartItemModel
    {
        private int cartItemId;
        private int quantity;
        private decimal price;
        private BookModel bookModel;

        public int CartItemId { get { return cartItemId; } set { cartItemId = value; } }

        public int Quantity { get { return quantity; } set { quantity = value; } }

        public decimal Price { get { return price; } set { price = value; } }

        public BookModel BookModel { get { return bookModel; } set { bookModel = value; } }



    }
}
