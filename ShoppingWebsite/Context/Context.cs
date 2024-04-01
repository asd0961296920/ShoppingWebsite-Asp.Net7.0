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


        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductClass)
            .WithMany(pc => pc.Product)
            .HasForeignKey(p => p.product_class_id);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Manufacturer)
            .WithMany(pc => pc.Product)
            .HasForeignKey(p => p.manufacturer_id);

        modelBuilder.Entity<Item>()
            .HasOne(p => p.Manufacturer)
            .WithMany(pc => pc.Item)
            .HasForeignKey(p => p.manufacturer_id);
        modelBuilder.Entity<Item>()
            .HasOne(p => p.User)
            .WithMany(pc => pc.Item)
            .HasForeignKey(p => p.user_id);
        modelBuilder.Entity<Item>()
            .HasOne(p => p.Order)
            .WithMany(pc => pc.Item)
            .HasForeignKey(p => p.order_id) // 外键引用 Item 表的 order_id 列
            .HasPrincipalKey(o => o.order_number); // 指定 Order 表中的 order_number 列作为外键引用的目标列

        modelBuilder.Entity<Item>()
            .HasOne(p => p.Product)
            .WithMany(pc => pc.Item)
            .HasForeignKey(p => p.product_id);
        OnModelCreatingPartial(modelBuilder);
    }

}
