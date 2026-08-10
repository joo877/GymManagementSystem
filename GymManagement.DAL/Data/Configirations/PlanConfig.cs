using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Data.Configirations
{
    public class PlanConfig : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(x => x.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(X => X.Description)
              .HasMaxLength(200);

            builder.Property(X => X.Price)
                .HasPrecision(10, 2);

            builder.Property(X => X.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");


            builder.ToTable(tp =>

                 tp.HasCheckConstraint("PlanDurationCheck", "DurationDays between 1 and 365")


            );
            builder.HasData(
                new Plan() 
                {Id=1,
                  Name="Basic Plan",
                  DurationDays=30,
                 Description="Access to gym equpment during staffed hours" ,
                  Price=300,
                  IsActive=true
                
                },

                 new Plan()
                 {
                     Id = 2,
                     Name = "Standard Plan",
                     DurationDays = 60,
                     Description = "includes gym equpment and 2 group classes per week",
                     Price = 500,
                     IsActive=false

                 },

                         new Plan()
                         {
                             Id = 3,
                             Name = "premium Plan",
                             DurationDays = 90,
                             Description = "unlimited access equpment,classes and sauna",
                             Price = 900,
                             IsActive = false

                         },

                          new Plan()
                          {
                              Id = 4,
                              Name = "Annual Plan",
                              DurationDays = 365,
                              Description = "full year access with personal trainer sessions",
                              Price = 3000,
                              IsActive = true

                          }



                );
                    
        }
    }
}
