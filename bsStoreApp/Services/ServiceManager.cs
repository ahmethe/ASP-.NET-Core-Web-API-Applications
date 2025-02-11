using Services.Contracts;

/*
<summary>
    Burada gerekli ifadelerin IoC kaydı yapıldı. Parametre olarak geçilen mapper ve logger
    ifadelerinin daha önceden IoC kaydı yapıldığı için framework bunu otomatik olarak çözümleyecektir.
    BookManager içinde kullanılan BookLinks ifadesinin de IoC kaydı yapılmıştı. UserManager ifadesi yerleşik
    olarak kullanılan bir ifadedir. Aynı şekilde configuration da dahili olarak var olan bir yapıdır. RepositoryManager
    ifadesinin kaydı zaten en başta yapılmıştı.
</summary>
*/

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IBookService _bookService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICategoryService _categoryService;

        public ServiceManager(IBookService bookService, 
            IAuthenticationService authenticationService, 
            ICategoryService categoryService)
        {
            _bookService = bookService;
            _authenticationService = authenticationService;
            _categoryService = categoryService;
        }

        public IBookService BookService => _bookService;
        public IAuthenticationService AuthenticationService => _authenticationService;
        public ICategoryService CategoryService => _categoryService;
    }
}
