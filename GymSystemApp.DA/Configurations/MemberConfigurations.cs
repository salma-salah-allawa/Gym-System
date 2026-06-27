using GymSystemApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystemG03.DAL.Configurations
{
    internal class MemberConfigurations : GymUserConfigurations<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(X => X.CreatedAt)
                   .HasColumnName("JoinDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(M => M.HealthRecord)
                   .WithOne(HR => HR.Member)
                   .HasForeignKey<HealthRecord>(M => M.MemberId);

            base.Configure(builder);

        }
    }
}
