

/* 
<summary>
    Burada price aralığını kontrol ettiğimiz exceptionda aslında constructora bir message yollayıp
    daha sonra bunun kalıtım alınan abstract sınıfa gönderilmesini isteyebilirdik. Fakat statik bir yapı
    inşa edilmek istendi. Yani bu ifade kullanılırken özel olarak bir hata mesajı üretilmeyecek.
    Statik mesajlar üretileceği zaman bunlar tek bir yerden de yönetilebilir.
</summary>
*/

namespace Entities.Exceptions
{
    public class PriceOutOfRangeBadRequestException : BadRequestException
    {
        public PriceOutOfRangeBadRequestException()
            : base("Maximum price should be less than 1000 and greeater than 10.")
        {
        }
    }
}
