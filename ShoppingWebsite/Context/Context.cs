using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace TextContext;

public partial class Context : DbContext
{

    //public virtual DbSet<User> User { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder = HasCharSet(modelBuilder);

        //每張表的基礎設定和類型設定
        //modelBuilder.Entity<User>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PRIMARY");

        //    entity.ToTable("user");


        //    //entity.Property(e => e.Id)
        //    //.ValueGeneratedNever()
        //    //.HasColumnType("int(11)")
        //    //.HasColumnName("id");
        //    //entity.Property(e => e.Name)
        //    //    .HasMaxLength(255)
        //    //    .HasColumnName("name");
        //    //entity.Property(e => e.Password)
        //    //    .HasMaxLength(255)
        //    //    .HasColumnName("password");
        //});

        OnModelCreatingPartial(modelBuilder);
    }

}
