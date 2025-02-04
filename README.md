# WEB API
Application Programming Interface (API), tümleşik (integrated) web uygulamaları geliştirmek ve kurmak için bir dizi tanımlar ve protokollerden oluşan bir uygulama programlama arayüzüdür.
API; sizin ürün ya da servisinizin, uygulama detayları ve teknoloji tercihlerinden bağımsız olarak başka ürün ya da servisler ile iletişim kurmasına olanak tanır.
Bir web sitesi yapılırken, SPA yapılırken, mobil uygulama geliştirirken veya servis ya da hizmet gerçekleştirirken API teknolojisinden faydalanılabilir.
API teknolojisi genellikle REST yaklaşımını benimser.

## YAZILIM MİMARİSİ
Bir yazılım geliştirilirken katmanlı mimari tercih edildiyse verilerin organizasyonu ve kalıcı olarak saklanması işlemi Persistance veya Data Access adı verilen katmanda gerçekleştirilir. 
Yazılımın sahip olduğu iş mantığı, kurallar, yetkilendirme, fonksiyonel ve fonksiyonel olmayan tüm ihtiyaçlar ise Services katmanında gerçekleştirilir. Geliştirilen bu yazılım API aracılığıyla 
Presentation katmanından kullanıcı kullanımına sunulur. Bu bir backend sistemidir. Frontend sistemler ise, web uygulamaları, mobil cihazlar IoT cihazları olabilir. Presentation katmanı kullanıcıdan 
gelen isteği alır ve Services katmanına aktarır. Gerekli prosedürler işlendikten sonra cevap kullanıcıya dönülür.
Günümüzde minimal API yaklaşımı ile HTTP verbleri, veri organizasyonu ve iş mantığı tek bir sayfada gerçeklenebiliyor. Bu da tıpkı katmanlı mimari gibi bir backend sistemine işaret eder. 
Bu durum kısaca Backend as a service (BaaS) veya Backend as a function (BaaF) şeklinde ifade edilir.

## HTTP PROTOKOLÜ
HTTP protokolünü anlayabilmek için istemci-sunucu kavramlarını kullanmalıyız. İstemci, daha önce bahsedildiği gibi bir web uygulaması, mobil cihaz veya IoT cihazı olabilir. Sunucu kavramı ise 
bizim kurduğumuz backend sistemi ve API kullanımına dayanır. İstemci bir HTTP verb kullanarak, header ve body ile beraber istekte bulunur. Header kısmında gönderilen istekle ilgili tanımlayıcı bilgiler
verilirken body kısmında ise isteğin içeriği organize edilir. Sunucu gelen isteğe durum kodu, header ve body bölümlerinden oluşan bir cevapla karşılık verir. Bu mekanizma REST yaklaşımının temeli olan 
temsil ifadesini tanımlar. İstemci tarafından bir istek yardımıyla gönderilen bilgi sunucu tarafında bir temsil buldu ve sunucu tarafından bu isteğe karşılık bir cevap üretilerek istemci-sunucu arasında
veri değiş tokuşu sağlanmış oldu.
İstemci-sunucu arasındaki bu iletişim sürekli değildir. Yani sunucu istemciyi tanımak için ekstra bir bilgi saklamaz. Tüm iletişim istekler üzerinden sağlanır. Bu stateless(durumsuzluk) kavramıyla ifade edilir.
Yani yetkilendirme gerektiren bir işlem yapılacaksa istemci bunu göndereceği isteğe yerleştirmelidir.

## İSTEK(REQUEST)
Bir istek, verb-headers-content(body) bölümlerinden oluşur. Verb kısmında kullanılmak istenen HTTP verb ifadesi yazılır. GET sunucudan kaynak isteme amacıyla kullanılır. POST sunucuda yeni bir kaynak oluşturmak
için kullanılır. İkincil olarak bir süreci tetikleme amacıyla da kullanılabilir. PUT kaynak güncelleme amacıyla kullanılır. İkincil olarak kaynak oluşturma amacıyla da kullanılabilir. PATCH kısmi olarak bir 
kaynağın sahip olduğu alanlardan bir veya birkaçını güncellemek için kullanılır. DELETE bir kaynak silmek için kullanılır. Headers kısmı, daha önce ifade edildiği gibi istekle ilgili üst bilgilerin, tanımlayıcı bilgilerin
yer aldığı alandır. Örneğin içeriğin formatı(content Type) json, xml, csv veya hangi format kullanıldıysa bunu belirtmek için kullanılır. Content Length içeriğin boyutu hakkında bilgi verir. Authorization eğer 
yetki gerektiren bir işlem gerçekleştiriliyorsa bu parametre yardımıyla yetki alma işlemi gerçekleştirilir. Buna benzer daha birçok ifade Headers kısmında kullanılabilir. (accept, cookies vb.) Content kısmı isteğin içeriğini
barındırır. HTML, CSS, JS, XML, JSON formatta olabilir. Bazı HTTP eylemlerinde kullanılmaz. İsteği gerçekleştirmek için yardımcı bilgilerin sunulduğu alan olarak ifade edilebilir.

