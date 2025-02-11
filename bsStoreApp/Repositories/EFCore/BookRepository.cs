using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;

/*
<summary>
    Burada bir işlem tekrarı var gibi görünse de aslında durum böyle değil.
    CRUD işlemleri mevcut modele göre özelleşebilir. Bunu sağlayabilmek için bu
    şekilde yazıldı. İleride nesne ilişkileri değişebilir. Bunlar da göz önüne alınmalıdır.
    
    .NET -> Asynchronous Programming Model (APM), Event-based Asynchronous Pattern (EAP)
    TASK-BASED ASYNCHRONOUS PATTERN (TAP)
    
    Senkron programlamada tek bir thread içerisinde bütün işlem gerçekleşir. Threadler idlere sahiptir.
    İşlemler thread içerisinde gerçekleştirilir. Senkron işlemde thread poola bir thread alınır ve ilgili
    işlem gerçekleştirilir.Eğer thread pool dolarsa ve yeni  bir istek gelirse bu istek önceki isteklerden 
    birinin tamamlanmasını bekler.

    Asenkron programlamada da benzer yapılar kullanılır. Fakat istekler birbirinden bağımsız şekilde
    thread poola thread alır ve farklı iş hatları üzerinden bu işlemleri gerçekleştirir. Bu performans anlamında
    bir fark yaratır. Fakat şuna dikkat edilmelidir ki işlemin gerçekleşme süresi ve kaynak kullanımında bir değişim
    gözlenmez. Bir iş hattı yine söz konusudr, fakat işlemler asenkron bir şekilde ayrı threadlerde gerçekleşir
    hatta threadler içerisinde bile asenkron işlemler gerçekleşebilir. Bunun yönetimi framework tarafından yapılır. Biz
    sadece belirli anahtar kelimeleri kullanırız.
    
    Single threadde işlemler ardışık bir şekilde tek bir threadde gerçekleşir. Multithread yapısında ise işlemler ayrı ayrı
    threadler içerisinde gerçekleşir ve görevlerin tamamlanması süreci birbirinden bağımsızdır. Performans artışı, zamandan
    kazanç sağlanır.
    
    Single threadde asenkron kod yazılabilir. Böylelikle bir işin tamamlanamsı beklemeden işler parça parça gerçekleşir. Fakat 
    burada bir bloklama süresi söz konusudur. Bundan kurtulunursa zaman kazancı sağlanabilir. Temel düşünce bloklama olduğu durumlarda
    başka bir görev yapılarak zamanın verimli kullanılmasıdır.

    .NET dünyasında asenkron programlama için thread pool kullanılır. Çalıştırılan thread yapılarının yönetimi Task ile sağlanır.
    Multithread yapılarda asenkron programlamayı sağlar. Hem thread hem asenkron yönetim süreci sağlanır. 
    Task objesi temel olarak üstlendiği işleri thread pool üzerinde asenkron olarak çalıştırır.

    Task içerisinde operasyon ile ilgili bir çok property barındırır. Buradan operasyonun durumu takip edilebilir.
    Async ile bir operasyon tanımlandıysa await bu operasyonun çalıştırılmasından sorumludur. Operasyonun başarı durumunu doğrular.
    Zaman uyumsuz yöntemde kodun geri kalanını yürütmek için devamı sağlar. await anahtar kelimesinin kullanımı zorunlu değildir.
    Fakat şiddetle kullanılması önerilir. Kullanılmazsa manuel yöntemler ile ilgili ifadenin çalışması sağlanabilir fakat süreç yönetimi zorlaşır.
    Ayrıca await anahtar kelimesi yalnızca bir kez kullanılmak zorunda da değildir.
</summary>
*/

namespace Repositories.EFCore
{
    public sealed class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateOneBook(Book book) => Create(book);

        public void DeleteOneBook(Book book) => Delete(book);

        public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, 
            bool trackChanges)
        {
            var books = await FindAll(trackChanges)
                .FilterBooks(bookParameters.MinPrice, bookParameters.MaxPrice)
                .Search(bookParameters.SearchTerm)
                .Sort(bookParameters.OrderBy)
                .ToListAsync();

            return PagedList<Book>.
                ToPagedList(books, 
                bookParameters.PageNumber, 
                bookParameters.PageSize);
        }

        public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderBy(b => b.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
        {
            return await _context
                .Books
                .Include(b => b.Category) //EF Core nesne ilişkisi durumunda Lazy Loading yapar. Kategoriyi bu ifadeyi yazmazsak getirmez.
                .OrderBy(b => b.Id)
                .ToListAsync();
        }

        public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges) =>
            await FindByCondition(b => b.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

        public void UpdateOneBook(Book book) => Update(book);
    }
}
