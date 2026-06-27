using GymSystemApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystemG03.DAL.Configurations
{
    internal class TrainerConfigurations : GymUserConfigurations<Trainer>, IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(X => X.CreatedAt)
                   .HasColumnName("HireDate")
                   .HasDefaultValueSql("GETDATE()");

            base.Configure(builder);
        }
    }
}