## CEVAP(RESPONSE)
Bir cevap, status code-headers-content bölümlerinden oluşur. Status code, istekle ilgili gerçekleştirilen eylemin durumuyla ilgili bilgi verir. 
- 100-199 arası kodlar bilgi(information)
- 200-299 başarı(success)
- 300-399 yeniden yönlendirme(redirection)
- 400-499 istemci hataları(client errors)
- 500-599 sunucu hataları(server errors) olarak operasyonu tanımlar.

Headers istek kısmına benzer şekilde cevapla ilgili olan üst bilgileri tanımlar. Daha önce tanımı veriken Content Type ve Content Lengthe ek olarak Expires alanı ile cevabın ne zaman geçersiz sayılacağı ile ilgili bilgi verilebilir.
API versiyonu ve sayfalama ile API hakkında çok daha teknik bilgiler de verilebilir. Oluşan cevabın içeriği de content kısmında ifade edilir.

## IDEMPOTENT
Sonucu değiştirmeden defalarca uygulanabilen işlemi ifade eder. İşlem sonuçları GET, PUT, PATCH ve DELETE eylemlerinde aynıdır. POST idempotent değildir. Tekrar tekrar çalışması durumunda kaynak tarafında değişikliğe neden olur.

## REST (REPRESENTATIONAL STATE TRANSFER / TEMSİLİ DURUM TRANSFERİ)
Kullanıcı bir HTTP verb ile kaynağa ait bir URI(query string) veya endpointe gider.
REST yapısına göre, istemciden örneğin bir GET isteği geldiği takdirde sunucu elindeki kaynakların temsili bir organizasyonunu, header bölümündeki tanımlayıcı bilgiler ile beraber, body kısmında istemciye döner. REST SOAP gibi değildir. 
SOAP da body kısmındaki içerik XML formatında olmak zorundadır. REST daha esnek bir yapıya sahiptir. En iyi pratikler bunun JSON olması gerektiğini söyler. Fakat bir zorunluluk değildir.

## BEST PRACTICES
- URI'ler kaynakların bir parçasıdır. /books, books/lastest, /books/mostread 
- Doğrudan kaynakla alakası olmayan ifadeleri bir route veya query string ifadesi ile verebiliriz. /books?sort=title, books?page=1, books?pageNumber=1&pageSize=10
- URI tasarlarken fiillerden uzak durup isimler kullanılır. GetBooks yerine books gibi. Ve kaynak isimlerinin çoğul olması tercih edilir.
- URI içerisinde tanımlayıcılar kullanılabilir. Bu tanımlayıcılar anahtar değer olmak zorunda değil./books/1, /books/devlet, books/bs-101
- Sonuçların kendini tanımlamasını sağlarız. (HATEOAS)
- Programlı gezinmeye izin verilir. (/users -> /users/10 veya /users/zafer)
- Sayfalama, sıralama, filtreleme ve arama desteği sağlanmalıdır.
- Önbellek desteği.(caching) API üzerindeki yükü ve stresi azaltır.
- Sorgu sınırı gelmelidir. Sorguya süreli bir sınır getirilebilir.
- Veri şekillendirme sunulmalıdır.
- Versiyonlama ve belgelendirme yapılmalıdır.
- API yayınlandıktan sonra değiştirilmemelidir. I ifadesi interface anlamına gelir. Değişiklik versiyon değişimi anlamına gelir.

Bu özellikler her API için olmazsa olmaz değil. Fakat esneklik ve güç kazandırır.
Hypermedia desteği pragmatik bir yaklaşımla ele alınır. Bu da bize çoğu API için bunun gerekli olmadığını söyler. Çünkü emek yoğun bir süreçtir. Ayrıca iyi tasarlanmış bir API bu desteği daha sonradan alabilecek durumdadır. 
API keşfedilebilirliğini(discoverability) artırır.

## API OLGUNLAŞMA SEVİYELERİ
- **Level 3** – HATEOAS (Hypermedia as the Engine of Application State) support  
- **Level 2** – Birden fazla kaynak ve HTTP methodları  
- **Level 1** – Birden fazla kaynak, sadece POST  
- **Level 0** – Tek kaynak, sadece POST

## curl: CLIENT URL
- Unix bazlı sistemlerde mevcut olan bir komuttur. Ücretsiz bir URL transfer kütüphanesidir.  
- URL'lerin bağlanabilirliğini kontrol etmek ve veri transferi için geliştirilmiş bir araçtır.
- HTTP de içinde olmak üzere birçok protokolü destekler.
- Postman, Swagger gibi araçlar curl komut setini kullanır.
