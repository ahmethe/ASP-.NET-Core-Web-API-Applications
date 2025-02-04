using HelloWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

/*
<summary>
    Sınıf tanımladık dönüş için. Bu sayede formatlı bir çıktı sağlamış olduk. Otomatik olarak serialize işlemi gerçekleşecek.
    Yani çıktı application/json şeklinde olacak. Bu da sınıfa kazandırılan default bir API davranışıdır.
</summary>
*/

namespace HelloWebAPI.Controllers
{
    [ApiController] //API yapısı kazandırır. Default hata sayfaları, default binding ifadeleri vb.
    [Route("home")] //parametre geçilen home ifadesi sunucu adresine eklenerek, burada tanımlı ifadelere erişilebilecek.
    public class HomeController : ControllerBase //controller özelliklerini kazanması için.
    {
        [HttpGet]
        public IActionResult GetMessage()
        {
            var result = new ResponseModel()
            {
                HttpStatus = 200,
                Message = "Hello ASP.NET Core Web API"
            };

            return Ok(result);
        }
    }
}
