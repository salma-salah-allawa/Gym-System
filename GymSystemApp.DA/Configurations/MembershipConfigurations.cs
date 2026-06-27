using GymSystemApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystemG03.DAL.Configurations
{
    internal class MembershipConfigurations : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(X => X.CreatedAt)
                   .HasColumnName("StartDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(m => m.Plan)
                          .WithMany(p => p.Memberships)
                          .HasForeignKey(m => m.PlanId)
                          .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Member)
                   .WithMany(me => me.Memberships)
                   .HasForeignKey(m => m.MemberId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
