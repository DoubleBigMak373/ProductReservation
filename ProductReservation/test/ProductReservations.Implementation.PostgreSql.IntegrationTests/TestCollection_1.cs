using Madrid.ProductReservations.Application.Contracts;
using Madrid.ProductReservations.Domain.Repositories;
using Madrid.ProductReservations.EntityFrameworkCore.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace ProductReservations.Implementation.PostgreSql.IntegrationTests;

public class TestCollection_1 : IAsyncDisposable
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IServiceScopeFactory _serviceScopeFactory;

	private readonly CancellationTokenSource _cancellationTokenSource;

	public TestCollection_1()
	{
		_cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(120));

		var services = new ServiceCollection();

		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>()
			{
				["ConnectionStrings.Default"] = @"Host=;Username=;Password=;Database="
			})
			.Build();

		services.AddProductReservationModule(configuration, "ConnectionStrings.Default");

		_serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions() { ValidateOnBuild = true, ValidateScopes = true });

		_serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();

		using var scope = _serviceScopeFactory.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();

		dbContext.Database.EnsureCreated();
	}

	public async ValueTask DisposeAsync()
	{
		using var scope = _serviceScopeFactory.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();

		await dbContext.Database.EnsureDeletedAsync();
	}

	[Fact]
	public async Task Test1()
	{
		// Arrange
		Guid productId = Guid.NewGuid();
		var warehouseid = Guid.NewGuid();

		using (var scope = _serviceScopeFactory.CreateScope())
		{
			using var dbContext = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
			var productApp = scope.ServiceProvider.GetRequiredService<IProductsAppService>();
			var residualApp = scope.ServiceProvider.GetRequiredService<IResidualStockAppService>();

			var product = await productApp.CreateAsync(new CreateProductRequest()
			{
				Name = "Тест продукт"
			},
			_cancellationTokenSource.Token);

			productId = product.Id;

			await residualApp.CreateAsync(new CreateProductResidualRequest()
			{
				ProductId = product.Id,
				Quantity = 100,
				WarehouseId = warehouseid
			},
			_cancellationTokenSource.Token);
		}

		// Action
		var rnd = new Random();

		await Parallel.ForAsync(
			0,
			999,
			new ParallelOptions
			{
				MaxDegreeOfParallelism = 10,
				CancellationToken = _cancellationTokenSource.Token
			},
			async (i, ct) =>
			{
				try
				{
					using var scope = _serviceScopeFactory.CreateScope();
					var reservationApp = scope.ServiceProvider.GetRequiredService<IProductReservationAppService>();

					await reservationApp.CreateAsync(new CreateProductResidualRequest()
					{
						ProductId = productId,
						Quantity = rnd.Next(1, 3),
						WarehouseId = warehouseid
					},
					ct);
				}
				catch { }
			});

		// Assert
		using (var scope = _serviceScopeFactory.CreateScope())
		{
			var reservationApp = scope.ServiceProvider.GetRequiredService<IProductReservationRepository>();

			var reservation = await reservationApp.FindAsync(
				productId,
				warehouseid,
				_cancellationTokenSource.Token);

			reservation.ShouldNotBeNull();
			reservation.Quantity.ShouldBeLessThanOrEqualTo(100);
			reservation.Quantity.ShouldBeGreaterThan(0);
		}
	}
}
