using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;

/*
<summary>   
    Kullanıcı sunucudaki bir kaynağa erişmek için login olabileceği tanımlı bir endpointe body kısmında kullanıcı adı ve şifresi ile bir istekte bulunur. Şayet basic authentication kullanılıyorsa kullanıcı kaynak ile iletişime geçebilmek için her defasında header kısmında
    authorization etiketi ile kullanıcı adı ve şifresini göndermelidir. Fakat kullanışlı bir yöntem olarak bunun yerine JWT(JSON Web Token) kullanılır. Kullanıcı giriş yaptığı zaman bir JWT döndürülür. JWT'nin içerisinde claims olarak adlandırılan bilgiler bulunur.
    Claims içerisinde username, subject, user roles ve kullanışlı diğer bilgiler olabilir.
    JWT encrypt(şifrelenmiş) bir yapı değildir. Kodlanmış(encoded) bir yapıdır. Yani Base64 kullanan herhangi bir decoder yardımıyla bu kod çözülebilir. Dolayısıyla token içerisine hassas bilgiler -kullanıcı şifresi gibi- kesinlikle konulmamalıdır.
    Kullanıcı daha sonra korumalı kaynaklara erişmek istediği zaman aldığı bu tokeni Authorization: Bearer Token set ederek ve header içerisinde Authorization tag aracılığıyla sunucuya göndermelidir. Eğer kullanıcı doğrulanamazsa 401 Unauthorized kodu ile dönüş yapılır.
    Şayet yetkisiz bir erişim söz konusuysa da 403 Forbidden hata kodu ile dönüş yapılır. Aksi durumda yani doğrulama gerçekleştiyse ilgili response kullanıcıya dönülür.
    Bir JWT sırasıyla Header, Payload, Signature olmak üzere 3 bölümden oluşur. JWT Stringi bu 3 bölümü 2 nokta ile ayırır. Header kısmında kullanılan algoritma ve token tipi belirtilir. Payload kısmında giriş yapan kullanıcı hakkında bilgiler verilir. Söz konusu bilgiler yukarıda 
    belirtilen bilgilere ek olarak tokenin geçerlilik tarihi, üretilme tarihi vb. de olabilir. Signature kısmında ise bir secret key aracılığıyla doğrulama gerçekleştirilir. Yani bu token geçerli mi ve gerçekten bu kaynak tarafından mı üretildi buna cevap aranır.
    
    İstemci authentication işlemini geçekleştirdikten sonra bir access token ve refresh token alır. Access token yardımıyla korunan kaynakların endpointlerine erişim sağlayabilir. Her access tokenin bir expire olma zamanı vardır. Bu zaman geldiğinde ise istemci bu sefer refresh token yardımıyla
    bir istekte bulunur ve yeni bir refresh token ve access token elde eder. Bu sayede access token yenilenmiş ve erişim kesilmemiş olur. Tekrar tekrar oturum açma işlemi gerçekleşmez.

    Burada POST verb süreç tetiklemek için kullanılıyor.
</summary>
*/

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AuthenticationController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            var result = await _service
                .AuthenticationService
                .RegisterUser(userForRegistrationDto);

            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _service.AuthenticationService.ValidateUser(user))
                return Unauthorized(); //401

            var tokenDto = await _service
                .AuthenticationService
                .CreateToken(populateExp: true);

            return Ok(tokenDto);
        }

        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _service
                .AuthenticationService
                .RefreshToken(tokenDto);

            return Ok(tokenDtoToReturn);
        }
    }
}
