using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class AddToCartModel
    {
        private int bookId;
        private int quantity;

       public int BookId { get { return bookId; } set { bookId = value; } }
       public int Quantity { get { return quantity; } set { quantity = value; } }
    }
}
