namespace Madrid.ProductReservations.Domain.Entities;
public interface IEntityBase
{
	Guid Id { get; }
}

public class EntityBase : IEntityBase
{
	public Guid Id { get; protected set; }

	protected EntityBase()
	{
	}

	protected EntityBase(Guid id)
	{
		this.Id = id;
	}
}
