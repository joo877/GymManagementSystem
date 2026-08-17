using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class BookingRepository : GenaricRepository<Booking>, IBookingRepository

    {
        private readonly GymManagementDbContext _dbContext;

        public BookingRepository(GymManagementDbContext dbContext ) : base(dbContext)
        {
           _dbContext = dbContext;
        }

        public async Task<IEnumerable<Booking>> GetBooKingBySessionIdAsyc(int sessionId, CancellationToken ct = default)
        {
            return await _dbContext.Bookings
                             .AsNoTracking()
                             .Include(b => b.Member)
                             .Where(b => b.SessionId == sessionId)
                             .ToListAsync(ct);
        }


    }
}
