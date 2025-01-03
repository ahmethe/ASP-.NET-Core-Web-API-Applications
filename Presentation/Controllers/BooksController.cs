using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc; //Bu paketi indirdik çünkü controller yapısını kullanabilmemiz gerekiyor. Controller özelliğini kazandırabilmek için.
using Presentation.ActionFilters;
using Services.Contracts;
using System.Text.Json;

/* 
<summary>
    Burada constructor üzerinde görüldüğü gibi bir dependency injection patterni
    mevcut. Burada IoC ile containerdan ilgili ifade resolve ediliyor framework tarafından.

    Asenkron yapısı tanımlanırken aslında method isimlerine dokunmayabilirdik. Çünkü bu fonksiyonlar
    çağrılırken bir Name ifadesi üzerinden çağrılacak imza üzerindeki isimden değil.
    
    [ActionFilter]: Bir controller ya da controller içindeki action yapısına uygulanan ve bu yolla
    ilgili yapıyı düzenleyen attribute. Authorization filters, resource filters, ACTION FILTERS, exception filters,
    result filters...
    IActionFilter, IAsyncActionFilter, ActionFilterAttribute(abstract class) bu 3 yapı kullanılarak action filter tasarlanabilir.
    Global -> AddController yapısı içerisinde register edilir.
    Controller ve action levelde de yapılabilir. IoC ile doğrudan yapılabilir.
    Bir actionun çalışmasından önce veya çalışmasından sonra action filter çalıştırılabilir.
    Birden fazla action filter kullandığımızda bunların, order ile çağrılma sırasını belirleyebiliriz.

    Paging, API'dan sonuçların kısmi olarak alınmasıdır. RESTful API tasarımı için önemlidir.
    Bant genişliği bizi kısıtlayan bir durumdur. Bilgisayar bilimlerinde bu tür taleplerin sonsuza gittiği varsayılır.
    Çok büyük bir veriseti istemciye gönderilemez. Hafıza sorunları ile hem istemci hem sunucu tarafında karşılaşılabilir.
    Bunun yerine bir sayfalama mekanizması ile bütün veri okunmadan istemciden alınan request parametreleri ile
    veri sunulabilir. Bu concrete bir query yardımıyla ve bir endpoint tanımı ile gerçekleştirilebilir. Kullanıcıya sayfalandırma
    mekanizması ile ilgili header kısmında bilgi sunulabilir.
    Bir kaynakla doğrudan ilgili olmayan ifadeler query string olarak tanımlanabilir.

    Filtering, bir takım kriterlere bağlı olarak sonuçların getirilmesini sağlayan bir mekanizmadır.
    Örneğin bir araç satış sitesinde, ilanları vasıtanın türüne, daha sonra markasına, modeline göre filtreleyebiliriz. Paginge benzer
    şekilde kaynak setinin bir alt kümesine erişiyoruz. Burada MVC patterninin view ifadesi front ende bırakıldığı için elimizde bir ui yok.
    Yine pagingde ifade edildiği gibi doğrudan kaynakla ilgili olmadığı için biz bu filtreleme işlemini query stringler üzerinden yapacağız.

    Searching bir terim ya da anahtar değer yardımıyla uygulama içerisindeki en alakalı sonuçları döndürmek üzere uygulanan bir işlevdir.
    Tek veya birden fazla kolon üzerinde yapılabilir. Bir veya birden fazla kaynak üzerinde yapılabilir.
    Bir arama terimi ile endpoint tanımı gerçekleştirilicek. Çeşitli kütüphaneler yardımıyla daha komplex searching yapılabilir.
</summary>
*/

namespace Presentation.Controllers
{
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks([FromQuery] BookParameters bookParameters)
        {
            var pagedResult = await _manager.
                BookService.
                GetAllBooksAsync(bookParameters, false);

            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(pagedResult.metaData));
            
            return Ok(pagedResult.books);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
            var book = await _manager
                .BookService
                .GetOneBookByIdAsync(id, false);

            return Ok(book);
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
        {
            var book = await _manager.BookService.CreateOneBookAsync(bookDto);
            return StatusCode(201, book); // CreatedAtRoute() Response headere location koyulabilir. Oluşan yeni kaynağın URI.
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id,
            [FromBody] BookDtoForUpdate bookDto)
        {
            await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
            await _manager.BookService.DeleteOneBookAsync(id, false);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBookAsync([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            if (bookPatch is null)
                return BadRequest();

            var result = await _manager.BookService.GetOneBookForPatchAsync(id, true);

            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

            TryValidateModel(result.bookDtoForUpdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);

            return NoContent();
        }

    }
}
