using WebApi.Utilities.Formatters;

/* 
<summary>
    AddCusyomCsvFormatter: Yazdığımız Custom Formatterı kullanabilmek için
    IMvcBuilder için bir extension method yazdık. Input veya output için formatter
    yazılabilir. Biz bunu output formatter olacak şekilde yazdık ve konfigüre ettik.
</summary>
*/

namespace WebApi.Extensions
{
    public static class IMvcBuilderExtensions
    {
        public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder) =>
            builder.AddMvcOptions(config =>
                config.OutputFormatters
            .Add(new CsvOutputFormatter()));
    }
}
