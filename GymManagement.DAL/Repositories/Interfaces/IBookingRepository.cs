using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IBookingRepository : IGenaricRepository<Booking>
    {
        public Task<IEnumerable<Booking>> GetBooKingBySessionIdAsyc(int sessionId ,CancellationToken ct=default);


    }
}
