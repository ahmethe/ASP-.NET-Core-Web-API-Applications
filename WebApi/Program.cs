using Microsoft.AspNetCore.Mvc;
using NLog;
using Services;
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

    Default olarak API i�erik pazarl���na kapal�d�r. AddControllers �zerinden bir konfig�rasyonla RespectBrowserAcceptHeader
    �zelli�i true yap�l�r. Desteklenmeyen ��kt�lar i�in 406 Not Acceptable ile d�n�� yapabilmek i�in de ReturnHttpNotAcceptable
    �zelli�i true yap�l�r. AddXmlDataContractSerializerFormatters ifadesi ile XML ��kt� d�n�lebilir hale gelinecek.
    
    API i�in controller yazarken controller yap�m�z� ControllerBase'den kal�t�m alarak ve [ApiController] attribute annotationunu
    yazarak ger�ekle�tiririz. Ve bu iki kullan�m API'ye birtak�m default �zellikler kazand�r�r. Bunlar mapleme, binding, validation
    gibi birtak�m default tan�mlard�r. Bunlar� istersek bask�layarak custom tan�mlamalar yapabiliriz. Biz burada do�rulamaya odakland���m�z
    i�in builder.Services �zerinden Configure methodu ile API'ye gelen invalid filter�n 400 bad request d�nmesini bast�rm�� olduk ve kendi kontrol�m�z
    ile invalid durumlarda 422 UnProcessableEntity kodunun d�n�lebilmesini sa�lad�k. Validationdaki ama� tan�mlad���m�z bir dizi kural�n istemci ile 
    sunucu aras�ndaki veri de�i�toku�u sa�lan�rken dikkate al�n�p al�nmad���d�r. Yerle�ik olarak bulunan attributeler ile yani data annotationlar ile kurallar
    tan�mlanacak. IValidationObject interfacesi implemente edilerek custom tan�mlar da yap�labilir.
    [ApiController] -> 400, attribute routing, binding, �oklu-par�al� dosya i�leme, problem details
    ControllerBase -> BadRequest, NotFound, TryValidateModel...
    Microsoft.AspNetCore.Mvc -> attribute ifadeleri. [Route], [Bind], [HttpGet]...

    X-Pagination ifademizin clientlar taraf�ndan okunup t�ketilebilmesi i�in bir izin tan�mlanmal�d�r. Bu da Cors konfig�rasyonuyla m�mk�nd�r.
    CORS: Cross Origin Resource Sharing.
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

            builder.Services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            })
            .AddXmlDataContractSerializerFormatters()
            .AddCustomCsvFormatter()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
            //.AddNewtonsoftJson();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureSqlContext(builder.Configuration);
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureLoggerService();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.ConfigureActionFilters();
            builder.Services.ConfigureCors();
            builder.Services.ConfigureDataShaper();
            builder.Services.AddCustomMediaTypes();
            builder.Services.AddScoped<IBookLinks, BookLinks>();

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

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
