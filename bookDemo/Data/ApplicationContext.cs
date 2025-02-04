using bookDemo.Models;

/*
<summary>
    Static tanımlı olduğu için bir yerde sınıf üzerinde yapılan bir değişiklik bu sınıfı kullanan
    her yerden farkedilebilecek. Yani bellekte sabit bir alanda veri tutuluyor olacak. Ve her erişim buraya
    yapılmış olacak. Buradaki veriler InMemory. Yani veriler kalıcı değil, yapılan değişiklikler uygulama kapatılınca
    sıfırlanacak.
</summary>
*/

namespace bookDemo.Data
{
    public static class ApplicationContext
    {
        public static List<Book> Books { get; set; } // static olduğu içi class member.
        static ApplicationContext()
        {
            Books = new List<Book>()
            {
                new Book() { Id = 1, Title = "Karagöz ve Hacivat", Price = 75},
                new Book() { Id = 2, Title = "Mesnevi", Price = 150},
                new Book() { Id = 3, Title = "Dede Korkut", Price = 75}
            };
        }
    }
}
