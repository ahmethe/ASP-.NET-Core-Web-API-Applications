﻿using AspNetCoreRateLimit;
using Entities.DataTransferObjects;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repositories.Contracts;
using Repositories.EFCore;
using Services;
using Services.Contracts;

/*
<summary>
    İhtiyacımz olan servis kayıtlarını static bir class içerisinde fonksiyon haline getirip saklayacağız.
    Extension method yazarken genişletmek istediğimiz ifadeyi this anahtar sözcüğü ile ilk parametre olarak
    vereceğiz. Daha sonra ihtiyaç duyulan başka parametreler varsa bunlar da eklenecek. Daha sonra eklenmek istenen
    kayıt fonksiyon gövdesine yazılacak. Böylelikle Program.cs içerisindeki konfigürasyonlar daha iyi yönetilebilecek.
    IoC kaydı yapılırken sınıflardan üretilen nesnelerin yaşam döngüsü için AddTransient, AddScoped, AddSingleton gibi ifadeler
    kullanılacak.

    ConfigureSqlContext: DbContext ifademizin servis kaydı. (IoC)
    ConfigureRepositoryManager: RepositoryManager ifademizin servis kaydı. (IoC)
    ConfigureServiceManager: ServiceManager ifademizin servis kaydı. (IoC)
    ConfigureLoggerService: NLog kütüphanesi ile kullandığımız LoggerService ifadesinin servis kaydı. (IoC)
    AddCustomMediaTypes: Zengin çıktı istendiğinde linkleri üretecek şekilde media type ifadesi eklemek amacıyla eklendi.(Hypermedia) Daha sonra
    kök belge organizasyonu gerekirse, yeni bir media type eklemek gerekirse yine bu noktadan yönetimi gerçekleştirilecek.
    ConfigureVersioning; 
    ReportApiVersions: API version bilgisinin response headerda yer almasını sağlar.
    AssumeDefaultVersionWhenUnspecified: Kullanıcı herhangi version bilgisi talep etmese de API default bir version bilgisine sahip.
    DefaultApiVersion: Default versionu belirttiğimiz yer. Major değişiklikler 1, minör değişikler 0.
    ApiVersionReader: Header ile versioning yapmak için.
    Conventions.Controller: Bu ifade ile konfigürasyon yardımıyla daha önceki yöntemlerde attribute ile yaptığımzıı yapmış olduk.
</summary>
*/

namespace WebApi.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, 
            IConfiguration configuration) => services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>(); //AddScoped ile her kullanıcıya özel olarak nesne üretilecek.

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerService, LoggerManager>(); //Bir nevi static bir tanım. Tek bir nesne üretilecek ve tüm kullanıcılar tarafından kullanılacak.
    
        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddSingleton<LogFilterAttribute>();
            services.AddScoped<ValidateMediaTypeAttribute>();
        }
        
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination") // Bu ifadenin kullanılmasına izin verdik.
                );
            });
        }

        public static void ConfigureDataShaper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
        }

        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config
                .OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

                if(systemTextJsonOutputFormatter is not null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.btkakademi.hateoas+json"); //vendor=vnd

                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.btkakademi.apiroot+json");
                }

                var xmlOutputFormatter = config
                .OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

                if(xmlOutputFormatter is not null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.btkakademi.hateoas+xml");

                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.btkakademi.apiroot+xml");
                }
            });
        }
        
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true; 
                opt.AssumeDefaultVersionWhenUnspecified = true; 
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
                
                opt.Conventions.Controller<BooksController>()
                    .HasApiVersion(new ApiVersion(1, 0));

                opt.Conventions.Controller<BooksV2Controller>()
                    .HasDeprecatedApiVersion(new ApiVersion(2, 0));
            });
        }

        public static void ConfigureResponseCaching(this IServiceCollection services) =>
            services.AddResponseCaching();

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
            services.AddHttpCacheHeaders(expirationOpt =>
            {
                expirationOpt.MaxAge = 90;
                expirationOpt.CacheLocation = CacheLocation.Public;
            },
            validationOpt => 
            {
                validationOpt.MustRevalidate = false;
            });

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>()
            {
                new RateLimitRule()
                {
                    Endpoint = "*",
                    Limit = 3,
                    Period = "1m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore,  MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }
    }
}
