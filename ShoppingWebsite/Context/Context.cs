using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace TextContext;

public partial class Context : DbContext
{

    public virtual DbSet<Product> User { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder = HasCharSet(modelBuilder);


        OnModelCreatingPartial(modelBuilder);
    }

}
