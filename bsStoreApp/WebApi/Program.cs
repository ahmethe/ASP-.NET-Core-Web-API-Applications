using Microsoft.AspNetCore.Mvc;
using NLog;
using Services;
using Services.Contracts;
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

    WebApplication için yazdýðýmýz bir extension methodu kullanmak Services için yazdýklarýmýzdan
    farklý olacak. Çünkü service ekleme safhasý build edip web applicationu elde etmeden önce gerçekleþiyor.
    Fakat bizim yazdýðýmýz method servislerden birine ihtiyaç duyuyor. Bunu saðlamak için gerekli ifade yazýldý.
    (GetRequiredService<>())

    Default olarak API içerik pazarlýðýna kapalýdýr. AddControllers üzerinden bir konfigürasyonla RespectBrowserAcceptHeader
    özelliði true yapýlýr. Desteklenmeyen çýktýlar için 406 Not Acceptable ile dönüþ yapabilmek için de ReturnHttpNotAcceptable
    özelliði true yapýlýr. AddXmlDataContractSerializerFormatters ifadesi ile XML çýktý dönülebilir hale gelinecek.
    
    API için controller yazarken controller yapýmýzý ControllerBase'den kalýtým alarak ve [ApiController] attribute annotationunu
    yazarak gerçekleþtiririz. Ve bu iki kullaným API'ye birtakým default özellikler kazandýrýr. Bunlar mapleme, binding, validation
    gibi birtakým default tanýmlardýr. Bunlarý istersek baskýlayarak custom tanýmlamalar yapabiliriz. Biz burada doðrulamaya odaklandýðýmýz
    için builder.Services üzerinden Configure methodu ile API'ye gelen invalid filterýn 400 bad request dönmesini bastýrmýþ olduk ve kendi kontrolümüz
    ile invalid durumlarda 422 UnProcessableEntity kodunun dönülebilmesini saðladýk. Validationdaki amaç tanýmladýðýmýz bir dizi kuralýn istemci ile 
    sunucu arasýndaki veri deðiþtokuþu saðlanýrken dikkate alýnýp alýnmadýðýdýr. Yerleþik olarak bulunan attributeler ile yani data annotationlar ile kurallar
    tanýmlanacak. IValidationObject interfacesi implemente edilerek custom tanýmlar da yapýlabilir.
    [ApiController] -> 400, attribute routing, binding, çoklu-parçalý dosya iþleme, problem details
    ControllerBase -> BadRequest, NotFound, TryValidateModel...
    Microsoft.AspNetCore.Mvc -> attribute ifadeleri. [Route], [Bind], [HttpGet]...

    X-Pagination ifademizin clientlar tarafýndan okunup tüketilebilmesi için bir izin tanýmlanmalýdýr. Bu da Cors konfigürasyonuyla mümkündür.
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

            //NLog konfigürasyonu için konfigürasyon dosyasýnýn yolu veriliyor ve bu klasör adýnýn alýnmasý ve dosya adýnýn alýnýp birleþtirilmesi þeklinde gerçekleþtiriliyor.
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
