using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymManagementDbContext _dbContext;
        private readonly Dictionary<string, object> _repositories = [];
        public UnitOfWork(GymManagementDbContext dbContext
            , ISessionRepository sessionRepository
            ,IMembeshipRepository membeshipRepository
            ,IBookingRepository bookingRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
            MembeshipRepository = membeshipRepository;
            BookingRepository = bookingRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IMembeshipRepository MembeshipRepository { get; }

        public IBookingRepository BookingRepository { get; }

        public IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            // check  typeof TEntity  (repository)
            // retun repository
            // create repositry - story - return
            var typename = typeof(TEntity).Name;
            if (_repositories.TryGetValue(typename, out object? value))
                return (IGenaricRepository<TEntity>)value;
            else 
            {
                var repo = new GenaricRepository<TEntity>(_dbContext);
            
                _repositories[typename] = repo;
            return repo;

            }


        }

        public async Task<int> SaveChangAsync(CancellationToken ct) => await _dbContext.SaveChangesAsync(ct);
         
    }
}
