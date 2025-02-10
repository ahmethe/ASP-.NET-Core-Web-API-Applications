using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

/*
<summary>
    Geliştirdiğimiz API, birtakım endpoint tanımlarına sahip. Bu tanımların olabildiğince değiştirilmemesi istenir. Fakat bazı durumlarda bu kaçınılmaz
    olmaktadır. Buna sebep olarak, API büyümesi, güvenlik açıkları, belirli değişiklik gereksinimleri örnek verilebilir. Amaç, API tüketen kullanıcıların 
    değişiklilerden en az seviyede etkilenmesini sağlamaktır.
</summary>
*/

namespace Presentation.Controllers
{
    //[ApiVersion("2.0", Deprecated = true)]
    [ApiController]
    [Route("api/books")]
    [ApiExplorerSettings(GroupName = "v2")]
    //[Route("api/{v:apiversion}/books")] URL ile versiyonlama.
    public class BooksV2Controller : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksV2Controller(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var books = await _manager
                .BookService
                .GetAllBooksAsync(false);

            var booksV2 = books.Select(b => new
            {
                Title = b.Title,
                Id = b.Id,
            });

            return Ok(booksV2);
        }
    }
}
