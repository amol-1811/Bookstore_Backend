using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class OrdersEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [ForeignKey("Books")]
        public int BookId { get; set; }

        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; }
        public UserEntity User { get; set; }
        public BookEntity Book { get; set; }
    }
}
