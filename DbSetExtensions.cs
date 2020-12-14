using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace EFRelatedData
{
    public static class DbContextExtensions
    {
        public static T AddOrUpdate<T>(this DbSet<T> dbSet, Func<T, int> identifier, T entity) where T : class
        {
            var result = dbSet.Find(identifier.Invoke(entity));

            if (result != null)
            {
                var context = dbSet.GetService<ICurrentDbContext>().Context;
                context.Entry(result).CurrentValues.SetValues(entity);
                dbSet.Update(result);
                return result;
            }
            else
            {
                dbSet.Add(entity);
                return entity;
            }
        }
    }
}
