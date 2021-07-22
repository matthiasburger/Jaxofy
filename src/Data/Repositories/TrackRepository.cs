using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Jaxofy.Data.Models;
using Jaxofy.Data.Repositories.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaxofy.Data.Repositories
{
    public interface ITrackRepository : IRepository<Track, long>
    {
    }

    public class TrackRepository : ITrackRepository
    {
        private readonly ApplicationDbContext _dbContext;
        
        public TrackRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Track> Get(long id)
        {
            return await _dbContext.Tracks.FindAsync(id);
        }

        public async Task<IEnumerable<Track>> GetAll()
        {
            return await _dbContext.Tracks.ToListAsync();
        }

        public IQueryable<Track> GetQueryable()
        {
            return _dbContext.Tracks.AsQueryable();
        }

        public async Task<Track> SingleOrDefault(Expression<Func<Track, bool>> predicate)
        {
            return await _dbContext.Tracks.SingleOrDefaultAsync(predicate);
        }

        public async Task<Track> SingleOrDefaultNoTracking(Expression<Func<Track, bool>> predicate)
        {
            return await _dbContext.Tracks.AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        public async Task<bool> AnyNoTracking(Expression<Func<Track, bool>> predicate)
        {
            return await _dbContext.Tracks.AsNoTracking().AnyAsync(predicate);
        }

        public async Task<int> CountNoTracking(Expression<Func<Track, bool>> predicate)
        {
            return await _dbContext.Tracks.AsNoTracking().CountAsync(predicate);
        }

        public async Task<long> CountLongNoTracking(Expression<Func<Track, bool>> predicate)
        {
            return await _dbContext.Tracks.AsNoTracking().LongCountAsync(predicate);
        }

        public async Task<IEnumerable<Track>> Find(Expression<Func<Track, bool>> predicate)
        {
            return await _dbContext.Tracks.Where(predicate).ToListAsync();
        }

        public async Task<(bool success, EntityEntry<Track> entity)> Add(Track entity)
        {
            EntityEntry<Track> createdTrack = await _dbContext.Tracks.AddAsync(entity);
            return (await _dbContext.SaveChangesAsync() == 1 && createdTrack.IsKeySet, createdTrack);
        }

        public async Task<bool> AddRange(IEnumerable<Track> entities)
        {
            IEnumerable<Track> tracks = entities as Track[] ?? entities.ToArray();

            await _dbContext.Tracks.AddRangeAsync(tracks);
            return await _dbContext.SaveChangesAsync() == tracks.Count();
        }

        public async Task<bool> Update(Track entity)
        {
            _dbContext.Tracks.Update(entity);
            return await _dbContext.SaveChangesAsync() == 1;
        }

        public async Task<bool> Remove(Track entity)
        {
            _dbContext.Tracks.Remove(entity);
            return await _dbContext.SaveChangesAsync() == 1;
        }

        public async Task<bool> Remove(long id)
        {
            Track entity = await _dbContext.Tracks.FindAsync(id);

            _dbContext.Tracks.Remove(entity);
            return await _dbContext.SaveChangesAsync() == 1;
        }

        public async Task<bool> RemoveAll()
        {
            IList<Track> entities = await _dbContext.Tracks.ToListAsync();

            _dbContext.Tracks.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync() == entities.Count;
        }

        public async Task<bool> RemoveRange(Expression<Func<Track, bool>> predicate)
        {
            IList<Track> entities = await _dbContext.Tracks.Where(predicate).ToListAsync();

            _dbContext.Tracks.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync() == entities.Count;
            
        }

        public async Task<bool> RemoveRange(IEnumerable<Track> entities)
        {
            IEnumerable<Track> tracks = entities as Track[] ?? entities.ToArray();

            _dbContext.Tracks.RemoveRange(tracks);
            return await _dbContext.SaveChangesAsync() == tracks.Count();
            
        }

        public async Task<bool> RemoveRange(IEnumerable<long> ids)
        {
            IEnumerable<Track> tracks =
                await _dbContext.Tracks.Where(w => ids.Contains(w.Id)).ToListAsync();

            _dbContext.Tracks.RemoveRange(tracks);
            return await _dbContext.SaveChangesAsync() == tracks.Count();
            
        }
    }
}