using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenaricRepository<Session>
    {

        Task<IEnumerable<Session>> GetAllSessionWithTrainerAndCategory(Expression<Func<Session,bool>>? expression = null  , CancellationToken ct = default);
        Task<int> GetCountOfSlotsAsync(int sessionId ,CancellationToken ct=default );

        Task<Session?> GetSessionWithTrainerAndCategory(int sessionId, CancellationToken ct=default);
    }
}
