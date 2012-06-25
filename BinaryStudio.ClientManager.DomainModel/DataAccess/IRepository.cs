using System;
using System.Linq;
using System.Linq.Expressions;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.DataAccess
{
    public interface IRepository
    {
        IQueryable<T> Query<T>(params Expression<Func<T, object>>[] eagerlyLoadedProperties) where T : class, IIdentifiable;
        T Get<T>(int id, params Expression<Func<T, object>>[] eagerlyLoadedProperties) where T : class, IIdentifiable;

        void Save<T>(T instance) where T : class, IIdentifiable;
        void Delete<T>(T instance) where T : class, IIdentifiable;
    }
}