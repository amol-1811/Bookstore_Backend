using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class CartItemModel
    {
        private int cartItemId;
        private int quantity;
        private decimal totalPrice;
        private decimal price;
        private decimal discountPrice;
        private BookModel bookModel;

        public int CartItemId { get { return cartItemId; } set { cartItemId = value; } }

        public int Quantity { get { return quantity; } set { quantity = value; } }

        public decimal TotalPrice { get { return totalPrice; } set { totalPrice = value; } }

        public decimal Price { get { return price; } set { price = value; } }

        public decimal DiscountPrice { get {return discountPrice; } set { discountPrice = value; } }

        public BookModel BookModel { get { return bookModel; } set { bookModel = value; } }



    }
}
