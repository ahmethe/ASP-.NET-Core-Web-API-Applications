using NLog;
using Services.Contracts;

/* 
<summary>
    nlog.config dosyasını ana projemiz olan WebApi projesine ekledik. Bu dosya
    gerekli log ifadelerinin yazılması için bir şablon görevi görür ve içinde çeşitli
    tanımlamalar barındırır. Hangi seviyede log alınacak, loglar nereye yazılacak ve nerede 
    kaydedilecek gibi. (veritabanı, email, dosya, konsol vb.) Burada _logger ifadesinin concrete 
    hali NLog kütüphanesi fonksiyonu ile alınıyor. Ve fonksiyonlarda ilgili şekilde kullanılıyor.
    Gerekli durumlarda ayrı ayrı ortamlar için konfigürasyon dosyası hazırlanabilir. Mesela
    nlog.development.json, nlog.production.json, nlog.staging.json gibi dosyalar üretilerek bu gerçekleştirilir.
</summary>
*/

namespace Services
{
    public class LoggerManager : ILoggerService
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message) => _logger.Debug(message);

        public void LogError(string message) => _logger.Error(message);

        public void LogInfo(string message) => _logger.Info(message);

        public void LogWarning(string message) => _logger.Warn(message);
    }
}
