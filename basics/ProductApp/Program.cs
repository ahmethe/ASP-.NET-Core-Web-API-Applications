/*
<summary>
    Bir yap�land�rma s�zkonusuysa bu program entry point olan Program.cs �zerinden yap�l�r.
    �lk �nce servis kayd� ger�ekle�tirilir daha sonra da app �zerinden bu ifade resolve edilir.
    Default bir logging ifadesi tan�ml� oldu�u i�in herhangi bir servis kayd� ger�ekle�tirilmedi.
</summary>
*/

namespace ProductApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
