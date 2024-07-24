using Madrid.ProductReservations.Domain.Entities;

namespace Madrid.ProductReservations.Domain.Repositories;
public interface IProductResidualStockRepository : ICommonRepository<ProductResidualStock>
{
	Task<ProductResidualStock?> FindAsync(Guid productId, Guid warehouseId, CancellationToken cancellationToken = default);
}
