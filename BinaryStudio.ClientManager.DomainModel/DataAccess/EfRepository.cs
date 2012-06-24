using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.TeamFoundation.TestManagement.Client;

namespace BinaryStudio.ClientManager.DomainModel.DataAccess
{
    public class EfRepository : IRepository
    {
        private readonly EfDataContext context = new EfDataContext();

        private string GetPath<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var e = (MemberExpression)expression.Body;
            return e.Member.Name;
        }

        public IQueryable<T> Query<T>(params Expression<Func<T, object>>[] eagerlyLoadedProperties) where T : class, IIdentifiable<int>
        {
            return eagerlyLoadedProperties.Aggregate(
                (DbQuery<T>)context.GetDbSet<T>(),
                (current, property) => current.Include(GetPath(property)));
        }

        public T Get<T>(int id, params Expression<Func<T, object>>[] eagerlyLoadedProperties) where T : class, IIdentifiable<int>
        {
            var set = context.GetDbSet<T>();

            if (!eagerlyLoadedProperties.Any())
            {
                return set.Find(id);
            }

            return eagerlyLoadedProperties.Aggregate(
                (DbQuery<T>)set,
                (current, property) => current.Include(GetPath(property))).First(x => x.Id == id);
        }

        public void Save<T>(T instance) where T : class, IIdentifiable<int>
        {
            if (instance.Id == default(int))
            {
                context.GetDbSet<T>().Add(instance);
            }
            else
            {
                context.Entry(instance).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void Delete<T>(T instance) where T : class, IIdentifiable<int>
        {
            context.GetDbSet<T>().Remove(instance);
            context.SaveChanges();
        }
    }
}