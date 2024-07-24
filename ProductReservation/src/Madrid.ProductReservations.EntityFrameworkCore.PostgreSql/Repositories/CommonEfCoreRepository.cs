using Madrid.ProductReservations.Domain.Entities;
using Madrid.ProductReservations.Domain.Repositories;

namespace Madrid.ProductReservations.EntityFrameworkCore.PostgreSql.Repositories;
public class CommonEfCoreRepository<TEntity>(ReservationDbContext dbContext) : ICommonRepository<TEntity>
	where TEntity : class, IEntityBase
{
	protected readonly ReservationDbContext DbContext = dbContext;

	public async Task<TEntity> InsertAsync(TEntity entity, bool saveChanges = false, CancellationToken cancellationToken = default)
	{
		var added = DbContext.Set<TEntity>().Add(entity);

		if (saveChanges)
		{
			await DbContext.SaveChangesAsync(cancellationToken);
		}

		return added.Entity;
	}

	public async Task<TEntity> UpdateAsync(TEntity entity, bool saveChanges = false, CancellationToken cancellationToken = default)
	{
		var updated = DbContext.Set<TEntity>().Update(entity);

		if (saveChanges)
		{
			await DbContext.SaveChangesAsync(cancellationToken);
		}

		return updated.Entity;
	}

	public async Task<TEntity> DeleteAsync(Guid id, bool saveChanges = false, CancellationToken cancellationToken = default)
	{
		var dbSet = DbContext.Set<TEntity>();

		var deleted = await dbSet.FindAsync([id], cancellationToken);

		if (deleted is null)
			throw new InvalidOperationException($"Entity not found. Id: {id}");

		dbSet.Remove(deleted);

		if (saveChanges)
		{
			await DbContext.SaveChangesAsync(cancellationToken);
		}

		return deleted;
	}
}
