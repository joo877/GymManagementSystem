using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class SessionRepository : GenaricRepository<Session>, ISessionRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public SessionRepository(GymManagementDbContext dbContext ) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Session>> GetAllSessionWithTrainerAndCategory(CancellationToken ct = default)
        {
            return await _dbContext.Sessions.AsNoTracking().Include(x => x.Trainer).Include(x => x.Category).ToListAsync(ct);
        }

        public async Task<int> GetCountOfSlotsAsync(int sessionId, CancellationToken ct = default)
        {
            return await _dbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId, ct);
        }

        public async Task<Session?> GetSessionWithTrainerAndCategory(int sessionId, CancellationToken ct)
        {
            return await _dbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category).FirstOrDefaultAsync(x => x.Id == sessionId);
        }
    }
}
