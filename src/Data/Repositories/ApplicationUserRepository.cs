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
    public interface IApplicationUserRepository : IRepository<ApplicationUser, long>
    {
    }

    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> Get(long id)
            => await _context.ApplicationUsers.FindAsync(id);

        /// <inheritdoc/>
        public async Task<IEnumerable<ApplicationUser>> GetAll()
            => await _context.ApplicationUsers.ToListAsync();

        /// <inheritdoc/>
        public IQueryable<ApplicationUser> GetQueryable()
            => _context.ApplicationUsers.AsQueryable();

        /// <inheritdoc/>
        public async Task<ApplicationUser> SingleOrDefault(Expression<Func<ApplicationUser, bool>> predicate)
            => await _context.ApplicationUsers.SingleOrDefaultAsync(predicate);

        public async Task<ApplicationUser> SingleOrDefaultNoTracking(Expression<Func<ApplicationUser, bool>> predicate)
            => await _context.ApplicationUsers.AsNoTracking().SingleOrDefaultAsync(predicate);

        public async Task<bool> AnyNoTracking(Expression<Func<ApplicationUser, bool>> predicate)
            => await _context.ApplicationUsers.AsNoTracking().AnyAsync(predicate);

        public async Task<long> CountLongNoTracking(Expression<Func<ApplicationUser, bool>> predicate)
            => await _context.ApplicationUsers.AsNoTracking().LongCountAsync(predicate);

        public async Task<int> CountNoTracking(Expression<Func<ApplicationUser, bool>> predicate)
            => await _context.ApplicationUsers.AsNoTracking().CountAsync(predicate);

        /// <inheritdoc/>
        public async Task<IEnumerable<ApplicationUser>> Find(Expression<Func<ApplicationUser, bool>> predicate)
            => await _context.ApplicationUsers.Where(predicate).ToListAsync();

        /// <inheritdoc/>
        public async Task<(bool success, EntityEntry<ApplicationUser> entity)> Add(ApplicationUser entity)
        {
            EntityEntry<ApplicationUser> createdUser = await _context.ApplicationUsers.AddAsync(entity);
            return (await _context.SaveChangesAsync() == 1 && createdUser.IsKeySet, createdUser);
        }

        /// <inheritdoc/>
        public async Task<bool> AddRange(IEnumerable<ApplicationUser> entities)
        {
            IEnumerable<ApplicationUser> applicationUsers = entities as ApplicationUser[] ?? entities.ToArray();

            await _context.ApplicationUsers.AddRangeAsync(applicationUsers);
            return await _context.SaveChangesAsync() == applicationUsers.Count();
        }

        /// <inheritdoc/>
        public async Task<bool> Update(ApplicationUser entity)
        {
            _context.ApplicationUsers.Update(entity);
            return await _context.SaveChangesAsync() == 1;
        }

        /// <inheritdoc/>
        public async Task<bool> Remove(ApplicationUser entity)
        {
            _context.ApplicationUsers.Remove(entity);
            return await _context.SaveChangesAsync() == 1;
        }

        /// <inheritdoc/>
        public async Task<bool> Remove(long id)
        {
            ApplicationUser entity = await _context.ApplicationUsers.FindAsync(id);

            _context.ApplicationUsers.Remove(entity);
            return await _context.SaveChangesAsync() == 1;
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveAll()
        {
            IList<ApplicationUser> entities = await _context.ApplicationUsers.ToListAsync();

            _context.ApplicationUsers.RemoveRange(entities);
            return await _context.SaveChangesAsync() == entities.Count;
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveRange(Expression<Func<ApplicationUser, bool>> predicate)
        {
            IList<ApplicationUser> entities = await _context.ApplicationUsers.Where(predicate).ToListAsync();

            _context.ApplicationUsers.RemoveRange(entities);
            return await _context.SaveChangesAsync() == entities.Count;
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveRange(IEnumerable<ApplicationUser> entities)
        {
            IEnumerable<ApplicationUser> applicationUsers = entities as ApplicationUser[] ?? entities.ToArray();

            _context.ApplicationUsers.RemoveRange(applicationUsers);
            return await _context.SaveChangesAsync() == applicationUsers.Count();
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveRange(IEnumerable<long> ids)
        {
            IEnumerable<ApplicationUser> applicationUsers =
                await _context.ApplicationUsers.Where(w => ids.Contains(w.Id)).ToListAsync();

            _context.ApplicationUsers.RemoveRange(applicationUsers);
            return await _context.SaveChangesAsync() == applicationUsers.Count();
        }
    }
}