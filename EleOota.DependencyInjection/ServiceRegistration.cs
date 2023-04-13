using AutoMapper;
using EleOota.FrameworkExtensions;
using EleOota.Repository.Implementation;
using EleOota.Repository.Infrastructure;
using EleOota.Repository.Infrastructure.Interface;
using EleOota.Repository.Interface;
using EleOota.Service.Implementation;
using EleOota.Service.Interface;
using EleOota.Service.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EleOota.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static void AddServices( this IServiceCollection services)
        {
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ILoginService, LoginService>();

            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<IEmailServices, EmailServices>();
        }
        public static void AddInfrastructure(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddSettingsProvider(configuration);
            services.AddTransient<IQueryBuilder, SqlQueryBuilder>();
           /* services.AddTransient<ISettingsService, SettingsService>();*/
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IConnectionFactory,SqlConnectionFactory>();
            
        }
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = AutoMapperConfiguration.Intialize();
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
