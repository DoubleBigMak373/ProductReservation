using Madrid.ProductReservations.Application.Contracts;
using Madrid.ProductReservations.Domain.Entities;
using Madrid.ProductReservations.Domain.Repositories;

namespace Madrid.ProductReservations.Application;
public class ResidualStockAppService(ICommonRepository<ProductResidualStock> repository) : IResidualStockAppService
{
	private readonly ICommonRepository<ProductResidualStock> _repository = repository;

	public async Task<ResidualStockDto> CreateAsync(CreateProductResidualRequest request, CancellationToken cancellationToken = default)
	{
		var residual = new ProductResidualStock(
			Guid.NewGuid(),
			request.ProductId,
			request.WarehouseId,
			request.Quantity);

		var created = await _repository.InsertAsync(residual, saveChanges: true, cancellationToken);

		return new ResidualStockDto()
		{
			Id = created.Id,
			ProductId = created.ProductId,
			WarehouseId = created.WarehouseId,
			InStockQuantity = created.InStockQuantity
		};
	}

	public Task<ResidualStockDto> UpdateAsync(UpdateProductResidualRequest request, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<ResidualStockDto> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}
