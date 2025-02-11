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

// TO DO: UPDATE DELETE CREATE TAMAMLANACAK.

using Entities.Models;

namespace Services.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
        Task<Category> GetOneCategoryByIdAsync(int id, bool trackChanges);

    }
}
