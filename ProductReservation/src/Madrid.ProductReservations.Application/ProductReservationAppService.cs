using Madrid.ProductReservations.Application.Contracts;
using Madrid.ProductReservations.Domain.Entities;
using Madrid.ProductReservations.Domain.Repositories;

namespace Madrid.ProductReservations.Application;

public class ProductReservationAppService(
	IProductResidualStockRepository productResidualRepository,
	IProductReservationRepository reservationRepository) : IProductReservationAppService
{
	private readonly IProductResidualStockRepository _productResidualRepository = productResidualRepository;
	private readonly IProductReservationRepository _reservationRepository = reservationRepository;

	public async Task<ProductResevationDto> CreateAsync(CreateProductResidualRequest request, CancellationToken cancellationToken = default)
	{
		var reservation = await _reservationRepository.FindAsync(
			request.ProductId,
			request.WarehouseId,
			cancellationToken);

		var productResidual = await _productResidualRepository.FindAsync(
			request.ProductId,
			request.WarehouseId,
			cancellationToken);

		if (productResidual is null)
			throw new InvalidOperationException("Невозможно создать бронирование товара. Причина: не найдено сведений по остаткам");

		if (reservation is not null)
		{
			return await UpdateReservationAsync(
				reservation,
				request,
				productResidual,
				cancellationToken);
		}

		return await CreateReservationAsync(
			request,
			productResidual,
			cancellationToken);
	}

	public Task<ProductResevationDto> UpdateAsync(UpdateProductResidualRequest request, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<ProductResevationDto> CancelAsync(Guid id, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	private async Task<ProductResevationDto> CreateReservationAsync(
		CreateProductResidualRequest request,
		ProductResidualStock productResidual,
		CancellationToken cancellationToken)
	{
		if (productResidual.InStockQuantity == 0)
			throw new InvalidOperationException("Невозможно создать бронирование товара. Причина: нет доступных товаров для резервирования");

		if (productResidual.InStockQuantity < request.Quantity)
			throw new InvalidOperationException("Невозможно создать бронирование товара. Причина: не хватает остатков на складе");

		var reservation = new ProductReservation(
			Guid.NewGuid(),
			request.ProductId,
			request.WarehouseId,
			request.Quantity);

		var created = await _reservationRepository.InsertAsync(reservation, saveChanges: true, cancellationToken);

		return new ProductResevationDto()
		{
			Id = created.Id,
			ProductId = created.ProductId,
			WarehouseId = created.WarehouseId,
			Quantity = created.Quantity
		};
	}

	private async Task<ProductResevationDto> UpdateReservationAsync(
		ProductReservation reservation,
		CreateProductResidualRequest request,
		ProductResidualStock productResidual,
		CancellationToken cancellationToken)
	{
		var avaliableToReserve = productResidual.InStockQuantity - reservation.Quantity;

		if (avaliableToReserve <= 0)
			throw new InvalidOperationException("Невозможно создать бронирование товара. Причина: не хватает остатков на складе");

		reservation.ChangeReservationQuantity(avaliableToReserve - request.Quantity);

		var updated = await _reservationRepository.UpdateAsync(reservation, saveChanges: true, cancellationToken);

		return new ProductResevationDto()
		{
			Id = updated.Id,
			ProductId = updated.ProductId,
			WarehouseId = updated.WarehouseId,
			Quantity = updated.Quantity
		};
	}
}
