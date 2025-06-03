using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class BookstoreDBContext : DbContext
    {
        public BookstoreDBContext(DbContextOptions option) : base(option) { }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<CartEntity> Cart { get; set; }
        public DbSet<CartItemEntity> CartItems { get; set; }
        public DbSet<WishlistEntity> Wishlist { get; set; }
        public DbSet<CustomerEntity> Customer { get; set; }
        public DbSet<OrdersEntity> Orders { get; set; }
    }
}
