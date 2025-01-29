using Entities.DataTransferObjects;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Services.Contracts;

/*
<summary>
    API; pagination, filtering, searching, data shaping gibi özelliklere de sahip. Link oluştururken bu ifadeler de tanımlanabilir.
    Keşfedilebilirlik dediğimiz özellik bu sayede gerçekleşir. Eğer hypermedia desteği verilemiyorsa keşfedilebilirlik için iyi bir dokümantasyon
    hazırlanmalıdır.
    Value kısmındaki linkler kaynağın bir elemanına ait tanımlayıcılardır. Value kısmından sonraki links alanı, bize ana route konumundayken 
    yani kaynak üzerinde hangi işlemleri yapabileceğimizi tanımlar. Örneğin en fazla satılan kitaplar, en çok okunan kitaplar gibi kaynakla ilişlili tanımları barındırır.
</summary>
*/

namespace Services
{
    public class BookLinks : IBookLinks
    {
        private readonly LinkGenerator _linkGenerator; //dahili olarak IoC kaydı yapılıdır.
        private readonly IDataShaper<BookDto> _dataShaper;

        public BookLinks(LinkGenerator linkGenerator, 
            IDataShaper<BookDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<BookDto> booksDto, 
            string fields, 
            HttpContext httpContext)
        {
            var shapedBooks = ShapeData(booksDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkedBooks(booksDto, fields, httpContext, shapedBooks);

            return ReturnShapedBooks(shapedBooks);
        }

        private LinkResponse ReturnLinkedBooks(IEnumerable<BookDto> booksDto, 
            string fields, 
            HttpContext httpContext, 
            List<Entity> shapedBooks)
        {
            var bookDtoList = booksDto.ToList();

            for (int index = 0; index < bookDtoList.Count(); index++)
            {
                var bookLinks = CreateForBook(httpContext, bookDtoList[index], fields);
                shapedBooks[index].Add("Links", bookLinks);
            }

            var bookCollection = new LinkCollectionWrapper<Entity>(shapedBooks);
            CreateForBooks(httpContext, bookCollection);
            return new LinkResponse { HasLinks = true, LinkedEntities = bookCollection };
        }

        private List<Link> CreateForBook(HttpContext httpContext, BookDto bookDto, string fields)
        {
            var links = new List<Link>()
            {
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}" +
                    $"/{bookDto.Id}",
                    Rel = "self",
                    Method = "GET"
                },
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",
                    Rel = "create",
                    Method = "POST"
                }
            };
            return links;
        }

        private LinkCollectionWrapper<Entity> CreateForBooks(HttpContext httpContext,
            LinkCollectionWrapper<Entity> bookCollectionWrapper)
        {
            bookCollectionWrapper.Links.Add(new Link() { 
                Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",
                Rel = "self",
                Method = "GET"
            });
            return bookCollectionWrapper;
        }

        private LinkResponse ReturnShapedBooks(List<Entity> shapedBooks)
        {
            return new LinkResponse { ShapedEntities = shapedBooks }; //HasLinks'e atama yapmadık çünkü boolean ifadeler zaten default false.
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType
                .SubTypeWithoutSuffix
                .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private List<Entity> ShapeData(IEnumerable<BookDto> booksDto, string fields)
        {
            return 
                _dataShaper.ShapeData(booksDto, fields)
                .Select(b => b.Entity)
                .ToList();
        }
    }
}
