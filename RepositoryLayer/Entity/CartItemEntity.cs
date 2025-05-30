using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class CartItemEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }

        public BookEntity Book { get; set; }
        public CartEntity Cart { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }

        [ForeignKey("Books")]
        public int BookId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        public bool IsPurchased { get; set; }

        [NotMapped]
        private decimal totalPrice => Quantity * Price;

        public decimal TotalPrice { get { return totalPrice; } }
    }
}
