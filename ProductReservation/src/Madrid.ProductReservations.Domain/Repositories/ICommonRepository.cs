using Madrid.ProductReservations.Domain.Entities;

namespace Madrid.ProductReservations.Domain.Repositories;
public interface ICommonRepository<TEntity>
	where TEntity : class, IEntityBase
{
	Task<TEntity> InsertAsync(TEntity entity, bool saveChanges = false, CancellationToken cancellationToken = default);
	Task<TEntity> UpdateAsync(TEntity entity, bool saveChanges = false, CancellationToken cancellationToken = default);
	Task<TEntity> DeleteAsync(Guid id, bool saveChanges = false, CancellationToken cancellationToken = default);
}
