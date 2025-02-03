using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.EFCore;

/* 
<summary>
    Hedef projemiz WebApi fakat DbContext Repositories katmanında ve migrationlar
    bu katmanda oluşmakta. Bunu WebApi'ye taşıyabilmek için Design timeda bu sınıfı 
    görevlendirdik. DbContextin oluşması için bir Configurationa bir de DbContextOptionsa
    ihtiyacımız var. Configuration appsettings.json dosyasındaki alanlara ulaşırken, DbContextOptions
    ConnectionString ile DbContexti buluşturmamızı sağlayacak. DbContextin olabilmesi için connection
    stringe ihtiyaç duyarız çünkü. Bunu yapabilmek için ilgili interfacein ilgili fonksiyonu
    sınıf tarafından implemente edildi.
</summary>
*/

namespace WebApi.ContextFactory
{
    public class RepositoryContextFactory
        : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            //ConfigurationBuilder
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            //DbContextOptionsBuilder
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                prj => prj.MigrationsAssembly("WebApi"));

            return new RepositoryContext(builder.Options);
        }
    }
}
