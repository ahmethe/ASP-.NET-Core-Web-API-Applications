using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;

/*
<summary>
    DTO ve DTO'yu record type olarak tanımlamamızın sebebi, presentation layerdaki controllerdan
    services katmanındaki ilgili servise veriyi iletirken değişmezliği garanti etmektir. init anahtar kelimesiyle
    bu değişmezliği sağlamış oluyoruz.
</summary>
*/

namespace Entities.DataTransferObjects
{
    public record LinkParameters
    {
        public BookParameters BookParameters { get; set; }
        public HttpContext HttpContext { get; set; }
    }
}
