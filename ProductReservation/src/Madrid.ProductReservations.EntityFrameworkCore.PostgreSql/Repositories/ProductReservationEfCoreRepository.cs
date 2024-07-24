using Madrid.ProductReservations.Domain.Entities;
using Madrid.ProductReservations.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Madrid.ProductReservations.EntityFrameworkCore.PostgreSql.Repositories;
public class ProductReservationEfCoreRepository : CommonEfCoreRepository<ProductReservation>, IProductReservationRepository
{
	public ProductReservationEfCoreRepository(ReservationDbContext dbContext)
		: base(dbContext)
	{
	}

	public Task<ProductReservation?> FindAsync(Guid productId, Guid warehouseId, CancellationToken cancellationToken = default)
	{
		return DbContext.Set<ProductReservation>()
			.FirstOrDefaultAsync(
				x => x.WarehouseId == warehouseId && x.ProductId == productId,
				cancellationToken);
	}
}
