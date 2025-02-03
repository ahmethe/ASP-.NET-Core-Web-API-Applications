using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

/* 
<summary>
    out bir parametre modifierdır. Parametre olarak geçilen değişkenin değeri fonksiyon içerisinde belli olur.
    Bu attributeyi koyduğumuz fonksiyondan linkler içeren zengin bir dönüş alabiliriz.
</summary>
*/

namespace Presentation.ActionFilters
{
    public class ValidateMediaTypeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var acceptHeaderPresent = context.HttpContext
                .Request
                .Headers
                .ContainsKey("Accept");

            if(!acceptHeaderPresent)
            {
                context.Result = new BadRequestObjectResult("Accept header is missing !");
                return;
            }

            var mediaType = context.HttpContext
                .Request
                .Headers["Accept"]
                .FirstOrDefault();

            if(!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
            {
                context.Result = 
                    new BadRequestObjectResult("Media type not present." +
                    "Please add Accept header with required media type.");
                return;
            }

            context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);
        }
    }
}
