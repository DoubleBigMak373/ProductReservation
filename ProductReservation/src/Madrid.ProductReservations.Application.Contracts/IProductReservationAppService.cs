namespace Madrid.ProductReservations.Application.Contracts;
public interface IProductReservationAppService
{
	Task<ProductResevationDto> CreateAsync(CreateProductResidualRequest request, CancellationToken cancellationToken = default);
	Task<ProductResevationDto> UpdateAsync(UpdateProductResidualRequest request, CancellationToken cancellationToken = default);
	Task<ProductResevationDto> CancelAsync(Guid id, CancellationToken cancellationToken = default);
}

public class CreateProductResevationRequest
{
	public required Guid ProductId { get; set; }
	public required Guid WarehouseId { get; set; }
	public required decimal Quantity { get; set; }
}

public class UpdateProductResevationRequest
{
	public required Guid ProductId { get; set; }
	public required Guid WarehouseId { get; set; }
	public required decimal Quantity { get; set; }
}

public class ProductResevationDto
{
	public required Guid Id { get; set; }
	public required Guid ProductId { get; set; }
	public required Guid WarehouseId { get; set; }
	public required decimal Quantity { get; set; }
}
