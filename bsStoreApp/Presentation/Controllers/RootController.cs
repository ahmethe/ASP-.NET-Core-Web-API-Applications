using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
/*
<summary>
    GetRoot methodundaki Name ifadesi BooksController'daki ilgili methodlarda da aynı şekilde tanımlı olmalı. Çünkü name ifadesine bağlı link üretiyoruz.
</summary>
*/

namespace Presentation.Controllers
{
    [ApiController] //API standart davranışını kazandırıp bu controllerin bir API controller olması için.
    [Route("api")]
    public class RootController : ControllerBase //Controller özelliği kazandırmak için.
    {
        private readonly LinkGenerator _linkGenerator;

        public RootController(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        [HttpGet(Name = "GetRoot")] 
        public async Task<IActionResult> GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if (mediaType.Contains("application/vnd.btkakademi.apiroot"))
            {
                var list = new List<Link>() 
                { 
                    new Link()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new {}),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(BooksController.GetAllBooksAsync), new {}),
                        Rel = "books",
                        Method = "GET"
                    },
                    new Link()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(BooksController.CreateOneBookAsync), new {}),
                        Rel = "books",
                        Method = "POST"
                    }
                };

                return Ok(list);
            }
            return NoContent(); // 204
        }
    }
}
