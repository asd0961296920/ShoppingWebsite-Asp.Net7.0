using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace TextContext;

public partial class Context : DbContext
{

    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<Item> Item { get; set; }
    public virtual DbSet<Manufacturer> Manufacturer { get; set; }
    public virtual DbSet<Order> Order { get; set; }
    public virtual DbSet<Product> Product { get; set; }
    public virtual DbSet<ProductClass> ProductClass { get; set; }
    public virtual DbSet<Shop> Shop { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder = HasCharSet(modelBuilder);


        OnModelCreatingPartial(modelBuilder);
    }

}
