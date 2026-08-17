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
    public class MembeshipRepository : GenaricRepository<MemberShip>, IMembeshipRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public MembeshipRepository(GymManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MemberShip>> GetMembershipWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? expression = null, CancellationToken ct = default)
        {
            var query = _dbContext.MemberShips.Include(m => m.Plan).Include(m => m.Member).AsNoTracking();

            if (expression is not null)
                query.Where(expression);

            return await query.ToListAsync(ct);
        }
    }
}
