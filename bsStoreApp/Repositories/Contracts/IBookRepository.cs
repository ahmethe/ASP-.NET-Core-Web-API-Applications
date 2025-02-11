using Entities.Models;
using Entities.RequestFeatures;

/* 
<summary>
    Dikkat edilirse create, update ve delete işlemleri üzerinde asenkron bir yapı tanımına gidilmedi.
    Çünkü burada tracking(izleme) temelli bir yaklaşım söz konusudur. Yani bu işlemlerden sonra değişim
    kalıcı olmaz. Ancak SaveChangesAsync methodu çağrıldığı zaman veritabanına yansır. Bu yüzden repository manager
    ifadesinde değişiklikleri kaydeden method düzenlenerek aslında asenkron değişim sağlanmış oldu.
</summary>
*/

namespace Repositories.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, 
            bool trackChanges);
        Task<Book> GetOneBookByIdAsync(int id, bool trackChanges);
        void CreateOneBook(Book book);
        void UpdateOneBook(Book book);
        void DeleteOneBook(Book book);
        Task<List<Book>> GetAllBooksAsync(bool trackChanges);
        Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges);
    }
}
