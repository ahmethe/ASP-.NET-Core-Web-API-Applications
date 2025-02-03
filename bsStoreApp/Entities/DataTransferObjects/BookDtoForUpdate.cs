
/* 
<summary>
    Data Transfer Obejcts (record) -> readonly, immutable(Değişmez. Değişecekse yenisi oluşmalı)
    LINQ desteğine sahip olmalı. Reference tiptir. Ctor(DTO) tanımı şansı verir.
</summary>
*/

using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public record BookDtoForUpdate : BookDtoForManipulation
    {
        [Required]
        public int Id { get; init; }
    }
}
