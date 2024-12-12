using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Repositories.Config;

namespace WebApi.Repositories
{
    /* 
    <summary>
        DbContext kodda yazdığımız modelin veritabanına 
        ilişkileriyle yansıtılmasını sağlayan metotları içeren bir sınıftır.
        Modelimize ait sınıfa DbSet diyerek bu sınıfın veritabanında bir tablo
        gibi davranmasını sağlarız.
        Bu DbContexti kullanabilmek için bunu bir connectionString ile
        ilişkilendirmemiz gerekir. Bunu da constructor aracılığıyla yaparız.
        Connection String ifademizi appsettings.json dosyasına yazarız.
        Bu uygulamada tek bir veritabanından gidileceği için direkt appsettings.json
        dosyasına yazıldı. Fakat genel olarak development modda sqlLite product
        modda sql server kullanılır.
        Daha sonra servis kaydı için Program.cs dosyasına gidilir. AddDbContext ile
        servis kaydı gerçekleştirilir. Bu bizim ihtiyacımız olduğunda bu contextin
        concrete halini elde etmemizi sağlar. Burada IoC(Inversion of Control) mekanizması
        çalışır. Biz bir framework kullanıyoruz. Yeri geldiğinde buna müdahale ederek
        kontrolü tersine çevirmiş oluyoruz. IoC register, resolve ve dispose işlemlerinden
        sorumludur. Program.cs'te (uygulamanın başlangıç noktası) yaptığımz şey register, ihtiyaç olduğunda bize concrete 
        ifadeyi vermesi resolve, oluşturulan nesnenin yaşam süresine karar verme dispose işlemi
        olarak adlandırılır.
    </summary>
    */
    public class RepositoryContext : DbContext 
    {
        public RepositoryContext(DbContextOptions options) :
            base(options)
        {
            
        }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfig());
        }
    }
}
