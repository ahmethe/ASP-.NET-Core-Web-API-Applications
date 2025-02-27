﻿using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/*
<summary>
    Çekirdek data ekleneceği zaman Id alanı girilmelidir.
</summary>
*/

namespace Repositories.EFCore.Config
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId); //PK
            builder.Property(c => c.CategoryName).IsRequired();

            builder.HasData(
                new Category()
                {
                    CategoryId = 1,
                    CategoryName = "Computer Science"
                },
                new Category()
                {
                    CategoryId = 2,
                    CategoryName = "Network"
                },
                new Category()
                {
                    CategoryId = 3,
                    CategoryName = "Database Management Systems"
                }
            );
        }
    }
}
