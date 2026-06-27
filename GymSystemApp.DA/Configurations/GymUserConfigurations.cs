using GymSystemApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystemG03.DAL.Configurations
{
    internal class GymUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(X => X.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.Street)
                       .HasColumnName("Street")
                       .HasColumnType("varchar")
                       .HasMaxLength(30);

                address.Property(a => a.City)
                       .HasColumnType("varchar")
                       .HasColumnName("City")
                       .HasMaxLength(30);

                address.Property(a => a.BuildingNumber)
                       .HasColumnName("BuildingNumber");
            });


            builder.Property(X => X.Phone)
                   .HasColumnType("varchar")
                   .HasMaxLength(11);

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("GymUser_EmailCheck", "Email LIKE '_%@_%._%'");
                t.HasCheckConstraint("GymUser_PhoneCheck", "[Phone] LIKE '010%' OR [Phone] LIKE '011%' OR [Phone] LIKE '012%' OR [Phone] LIKE '015%'");
            });
        }
    }
}
