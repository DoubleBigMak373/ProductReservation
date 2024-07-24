using Madrid.ProductReservations.Domain.Entities;

namespace Madrid.ProductReservations.Domain.Repositories;

public interface IProductReservationRepository : ICommonRepository<ProductReservation>
{
	Task<ProductReservation?> FindAsync(Guid productId, Guid warehouseId, CancellationToken cancellationToken = default);
}
