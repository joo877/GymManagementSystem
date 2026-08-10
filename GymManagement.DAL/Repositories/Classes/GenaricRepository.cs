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
    public class GenaricRepository<TEntity> : IGenaricRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymManagementDbContext _dbContext;

        public GenaricRepository(GymManagementDbContext dbContext ) 
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool IsTraking = false, CancellationToken ct = default)
        {
            var entity =  IsTraking ? await _dbContext.Set<TEntity>().ToListAsync(ct) : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync(ct);
            return entity;
        }



        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id ,ct);
        }




        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
          
        }


        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
       
        }


        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
           
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        {
            return await _dbContext.Set<TEntity>().AnyAsync(predicate, ct);
        }

        public  async Task<TEntity?> FristOrDefualtAsync(Expression<Func<TEntity, bool>> predicate, bool traking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = traking ?  _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? Condition = null, CancellationToken ct = default)
        {
            return Condition == null ? await _dbContext.Set<TEntity>().AsNoTracking().CountAsync(ct) : await _dbContext.Set<TEntity>().AsNoTracking().CountAsync(Condition,ct);
                
        }
    }
}
