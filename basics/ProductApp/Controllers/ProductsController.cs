using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;

/*
<summary>
    Readonly bir ifadeye değer ataması ya constructorda, ya da tanımlandığı yerde gerçekleştirilir.
    ILogger bir interface yapısıdır yani referans tutucudur. Yani ILogger ile tanımlanan ifadeye new anahtar kelimesi
    ile bir tanımlama yapılacak. Fakat dahili olarak bu ifadenin IoC kaydı yapıldığı için bunu direkt olarak kullanabileceğiz.
    Loglama işlemi bir veritabanına da yapılabilir. Burada console ve debug outputa loglama yapıldı.
</summary>
*/

namespace ProductApp.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger; //Yerleşik olarak gelen bir interface yapısı. Implementasyonu dahili olarak yapılmış durumda.
        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = new List<Product>()
            {
                new Product() {Id = 1, ProductName = "Computer"},
                new Product() {Id = 2, ProductName = "Keyboard"},
                new Product() {Id = 3, ProductName = "Mouse"}
            };

            _logger.LogInformation("GetAllProducts action has been called.");

            return Ok(products);
        }

        [HttpPost]
        public IActionResult GetAllProducts([FromBody] Product product)
        {
            _logger.LogWarning("Product has been created");
            return StatusCode(201); //Created.
        }
    }
}
