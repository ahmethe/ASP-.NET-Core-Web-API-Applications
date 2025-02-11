/*
<summary>
    "Her kitabın bir kategorisi var.". Bu ilişki incelendiği zaman Book nesnesi somut olarak
    bir CategoryId değerine sahip olmalıdır.
</summary>
*/

namespace Entities.Models
{
    public class Book
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public decimal Price { get; set; }

        // Ref: Navigation Property
        public int CategoryId { get; set; } // Bu ifadeye karşılık bir alan, fiziksel bir karşılığı olmalı.
        public Category Category { get; set; } // Bu ifadeye karşılık bir şey oluşmayacak.
    }
}
