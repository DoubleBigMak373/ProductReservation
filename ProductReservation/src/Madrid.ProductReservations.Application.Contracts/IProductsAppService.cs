namespace Madrid.ProductReservations.Application.Contracts;

public interface IProductsAppService
{
	Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
	Task<ProductDto> UpdateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
	Task<ProductDto> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public class ProductDto
{
	public required Guid Id { get; set; }
	public required string Name { get; set; }
}

public class CreateProductRequest
{
	public required string Name { get; set; }
}

public class UpdateProductRequest
{
	public required string Name { get; set; }
}
