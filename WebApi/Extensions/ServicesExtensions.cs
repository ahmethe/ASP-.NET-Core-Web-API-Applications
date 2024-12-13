using Microsoft.EntityFrameworkCore;
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

    ConfigureSqlContext: DbContext ifademizin servis kaydı. (IoC)
    ConfigureRepositoryManager: RepositoryManager ifademizin servis kaydo. (IoC)
    ConfigureServiceManager: ServiceManager ifademizin servis kaydo. (IoC)
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
    }
}
