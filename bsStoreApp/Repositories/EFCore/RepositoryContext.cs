using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

/*
<summary>
    Context olmadan herhangi bir şekilde herhangi bir varlık kullanılamaz. 
    Varlık varsa bu DbSet ile sarmallanmalıdır ki tablosu oluşabilsin.
    Aksi halde context üzerinden bu varlığa ait tabloya ulaşamayız.
</summary>
*/

namespace Repositories.EFCore
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) :
            base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //Veritabanı tablo yapısını Identity paketine uygun düzenlemeyi ve doğru bir şekilde migration alabilmeyi sağlayacak.
            /* modelBuilder.ApplyConfiguration(new BookConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig()); */
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
