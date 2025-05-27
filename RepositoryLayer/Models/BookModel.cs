﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Models
{
    public class BookModel
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "Discount Price is required")]
        public decimal DiscountPrice { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Book Image is required")]
        public string BookImage { get; set; } = null!;

        [Required(ErrorMessage = "Book Name is required")]
        public string BookName { get; set; } = null!;

        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; } = null!;
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
        public bool IsPurchased { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
