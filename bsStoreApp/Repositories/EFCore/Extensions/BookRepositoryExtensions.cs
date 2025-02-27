﻿using Entities.Models;
using System.Linq.Dynamic.Core;

/* 
<summary>
    FindByCondition ifadesine baktığımızda IQueryable<Book> yani sorgulanabilir bir ifade dönüyor.
    Bu dönüş türü referans alınarak yazılan extension methodun dönüş türü de böyle tanımlandı.
    Yine bir sorgu ifadesi, bir expression yazıldığı için genişletilmesi gereken ifade de IQueryable<Book>.
    BookRepository sealed anahtar kelimesi ile zırhlanıp kalıtıma kapalı hale getirildi yani son halini aldı.
    Artık sadece gelişmeye açık. Ve bu gelişme de extension methodlar aracılığıyla sağlanacak.
    Search ifadesi için de aynı durum geçerli.
    Ayrıca bu 2 özellik için de belirtmek gerekir ki kontrat üzerinde bir değişiklik yapılmadığı için
    diğer katmanlarda ayrıca bir değişiklik yapılmadı. Uygulama detaylarını ilgilendirecek bir değişiklik yapıldı.
    BookParameters ifadesine yeni özellikler tanımlandı. Ve bu özellikler uygulamada kullanıldı.
    Sort fonksiyonunda oluşturulan genel sorgu oluşturucu fonksiyonun çağrılmasına ek olarak kaynağa has birtakım kontroller yapılıp
    sorgu işlemi gerçekleştirildi.
</summary>
*/

namespace Repositories.EFCore.Extensions
{
    public static class BookRepositoryExtensions
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books,
            uint minPrice, uint maxPrice) =>
            books.Where(book =>
            book.Price >= minPrice &&
            book.Price <= maxPrice);

        public static IQueryable<Book> Search(this IQueryable<Book> books, 
            string searchTerm)
        {
            if(string.IsNullOrWhiteSpace(searchTerm))
                return books;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return books
                .Where(b => b.Title
                .ToLower()
                .Contains(searchTerm));
        }

        public static IQueryable<Book> Sort(this IQueryable<Book> books,
            string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return books.OrderBy(b => b.Id); //Repositoryde default karşılaştırma id üzerinden ascending tanımlandığı için böyle bir ifade var.

            var orderQuery = OrderQueryBuilder
                .CreateOrderQuery<Book>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return books.OrderBy(b => b.Id); // Aynı default davranış burası için de geçerli.

            return books.OrderBy(orderQuery);
        }
    }
}
