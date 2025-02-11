using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

/*
<summary>
    ASP NET Core MVC pattern ile çalışırken dosyalama yapıyorsak, bu dosyanın içeriği ile ilgili 
    bilgi ve çok daha fazlasına erişme amacımız varsa IFormFile interface yapısını kullanırız.
    Dosyaları yüklerken bulut hizmeti veren yerlerin servisleri kullanılarak dosyalar genelde bulutta depolanır.
    API default davranışını değiştirdiğimiz için hata durumunu kendimiz ele almak zorundayız. Değiştirmemiş olsaydık 400 ile dönecekti.
    Klasik web yaklaşımında istediğiniz dosyaya bastığınızda -örneğin bir resime- bunun açılıp ekrana yansımasını bekleriz. Fakat API uygulamalarında
    bunun indirilmesini(download) sağlarız.
</summary>
*/

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file) 
        {
            if(!ModelState.IsValid) // Eğer file ifadesi boş gönderilirse bu hata durumu ele alınıyor.
                return BadRequest();

            // folder
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Media");
            
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // path URLe çevrilip veritabanında saklanabilir veya direkt dosyanın kendisi de saklanabilir.
            var path = Path.Combine(folder, file.FileName);

            // stream(binary)-async
            using (var stream = new FileStream(path, FileMode.Create)) // stream işlemleri maliyetlidir. Bu yüzden using yapısı içerisinde yapılacak.
            {
                await file.CopyToAsync(stream);
            }

            // response body
            return Ok(new
            {
                File = file.FileName,
                Path = path,
                Size = file.Length
            });
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] string fileName)
        {
            // filePath
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Media", fileName);

            // ContentType(provider) : (MIME)
            var provider = new FileExtensionContentTypeProvider(); // Bu provider ile content type belirlemeye çalışacağız. Sunucular her dosyaya yanıt veremeyebilir.
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream"; // ilgili dosyanın indirilebilmesi sağlanacak.
            }

            // Read
            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(bytes, contentType, Path.GetFileName(filePath));
        }
    }
}
