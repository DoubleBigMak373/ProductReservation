using Madrid.ProductReservations.Application.Contracts;
using Madrid.ProductReservations.Domain.Entities;
using Madrid.ProductReservations.Domain.Repositories;

namespace Madrid.ProductReservations.Application;
public class ProductsAppService(ICommonRepository<Product> repository) : IProductsAppService
{
	private readonly ICommonRepository<Product> _repository = repository;

	public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
	{
		var product = new Product(
			Guid.NewGuid(),
			request.Name);

		var created = await _repository.InsertAsync(product, saveChanges: true, cancellationToken);

		return new ProductDto()
		{
			Id = created.Id,
			Name = created.Name
		};
	}

	public Task<ProductDto> UpdateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<ProductDto> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}
