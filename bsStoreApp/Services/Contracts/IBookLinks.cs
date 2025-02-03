using Entities.DataTransferObjects;
using Entities.LinkModels;
using Microsoft.AspNetCore.Http;

/* 
<summary>
    Microsoft.AspNetCore.Mvc.Abstractions paketini Entities projesine kurduk. Fakat servis projesinde
    bu projenin referansı olduğu için bu paketi burada da kullanabiliyoruz.
</summary>
*/

namespace Services.Contracts
{
    public interface IBookLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<BookDto> booksDto,
            string fields, HttpContext httpContext);
    }
}
