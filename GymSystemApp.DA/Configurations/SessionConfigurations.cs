using GymSystemApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystemG03.DAL.Configurations
{
    internal class SessionConfigurations : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(T =>
            {
                T.HasCheckConstraint("SessionCapacityConstraint", "Capacity between 1 and 25");
                T.HasCheckConstraint("SessionEndDateAfterStartDate", "EndDate > StartDate");
            });

            builder.HasOne(X => X.Trainer)
                .WithMany(X => X.Sessions)
                .HasForeignKey(X => X.TrainerId);

            builder.HasOne(X => X.Category)
                .WithMany(X => X.Sessions)
                .HasForeignKey(X => X.CategoryId);


        }
    }
}
