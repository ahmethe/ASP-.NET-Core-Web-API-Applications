using Entities.Models;

/*
<summary>
    Repositoryde de benzer tanımları yapmıştık. Fakat burada işler değişecek.
    Kullanılan verinin yapısı değişecek, çeşitli lojikler işletilecek. Fonksiyonların
    imzaları değişecek. Dolayısıyla ayrı ayrı tanımların olması gerekir.
</summary>
*/

namespace Services.Contracts
{
    public interface IBookService
    {
        //IEnumerable yani foreach ile dolaşılabilir bir ifade.
        IEnumerable<Book> GetAllBooks(bool trackChanges);
        Book GetOneBookById(int id, bool trackChanges);
        Book CreateOneBook(Book book);
        void UpdateOneBook(int id, Book book, bool trackChanges);
        void DeleteOneBook(int id, bool trackChanges); 
    }
}
