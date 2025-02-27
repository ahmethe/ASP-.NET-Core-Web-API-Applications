﻿using System.ComponentModel.DataAnnotations;

/* 
<summary>
    Burada her ne kadar Single Responsibility ihlal edilmiş gibi görünse de
    bu ihlal abstract tanım ile az da olsa yumuşatılmış durumda.
</summary>
*/

namespace Entities.DataTransferObjects
{
    public abstract record BookDtoForManipulation
    {
        [Required(ErrorMessage = "Title is a required field.")]
        [MinLength(2, ErrorMessage = "Title must consist of at least 2 characters.")]
        [MaxLength(50, ErrorMessage = "Title must consist of at maximum 50 characters.")]
        public String Title { get; init; }

        [Required(ErrorMessage = "Price is a required field.")]
        [Range(10, 1000)]
        public decimal Price { get; init; }
    }
}
