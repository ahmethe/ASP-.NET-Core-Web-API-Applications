
/* 
<summary>
    Data Transfer Obejcts (record) -> readonly, immutable(Değişmez. Değişecekse yenisi oluşmalı)
    LINQ desteğine sahip olmalı. Reference tiptir. Ctor(DTO) tanımı şansı verir.
</summary>
*/

namespace Entities.DataTransferObjects
{
    public record BookDtoForUpdate(int Id, String Title, decimal Price);
}
