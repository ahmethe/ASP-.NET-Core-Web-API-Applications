using Entities.Models;

/* 
<summary>
    FindByCondition ifadesine baktığımızda IQueryable<Book> yani sorgulanabilir bir ifade dönüyor.
    Bu dönüş türü referans alınarak yazılan extension methodun dönüş türü de böyle tanımlandı.
    Yine bir sorgu ifadesi, bir expression yazıldığı için genişletilmesi gereken ifade de IQueryable<Book>.
    BookRepository sealed anahtar kelimesi ile zırhlanıp kalıtıma kapalı hale getirildi yani son halini aldı.
    Artık sadece gelişmeye açık. Ve bu gelişme de extension methodlar aracılığıyla sağlanacak.
</summary>
*/

namespace Repositories.EFCore
{
    public static class BookRepositoryExtensions
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books,
            uint minPrice, uint maxPrice) =>
            books.Where(book => 
            book.Price >= minPrice && 
            book.Price <= maxPrice);
    }
}
