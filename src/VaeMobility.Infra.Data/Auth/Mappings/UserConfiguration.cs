using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaeMobility.Domain.Auth.Entities;
using VaeMobility.Infra.Data.Extensions;

namespace VaeMobility.Infra.Data.Auth.Mappings;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id).HasColumnName("id");
        builder.Property(user => user.Email).HasColumnName("email").HasMaxLength(256).IsRequired();
        builder.Property(user => user.PasswordHash).HasColumnName("password_hash").HasMaxLength(500).IsRequired();
        builder.Property(user => user.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.ConfigureAuditColumns();
    }
}
