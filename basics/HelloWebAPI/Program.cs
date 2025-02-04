/*
<summary>
    Development-Staging-Production olmak üzere temelde 3 ortam vardýr.
    Yaptýðýmýz uygulamada, development ve production ortamlarýný ayýrdýk. Https development modda, IIS Express ise Production modda çalýþacak þekilde düzenlendi.
    Production modda swagger ekraný istense de gelmeyecek þekilde bir düzenleme yapýldý.
    Bu konfigürasyon dosyalarýnda veritabaný baðlantýlarýnda, loglama ifadelerinde farklýlýklar olabilir.
    Development modda sqlite, production modda SQLServer kullanmak gibi.
</summary>
*/

namespace HelloWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Service (Container)
            builder.Services.AddControllers(); //Controller tanýmý
            builder.Services.AddEndpointsApiExplorer(); //Servisin controlleri keþfetmesi için.
            builder.Services.AddSwaggerGen(); //Swagger kullanýmý için servis kaydý.

            var app = builder.Build();
            
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger(); //kaydýný yaptýðýmýz servislerin inþasýný gerçekleþtirdiðimiz app tarafýndan kullanýlmasýný saðladýk.
                app.UseSwaggerUI(); //arayüzü görebilmek için.
            }

            app.MapControllers(); //Controller map iþleminin gerçekleþebilmesi için.
            
            app.Run();
        }
    }
}
