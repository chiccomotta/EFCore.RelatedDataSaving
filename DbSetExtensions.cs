using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFRelatedData
{
    public static class DbContextExtensions
    {
        //public static void AddOrUpdate<T>(this DbSet<T> dbSet, T newEntity, Func<T, bool> predicate) where T : class
        //{
        //    var entity = dbSet.SingleOrDefault(predicate);
        //    if (entity != null)
        //    {
        //        dbSet.Update(newEntity);
        //    }
        //    else
        //    {
        //        dbSet.Add(newEntity);
        //    }
        //}

        public static T AddOrUpdate<T>(this DbSet<T> dbSet, DbContext context, Func<T, int> identifier, T entity) where T : class
        {
            var result = dbSet.Find(identifier.Invoke(entity));
            if (result != null)
            {
                //var context2 = dbSet.GetService<ICurrentDbContext>().Context;
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
