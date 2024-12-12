using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

/* 
<summary>
    Type Configuration, modelimiz veritabanına yansıtılacağı zaman ilgili alanlara
    default değerler atamak, çeşitli kısıtlar uygulamak veya direkt olarak ilgili tabloya seed data
    eklemek için kullanılır. Bunun yapılacağı modele ait bir sınıf oluşturulur. HasData fonksiyonu ile
    şayet veritabanı oluşturulurken tabloda değer yoksa istenilen sayıda parametre geçilen veriler
    veritabanına eklenir.
    Yaptığımız bu değişikliklerin veritabanına yansıması için elbette DbContext ile iletişime geçilmesi
    gerekir. Bunun için DbContext sınıfından gelen OnModelCreating metodu override edilir. İlgili metot gövdesi 
    yazılır. BookConfig sınıfının kalıtım aldığı interface ve OnModelCreating metodunun gövdesinde
    kullanılan modelBuilder.ApplyConfiguration metodunun aldığı parametre aynıdır. Bu önemli bir noktadır.
</summary>
*/

namespace WebApi.Repositories.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, Title = "Karagöz ve Hacivat", Price = 75},
                new Book { Id = 2, Title = "Mesnevi", Price = 175 },
                new Book { Id = 3, Title = "Devlet", Price = 375 }
            );
        }
    }
}
