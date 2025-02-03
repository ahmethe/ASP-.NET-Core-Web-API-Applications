using Repositories.Contracts;

/*
<summary>
    Burada Unit Of Work Pattern kullanıldı. Uygulamanın sahip olduğu repolara
    buradan erişilecek. Ve farklı farklı repolarda yapılan farklı farklı işlemler
    Save metodu yardımıyla kaydedilecek. Burada ek olarak bir newleme işlemi söz konusu.
    Aslında bu da constructor injection yardımı ve IoC kaydı ile yapılabilirdi. Fakat
    bu sefer constructor parametreleri artacak. Uygulamanın implementasyonu değişecek.
    Dolayısıyla bu şekilde bir istisna gerçekleştirildi.
    Eager Loading (Şahin yükleme): Nesne ve nesne ile alakalı her şeyi bir defada anında oluşturmak.
    Lazy Loading (Tembel yükleme): Nesne ve nesne ile alakalı her şeyi yalnız kullanılacağı zaman 
    oluşturmak. Bir referans olabilir fakat nesne üretimi ihtiyaç anında olur. Kaynak kullanım verimliliği artar.
</summary>
*/

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly Lazy<IBookRepository> _bookRepository;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _bookRepository = new Lazy<IBookRepository>(() => new BookRepository(_context));
        }

        public IBookRepository Book => _bookRepository.Value;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
