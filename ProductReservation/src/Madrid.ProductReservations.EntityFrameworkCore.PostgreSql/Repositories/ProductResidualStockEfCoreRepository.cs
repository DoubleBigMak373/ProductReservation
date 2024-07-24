using Madrid.ProductReservations.Domain.Entities;
using Madrid.ProductReservations.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Madrid.ProductReservations.EntityFrameworkCore.PostgreSql.Repositories;
public class ProductResidualStockEfCoreRepository : CommonEfCoreRepository<ProductResidualStock>, IProductResidualStockRepository
{
	public ProductResidualStockEfCoreRepository(ReservationDbContext dbContext)
		: base(dbContext)
	{
	}

	public Task<ProductResidualStock?> FindAsync(Guid productId, Guid warehouseId, CancellationToken cancellationToken = default)
	{
		return DbContext.Set<ProductResidualStock>()
			.FirstOrDefaultAsync(
				x => x.WarehouseId == warehouseId && x.ProductId == productId,
				cancellationToken);
	}
}
