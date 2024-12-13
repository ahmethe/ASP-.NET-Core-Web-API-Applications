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
</summary>
*/

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly) 
                .AddNewtonsoftJson();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureSqlContext(builder.Configuration);
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
