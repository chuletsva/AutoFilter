﻿using Microsoft.EntityFrameworkCore;

namespace Autofilter.Tests.Common.EF;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}