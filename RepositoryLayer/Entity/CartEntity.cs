using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class CartEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        public virtual UserEntity User { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public ICollection<CartItemEntity> CartItems { get; set; } = new List<CartItemEntity>();

        public int TotalPrice { get; set; }
    }
}
