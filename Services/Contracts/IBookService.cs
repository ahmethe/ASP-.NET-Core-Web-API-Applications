using Entities.DataTransferObjects;
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
        IEnumerable<BookDto> GetAllBooks(bool trackChanges);
        BookDto GetOneBookById(int id, bool trackChanges);
        BookDto CreateOneBook(BookDtoForInsertion book);
        void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges);
        void DeleteOneBook(int id, bool trackChanges);
        (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges);
        void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book);
    }
}
