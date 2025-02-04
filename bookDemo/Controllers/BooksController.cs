using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

/*
<summary>
    HttpGet verb 2 farklı yerde kullanıldı. Bunların endpoint olarak ayrılması gerekir. Eğer
    ikisi de HttpGet olarak kullanılırsa API hangi methodu çağıracağına karar veremez. Dolayısıyla
    tek bir kitap getireceğimiz GetOneBook methodunda URI id route bilgisi eklendi ve bu problem giderildi.
    Parametre verileri route, query string, body aracılığıyla gelebilir. Yine ApiController sayesinde automatic
    binding özelliğiyle bunları yakalar. Fakat kod okunabilirliğini artırmak için anotasyonlar kullanılmalıdır.
    UpdateOneBook fonksiyonu asnyc çalışmalarda bazen çakışmalardan kaynaklı yaşanabilecek hatalardan dolayı 409 Conflict
    koduyla da dönebilir.
    [PATCH]
    [FromBody] JsonPatchDocument<T> -> burada body aracılığıyla gelen nesne sarmallanıyor. Json olarak gelen veri üzerinde
    patch document patch operasyonunu gerçekleştiricek. Content type header üzerinde application/json-patch+json olarak belirtilmelidir.
    Patch işlemi için 2 pakete ihtiyacımız var:
    Microsoft.AspNetCore.JsonPatch
    Microsoft.AspNetCore.Mvc.NewtonsoftJson
    Patch istekleri array içerisinde tanımlanır. Array içerisinde ne kadar operasyon yapmak istiyorsak o kadar nesne tanımlarız.
    Eğer istenilen content type gelmediyse 415 Unsupported media types hatası döndürülebilir. Eğer patch ifadesi iyi yapılandırılmadıysa ki
    bununla çok karşılaşılır 400 Bad Request ile dönülebilir. PUT isteğinde olduğu gibi 409 Conflict ile de dönülebilir.
</summary>
*/

namespace bookDemo.Controllers
{
    [Route("api/books")]
    [ApiController] //404 ile beraber gelen ek bilgilerin sebebi. Sınıfa API davranışı kazandırıyor.
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books;
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = ApplicationContext
                .Books
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault(); //LINQ

            if (book is null)
                return NotFound(); //404

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest(); //400

                ApplicationContext.Books.Add(book);

                return StatusCode(201, book); //Created
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id,
             [FromBody] Book book)
        {
            // check book ?
            var entity = ApplicationContext
                .Books
                .Find(b => b.Id.Equals(id));

            if (entity is null)
                return NotFound(); //404

            // check id
            if(id != book.Id)
                return BadRequest(); //400

            ApplicationContext.Books.Remove(entity);
            book.Id = entity.Id;
            ApplicationContext.Books.Add(book);

            return Ok(book);
        }

        [HttpDelete]
        public IActionResult DeleteAllBooks()
        {
            ApplicationContext.Books.Clear();
            return NoContent(); //204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            var entity = ApplicationContext
                .Books
                .Find(b => b.Id.Equals(id));

            if(entity is null)
                return NotFound(new // anonim nesne.
                {
                    statusCode = 404,
                    message = $"Book with id:{id} could not found."
                }); // 404

            ApplicationContext.Books.Remove(entity);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, 
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            // check entity
            var entity = ApplicationContext
                .Books
                .Find(b => b.Id.Equals(id));

            if (entity is null)
                return NotFound(); // 404

            bookPatch.ApplyTo(entity);
            return NoContent(); // 204
        }
    }
}
