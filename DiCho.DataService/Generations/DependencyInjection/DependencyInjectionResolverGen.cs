
/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using DiCho.DataService.Models;
using Microsoft.Extensions.DependencyInjection;
using DiCho.DataService.Services;
using DiCho.DataService.Repositories;
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
namespace DiCho.DataService.Commons
{
    public static class DependencyInjectionResolverGen
    {
        public static void InitializerDI(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DbContext, DiChoNaoContext>();
        
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAddressRepository, AddressRepository>();
        
            services.AddScoped<IAspNetRoleClaimsService, AspNetRoleClaimsService>();
            services.AddScoped<IAspNetRoleClaimsRepository, AspNetRoleClaimsRepository>();
        
            services.AddScoped<IAspNetRolesService, AspNetRolesService>();
            services.AddScoped<IAspNetRolesRepository, AspNetRolesRepository>();
        
            services.AddScoped<IAspNetUserClaimsService, AspNetUserClaimsService>();
            services.AddScoped<IAspNetUserClaimsRepository, AspNetUserClaimsRepository>();
        
            services.AddScoped<IAspNetUserLoginsService, AspNetUserLoginsService>();
            services.AddScoped<IAspNetUserLoginsRepository, AspNetUserLoginsRepository>();
        
            services.AddScoped<IAspNetUserRolesService, AspNetUserRolesService>();
            services.AddScoped<IAspNetUserRolesRepository, AspNetUserRolesRepository>();
        
            services.AddScoped<IAspNetUsersService, AspNetUsersService>();
            services.AddScoped<IAspNetUsersRepository, AspNetUsersRepository>();
        
            services.AddScoped<IAspNetUserTokensService, AspNetUserTokensService>();
            services.AddScoped<IAspNetUserTokensRepository, AspNetUserTokensRepository>();
        
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
        
            services.AddScoped<ICampaignDeliveryZoneService, CampaignDeliveryZoneService>();
            services.AddScoped<ICampaignDeliveryZoneRepository, CampaignDeliveryZoneRepository>();
        
            services.AddScoped<IFarmService, FarmService>();
            services.AddScoped<IFarmRepository, FarmRepository>();
        
            services.AddScoped<IFarmOrderService, FarmOrderService>();
            services.AddScoped<IFarmOrderRepository, FarmOrderRepository>();
        
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
        
            services.AddScoped<IPaymentTypeService, PaymentTypeService>();
            services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
        
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        
            services.AddScoped<IProductHarvestService, ProductHarvestService>();
            services.AddScoped<IProductHarvestRepository, ProductHarvestRepository>();
        
            services.AddScoped<IProductHarvestInCampaignService, ProductHarvestInCampaignService>();
            services.AddScoped<IProductHarvestInCampaignRepository, ProductHarvestInCampaignRepository>();
        
            services.AddScoped<IProductHarvestOrderService, ProductHarvestOrderService>();
            services.AddScoped<IProductHarvestOrderRepository, ProductHarvestOrderRepository>();
        
            services.AddScoped<IProductSalesCampaignService, ProductSalesCampaignService>();
            services.AddScoped<IProductSalesCampaignRepository, ProductSalesCampaignRepository>();
        
            services.AddScoped<IProductSystemService, ProductSystemService>();
            services.AddScoped<IProductSystemRepository, ProductSystemRepository>();
        
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IShipmentRepository, ShipmentRepository>();
        
            services.AddScoped<IShipmentDestinationService, ShipmentDestinationService>();
            services.AddScoped<IShipmentDestinationRepository, ShipmentDestinationRepository>();
        
            services.AddScoped<IWareHouseService, WareHouseService>();
            services.AddScoped<IWareHouseRepository, WareHouseRepository>();
        
            services.AddScoped<IWareHouseZoneService, WareHouseZoneService>();
            services.AddScoped<IWareHouseZoneRepository, WareHouseZoneRepository>();
        }
    }
}
