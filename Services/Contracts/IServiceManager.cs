/*
<summary>
    Repository katmanında kullandığımız mantıkla aynı şekilde burada da
    Unit Of Work Pattern kullanıyoruz. Bu pattern ile tüm serviceleri bir manager
    yapısı üzerinden kullanacağız.
</summary>
*/

namespace Services.Contracts
{
    public interface IServiceManager
    {
        IBookService BookService { get; }
    }
}
