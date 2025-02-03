using Entities.DataTransferObjects;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using System.Dynamic;

/*
<summary>
    Repositoryde de benzer tanımları yapmıştık. Fakat burada işler değişecek.
    Kullanılan verinin yapısı değişecek, çeşitli lojikler işletilecek. Fonksiyonların
    imzaları değişecek. Dolayısıyla ayrı ayrı tanımların olması gerekir.
    Repository katmanından farklı olarak burada create, update ve delete yapan methodlar
    asenkron tanımlandı. Çünkü bu methodlar gövdelerinde kayıt işlemi yapıyor. Bu yüzden
    kullanmalıyız.
</summary>
*/

namespace Services.Contracts
{
    public interface IBookService
    {
        //IEnumerable yani foreach ile dolaşılabilir bir ifade.
        Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(LinkParameters linkParameters, 
            bool trackChanges);
        Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges);
        Task<BookDto> CreateOneBookAsync(BookDtoForInsertion book);
        Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges);
        Task DeleteOneBookAsync(int id, bool trackChanges);
        Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges);
        Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book);
    }
}
