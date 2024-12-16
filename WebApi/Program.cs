using NLog;
using WebApi.Extensions;

/* 
<summary>
    AddAplicationPart: Reflection alýyor parametre olarak. Çalýþma zamanýnda 
    iþlemin çözülebilmesi demektir bu.Controller farklý bir projeye taþýndýðý 
    için ve bu yapýnýn kullanýlabilmesi için bu fonksiyonun çaðrýlmasý gereklidir.
    EndPointsApiExplorer bu fonksiyonla beraber bu projedeki controllerlarý keþfedebilecek.
    AddNewtonsoftJson: Bu fonksiyon Patch iþleminin yapýlabilmesi için gerekli 2 kütüphaneden biri.
    Diðer indirilen kütüphane (JsonPatch) controllerlarýn olduðu Presentation katmanýna taþýndý. Çünkü
    Patch iþlemi controllerda tanýmlanmakta ve JsonPatchDocument sýnýfý bu actionun gerçekleþtirildiði
    fonksiyonda kullanýlmaktadýr.
</summary>
*/

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //NLog konfigürasyonu için konfigürasyon dosyasýnýn yolu veriliyor ve bu klasör adýnýn alýnmasý ve dosya adýnýn alýnýp birleþtirilmesi þeklinde gerçekleþtiriliyor.
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
