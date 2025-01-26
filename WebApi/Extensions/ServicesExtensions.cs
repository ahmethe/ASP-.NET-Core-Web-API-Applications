using Entities.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
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
    }
}
