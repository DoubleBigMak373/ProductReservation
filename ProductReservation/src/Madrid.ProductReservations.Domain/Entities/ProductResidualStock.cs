namespace Madrid.ProductReservations.Domain.Entities;
public class ProductResidualStock : EntityBase
{
	public Guid ProductId { get; private set; }
	public Guid WarehouseId { get; private set; }

	/// <summary>
	/// Фактическое количество продукта на складе
	/// </summary>
	public decimal InStockQuantity { get; private set; }

	protected ProductResidualStock()
	{
	}

	public ProductResidualStock(
		Guid id,
		Guid productId,
		Guid warehouseId,
		decimal quantity)
		: base(id)
	{
		Id = id;
		ProductId = productId;
		WarehouseId = warehouseId;
		InStockQuantity = quantity;
	}

	public void ChangeInStockQuantity(decimal value)
	{
		if (value < 0)
		{
			throw new ArgumentException("Нельзя присвоить отрицательное число товара на складе", nameof(value));
		}

		InStockQuantity = value;
	}
}
