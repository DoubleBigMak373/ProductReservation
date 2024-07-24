using Madrid.ProductReservations.Application;
using Madrid.ProductReservations.Application.Contracts;
using Madrid.ProductReservations.Domain.Repositories;
using Madrid.ProductReservations.EntityFrameworkCore.PostgreSql;
using Madrid.ProductReservations.EntityFrameworkCore.PostgreSql.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ProductReservationServiceCollectionExtensions
{
	public static IServiceCollection AddProductReservationModule(
		this IServiceCollection services,
		IConfiguration configuration,
		string connectionStringName)
	{
		if (string.IsNullOrWhiteSpace(connectionStringName))
			throw new ArgumentException(nameof(connectionStringName));

		services.TryAddTransient<IProductReservationAppService, ProductReservationAppService>();
		services.TryAddTransient<IProductsAppService, ProductsAppService>();
		services.TryAddTransient<IResidualStockAppService, ResidualStockAppService>();

		services.TryAddTransient(typeof(ICommonRepository<>), typeof(CommonEfCoreRepository<>));
		services.TryAddTransient<IProductReservationRepository, ProductReservationEfCoreRepository>();
		services.TryAddTransient<IProductResidualStockRepository, ProductResidualStockEfCoreRepository>();

		services.AddDbContext<ReservationDbContext>(options =>
			options.UseNpgsql(configuration.GetRequiredSection(connectionStringName).Value));

		return services;
	}
}
