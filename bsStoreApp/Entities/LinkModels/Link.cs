/* 
<summary>
    Serileştirme işlemi için default constructor tanımı yapıldı. Diğer constructor ise
    uygulama detaylarında kullanılacak.
</summary>
*/

namespace Entities.LinkModels
{
    public class Link
    {
        public string? Href { get; set; }
        public string? Rel { get; set; }
        public string? Method { get; set; }

        public Link()
        {
            
        }

        public Link(string? href, 
            string? rel, 
            string? method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
