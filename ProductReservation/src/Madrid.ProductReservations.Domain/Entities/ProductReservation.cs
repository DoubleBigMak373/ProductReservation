namespace Madrid.ProductReservations.Domain.Entities;
public class ProductReservation : EntityBase
{
	public Guid ProductId { get; private set; }
	public Guid WarehouseId { get; private set; }
	public decimal Quantity { get; private set; }

	public Guid ConcurrencyToken { get; set; }

	protected ProductReservation()
	{
	}

	public ProductReservation(
		Guid id,
		Guid productId,
		Guid warehouseId,
		decimal quantity)
		: base(id)
	{
		Id = id;
		ProductId = productId;
		WarehouseId = warehouseId;
		ChangeReservationQuantity(quantity);

		ConcurrencyToken = Guid.NewGuid();
	}

	public void ChangeReservationQuantity(decimal value)
	{
		if (value < 0)
		{
			throw new ArgumentException("Нельзя создать бронирование на отрицательное число товара", nameof(value));
		}

		Quantity = value;
	}
}
