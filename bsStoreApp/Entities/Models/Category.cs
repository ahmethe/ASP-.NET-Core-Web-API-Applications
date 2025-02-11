/*
<summary>
    Kategori fiziksel olarak doğrudan kitapla ilişkili olmayacak. Somut bir ifade olmayacak.
    Bir referans tanımlanacak. İstenirse bu tanım hiç yapılmayadabilir.
    Koleksiyon navigation propertyi kaldırmanın dışında JSON'a ilgili ifadeyi görmezden gelmesini sağlayacak
    şekilde bir konfigürasyon yapmak da tek yönlü çıktı almanın bir yoludur.
</summary>
*/

namespace Entities.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public String? CategoryName { get; set; }

        // Ref : Collection navigation property
        // public ICollection<Book> Books { get; set; } İlgili ilişki kurulduktan sonra bu satır yorum satırı yapılırsa tek yönlü bir çıktı alınmış olur.
    }
}
