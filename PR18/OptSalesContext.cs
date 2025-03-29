using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PR18;

public partial class OptSalesContext : DbContext
{
    public OptSalesContext()
    {
    }

    public OptSalesContext(DbContextOptions<OptSalesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OptSale> OptSales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAB44-11\\SQLEXPRESS; User=ИСП-34; Password=1234567890; Database=OptSales ;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OptSale>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOfReceipt).HasColumnType("datetime");
            entity.Property(e => e.DateSellBatch).HasColumnType("datetime");
            entity.Property(e => e.NameCompany).HasMaxLength(50);
            entity.Property(e => e.NameProduct).HasMaxLength(50);
            entity.Property(e => e.PriceBatch).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PriceProduct).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SizeBatch).HasMaxLength(50);
            entity.Property(e => e.SizeSellBatch).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
