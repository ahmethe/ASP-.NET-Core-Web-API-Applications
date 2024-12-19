using NLog;
using Services.Contracts;
using WebApi.Extensions;

/* 
<summary>
    AddAplicationPart: Reflection al�yor parametre olarak. �al��ma zaman�nda 
    i�lemin ��z�lebilmesi demektir bu.Controller farkl� bir projeye ta��nd��� 
    i�in ve bu yap�n�n kullan�labilmesi i�in bu fonksiyonun �a�r�lmas� gereklidir.
    EndPointsApiExplorer bu fonksiyonla beraber bu projedeki controllerlar� ke�fedebilecek.
    AddNewtonsoftJson: Bu fonksiyon Patch i�leminin yap�labilmesi i�in gerekli 2 k�t�phaneden biri.
    Di�er indirilen k�t�phane (JsonPatch) controllerlar�n oldu�u Presentation katman�na ta��nd�. ��nk�
    Patch i�lemi controllerda tan�mlanmakta ve JsonPatchDocument s�n�f� bu actionun ger�ekle�tirildi�i
    fonksiyonda kullan�lmaktad�r.

    WebApplication i�in yazd���m�z bir extension methodu kullanmak Services i�in yazd�klar�m�zdan
    farkl� olacak. ��nk� service ekleme safhas� build edip web applicationu elde etmeden �nce ger�ekle�iyor.
    Fakat bizim yazd���m�z method servislerden birine ihtiya� duyuyor. Bunu sa�lamak i�in gerekli ifade yaz�ld�.
    (GetRequiredService<>())
</summary>
*/

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //NLog konfig�rasyonu i�in konfig�rasyon dosyas�n�n yolu veriliyor ve bu klas�r ad�n�n al�nmas� ve dosya ad�n�n al�n�p birle�tirilmesi �eklinde ger�ekle�tiriliyor.
            LogManager.Setup().LoadConfigurationFromFile(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            // Add services to the container.

            builder.Services.AddControllers()
                .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly) 
                .AddNewtonsoftJson();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureSqlContext(builder.Configuration);
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureLoggerService();
            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILoggerService>();
            app.ConfigureExceptionHandler(logger);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (app.Environment.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
