using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Configirations
{
    public class SessionConfig : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.Property(x => x.Capacity)
                   .HasMaxLength(25);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("SessionCapacityCheck ", "Capacity between 1 and 25  ");
                tb.HasCheckConstraint("SessionEndDateCheck ", "EndDate > StartDate ");

            });

           
        }
    }
}
