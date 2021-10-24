using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CardMon.Core.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate, bool trackChanges);
        bool HasAny(Expression<Func<T, bool>> predicate);
        void Create(T data);
        void Update(T data);
        void UpdateRange(IList<T> data);
        void Delete(T data);
    }
}
