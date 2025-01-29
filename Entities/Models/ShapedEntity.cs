/* 
<summary>
    Genel bir ShapedEntity tanımı. Her varlık için PK olarak id tanımlanacağı için tanımlanan Entity
    sınıfına Id property kazandırıldı.
</summary>
*/

namespace Entities.Models
{
    public class ShapedEntity
    {
        public int Id { get; set; } //read-only struct. Referans aldırmamıza gerek yok.
        public Entity Entity { get; set; }

        public ShapedEntity()
        {
            Entity = new Entity();
        }
    }
}
