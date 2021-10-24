using CardMon.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CardMon.Infrastructure.Data.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
            => _context = context;

        public void Create(T data) =>
            _context.Set<T>().Add(data);

        public void Delete(T data) =>
            _context.Set<T>().Remove(data);

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ? _context.Set<T>()
            .AsNoTracking()
            : _context.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate, bool trackChanges) =>
            !trackChanges ? _context.Set<T>()
            .Where(predicate)
            .AsNoTracking()
            : _context.Set<T>()
            .Where(predicate);

        public virtual bool HasAny(Expression<Func<T, bool>> predicate) =>
            _context.Set<T>().Any(predicate);

        public void Update(T data) =>
            _context.Set<T>().Update(data);

        public void UpdateRange(IList<T> data) =>
            _context.Set<T>().UpdateRange(data);
    }
}
