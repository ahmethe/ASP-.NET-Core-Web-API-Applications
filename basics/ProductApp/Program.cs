/*
<summary>
    Bir yapýlandýrma sözkonusuysa bu program entry point olan Program.cs üzerinden yapýlýr.
    Ýlk önce servis kaydý gerçekleþtirilir daha sonra da app üzerinden bu ifade resolve edilir.
    Default bir logging ifadesi tanýmlý olduðu için herhangi bir servis kaydý gerçekleþtirilmedi.
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
