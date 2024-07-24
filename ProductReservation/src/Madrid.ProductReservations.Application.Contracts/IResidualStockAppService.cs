using System.ComponentModel.DataAnnotations;

namespace Madrid.ProductReservations.Application.Contracts;

public interface IResidualStockAppService
{
	Task<ResidualStockDto> CreateAsync(CreateProductResidualRequest request, CancellationToken cancellationToken = default);
	Task<ResidualStockDto> UpdateAsync(UpdateProductResidualRequest request, CancellationToken cancellationToken = default);
	Task<ResidualStockDto> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public class CreateProductResidualRequest : IValidatableObject
{
	public required Guid ProductId { get; set; }
	public required Guid WarehouseId { get; set; }
	public required decimal Quantity { get; set; }

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (Quantity <= 0)
		{
			yield return new ValidationResult("Must be positive", [nameof(Quantity)]);
		}
	}
}

public class UpdateProductResidualRequest
{
	public required Guid ProductId { get; set; }
	public required Guid WarehouseId { get; set; }
	public required decimal Quantity { get; set; }
}

public class ResidualStockDto
{
	public required Guid Id { get; set; }
	public required Guid ProductId { get; set; }
	public required Guid WarehouseId { get; set; }
	public required decimal InStockQuantity { get; set; }
}
