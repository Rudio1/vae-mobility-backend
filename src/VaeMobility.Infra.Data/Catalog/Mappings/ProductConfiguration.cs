using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaeMobility.Domain.Catalog.Entities;
using VaeMobility.Infra.Data.Extensions;

namespace VaeMobility.Infra.Data.Catalog.Mappings;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(product => product.Id);
        builder.Property(product => product.Id).HasColumnName("id");
        builder.Property(product => product.CategoryId).HasColumnName("category_id");
        builder.Property(product => product.Slug).HasColumnName("slug").HasMaxLength(120).IsRequired();
        builder.Property(product => product.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(product => product.Status).HasColumnName("status").HasMaxLength(20).IsRequired();
        builder.Property(product => product.Featured).HasColumnName("featured");
        builder.Property(product => product.SortOrder).HasColumnName("sort_order");
        builder.Property(product => product.Summary).HasColumnName("summary").HasMaxLength(160).IsRequired();
        builder.Property(product => product.Description).HasColumnName("description").IsRequired();
        builder.Property(product => product.PriceAmount).HasColumnName("price_amount").HasColumnType("decimal(18,2)");
        builder.Property(product => product.Currency).HasColumnName("currency").HasMaxLength(3);
        builder.Property(product => product.WarrantyMonths).HasColumnName("warranty_months");
        builder.Property(product => product.SeoTitle).HasColumnName("seo_title").HasMaxLength(60);
        builder.Property(product => product.SeoDescription).HasColumnName("seo_description").HasMaxLength(160);
        builder.Property(product => product.SeoOgImage).HasColumnName("seo_og_image").HasMaxLength(500);
        builder.ConfigureAuditColumns();

        builder.HasMany(product => product.Images).WithOne().HasForeignKey(image => image.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(product => product.Highlights).WithOne().HasForeignKey(item => item.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(product => product.Specs).WithOne().HasForeignKey(item => item.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(product => product.Variants).WithOne().HasForeignKey(item => item.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(product => product.Badges).WithOne().HasForeignKey(item => item.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(product => product.Related).WithOne().HasForeignKey(item => item.ProductId).OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(product => product.Images).HasField("_images").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(product => product.Highlights).HasField("_highlights").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(product => product.Specs).HasField("_specs").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(product => product.Variants).HasField("_variants").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(product => product.Badges).HasField("_badges").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(product => product.Related).HasField("_related").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("product_images");
        builder.HasKey(image => image.Id);
        builder.Property(image => image.Id).HasColumnName("id");
        builder.Property(image => image.ProductId).HasColumnName("product_id");
        builder.Property(image => image.Src).HasColumnName("src").HasMaxLength(500);
        builder.Property(image => image.Alt).HasColumnName("alt").HasMaxLength(300);
        builder.Property(image => image.Width).HasColumnName("width");
        builder.Property(image => image.Height).HasColumnName("height");
        builder.Property(image => image.SortOrder).HasColumnName("sort_order");
        builder.Property(image => image.IsCard).HasColumnName("is_card");
    }
}

public sealed class ProductHighlightConfiguration : IEntityTypeConfiguration<ProductHighlight>
{
    public void Configure(EntityTypeBuilder<ProductHighlight> builder)
    {
        builder.ToTable("product_highlights");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).HasColumnName("id");
        builder.Property(item => item.ProductId).HasColumnName("product_id");
        builder.Property(item => item.Text).HasColumnName("text").HasMaxLength(200);
        builder.Property(item => item.SortOrder).HasColumnName("sort_order");
    }
}

public sealed class ProductSpecConfiguration : IEntityTypeConfiguration<ProductSpec>
{
    public void Configure(EntityTypeBuilder<ProductSpec> builder)
    {
        builder.ToTable("product_specs");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).HasColumnName("id");
        builder.Property(item => item.ProductId).HasColumnName("product_id");
        builder.Property(item => item.Label).HasColumnName("label").HasMaxLength(120);
        builder.Property(item => item.Value).HasColumnName("value").HasMaxLength(300);
        builder.Property(item => item.Group).HasColumnName("spec_group").HasMaxLength(30);
        builder.Property(item => item.SortOrder).HasColumnName("sort_order");
    }
}

public sealed class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("product_variants");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).HasColumnName("id");
        builder.Property(item => item.ProductId).HasColumnName("product_id");
        builder.Property(item => item.Name).HasColumnName("name").HasMaxLength(120);
        builder.Property(item => item.Type).HasColumnName("type").HasMaxLength(30);
        builder.Property(item => item.Value).HasColumnName("value").HasMaxLength(120);
        builder.Property(item => item.Swatch).HasColumnName("swatch").HasMaxLength(20);
        builder.Property(item => item.ImageSrc).HasColumnName("image_src").HasMaxLength(500);
        builder.Property(item => item.ImageAlt).HasColumnName("image_alt").HasMaxLength(300);
        builder.Property(item => item.ImageWidth).HasColumnName("image_width");
        builder.Property(item => item.ImageHeight).HasColumnName("image_height");
        builder.Property(item => item.SortOrder).HasColumnName("sort_order");
        builder.Ignore(item => item.Image);
    }
}

public sealed class ProductBadgeConfiguration : IEntityTypeConfiguration<ProductBadge>
{
    public void Configure(EntityTypeBuilder<ProductBadge> builder)
    {
        builder.ToTable("product_badges");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).HasColumnName("id");
        builder.Property(item => item.ProductId).HasColumnName("product_id");
        builder.Property(item => item.Badge).HasColumnName("badge").HasMaxLength(30);
    }
}

public sealed class ProductRelatedConfiguration : IEntityTypeConfiguration<ProductRelated>
{
    public void Configure(EntityTypeBuilder<ProductRelated> builder)
    {
        builder.ToTable("product_related");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).HasColumnName("id");
        builder.Property(item => item.ProductId).HasColumnName("product_id");
        builder.Property(item => item.RelatedProductId).HasColumnName("related_product_id");
        builder.Property(item => item.SortOrder).HasColumnName("sort_order");
    }
}
