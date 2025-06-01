using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Models
{
    public class AddToCartModel
    {
        private int bookId;
        private int quantity;

       public int BookId { get { return bookId; } set { bookId = value; } }

        [Required]
        [Range(1, 1, ErrorMessage = "Only 1 quantity is allowed when adding to cart. Use update-cart API for multiple quantities.")]
        public int Quantity { get { return quantity; } set { quantity = value; } }
    }
}
