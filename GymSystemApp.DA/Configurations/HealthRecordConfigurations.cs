using GymSystemApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystemG03.DAL.Configurations
{
    internal class HealthRecordConfigurations : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.Property(x => x.BloodType)
                .HasMaxLength(5);

            builder.Property(x => x.Note)
                   .HasMaxLength(500);
        }
    }
}
