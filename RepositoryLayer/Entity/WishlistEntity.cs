using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class WishlistEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishListId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [ForeignKey("Books")]
        public int BookId { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual BookEntity Book { get; set; }
    }
}
