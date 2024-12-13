using Entities.Models;
using Repositories.Contracts;

/*
<summary>
    Burada bir işlem tekrarı var gibi görünse de aslında durum böyle değil.
    CRUD işlemleri mevcut modele göre özelleşebilir. Bunu sağlayabilmek için bu
    şekilde yazıldı. İleride nesne ilişkileri değişebilir. Bunlar da göz önüne alınmalıdır.
</summary>
*/

namespace Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateOneBook(Book book) => Create(book);

        public void DeleteOneBook(Book book) => Delete(book);

        public IQueryable<Book> GetAllBooks(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(b => b.Id);

        public Book GetOneBookById(int id, bool trackChanges) =>
            FindByCondition(b => b.Id.Equals(id), trackChanges)
            .SingleOrDefault();

        public void UpdateOneBook(Book book) => Update(book);
    }
}
