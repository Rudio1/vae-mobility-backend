using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaeMobility.Domain.Catalog.Entities;
using VaeMobility.Infra.Data.Extensions;

namespace VaeMobility.Infra.Data.Catalog.Mappings;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(category => category.Id);
        builder.Property(category => category.Id).HasColumnName("id");
        builder.Property(category => category.Slug).HasColumnName("slug").HasMaxLength(120).IsRequired();
        builder.Property(category => category.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(category => category.ShortDescription).HasColumnName("short_description").HasMaxLength(500).IsRequired();
        builder.Property(category => category.Description).HasColumnName("description").IsRequired();
        builder.Property(category => category.SortOrder).HasColumnName("sort_order");
        builder.Property(category => category.SeoTitle).HasColumnName("seo_title").HasMaxLength(60);
        builder.Property(category => category.SeoDescription).HasColumnName("seo_description").HasMaxLength(160);
        builder.Property(category => category.SeoOgImage).HasColumnName("seo_og_image").HasMaxLength(500);
        builder.ConfigureAuditColumns();
        builder.Ignore(category => category.HasExplore);

        builder.HasMany(category => category.Images)
            .WithOne()
            .HasForeignKey(image => image.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(category => category.Images)
            .HasField("_images")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public sealed class CategoryImageConfiguration : IEntityTypeConfiguration<CategoryImage>
{
    public void Configure(EntityTypeBuilder<CategoryImage> builder)
    {
        builder.ToTable("category_images");
        builder.HasKey(image => image.Id);
        builder.Property(image => image.Id).HasColumnName("id");
        builder.Property(image => image.CategoryId).HasColumnName("category_id");
        builder.Property(image => image.Slot).HasColumnName("slot").HasMaxLength(30).IsRequired();
        builder.Property(image => image.Src).HasColumnName("src").HasMaxLength(500).IsRequired();
        builder.Property(image => image.Alt).HasColumnName("alt").HasMaxLength(300).IsRequired();
        builder.Property(image => image.Width).HasColumnName("width");
        builder.Property(image => image.Height).HasColumnName("height");
    }
}
