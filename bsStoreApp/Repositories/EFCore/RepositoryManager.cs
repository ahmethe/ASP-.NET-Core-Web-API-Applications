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
    EFCore Lazy Loadig yaklaşımını benimser. Sadece burada implementasyon yapıldığı için de bu sorun oluşturmayacak.

    Lazy Loading yaklaşımını terk edebilmek için gerekli interface-concrete ifade kaydının bir extension method aracılığıyla
    yapılması gerekir. Bundan sonra constructor injection yardımıyla gerekli atamalar yapılır.
</summary>
*/

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public RepositoryManager(RepositoryContext context, 
            IBookRepository bookRepository, 
            ICategoryRepository categoryRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public IBookRepository Book => _bookRepository;
        public ICategoryRepository Category => _categoryRepository;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
