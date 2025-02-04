/*
<summary>
    Development-Staging-Production olmak �zere temelde 3 ortam vard�r.
    Yapt���m�z uygulamada, development ve production ortamlar�n� ay�rd�k. Https development modda, IIS Express ise Production modda �al��acak �ekilde d�zenlendi.
    Production modda swagger ekran� istense de gelmeyecek �ekilde bir d�zenleme yap�ld�.
    Bu konfig�rasyon dosyalar�nda veritaban� ba�lant�lar�nda, loglama ifadelerinde farkl�l�klar olabilir.
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
            builder.Services.AddControllers(); //Controller tan�m�
            builder.Services.AddEndpointsApiExplorer(); //Servisin controlleri ke�fetmesi i�in.
            builder.Services.AddSwaggerGen(); //Swagger kullan�m� i�in servis kayd�.

            var app = builder.Build();
            
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger(); //kayd�n� yapt���m�z servislerin in�as�n� ger�ekle�tirdi�imiz app taraf�ndan kullan�lmas�n� sa�lad�k.
                app.UseSwaggerUI(); //aray�z� g�rebilmek i�in.
            }

            app.MapControllers(); //Controller map i�leminin ger�ekle�ebilmesi i�in.
            
            app.Run();
        }
    }
}
