using Madrid.ProductReservations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Madrid.ProductReservations.EntityFrameworkCore.PostgreSql;
public class ReservationDbContext : DbContext
{
	public DbSet<Product> Products { get; set; }
	public DbSet<ProductReservation> ProductReservations { get; set; }
	public DbSet<ProductResidualStock> ProductResidualStocks { get; set; }

	public ReservationDbContext(DbContextOptions<ReservationDbContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Product>(e =>
		{
			e.Property(x => x.Id).ValueGeneratedNever();
		});

		modelBuilder.Entity<ProductReservation>(e =>
		{
			e.Property(x => x.Id).ValueGeneratedNever();

			e.HasIndex(x => new { x.ProductId, x.WarehouseId })
				.IsUnique();

			e.Property(p => p.ConcurrencyToken)
				.IsConcurrencyToken();
		});

		modelBuilder.Entity<ProductResidualStock>(e =>
		{
			e.Property(x => x.Id).ValueGeneratedNever();

			e.HasIndex(x => new { x.ProductId, x.WarehouseId })
				.IsUnique();
		});
	}
}
