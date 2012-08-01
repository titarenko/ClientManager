using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.DataAccess
{
    /// <summary>
    /// Supports multi-tenancy. Can work on top of any implementation of repository.
    /// </summary>
    public class MultitenantRepository : IRepository
    {
        private readonly IRepository repository;

        private readonly UserService userService;

        private readonly MethodInfo queryFilteredInternal = typeof(MultitenantRepository)
            .GetMethod("QueryFilteredInternal", BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly IDictionary<Type, MethodInfo> queryFiltered = new Dictionary<Type, MethodInfo>();

        private readonly object[] queryFilteredArguments = new object[1];

        /// <summary>
        /// Initializes a new instance of the <see cref="MultitenantRepository"/> class.
        /// </summary>
        /// <param name="repository">Base repository.</param>
        /// <param name="userService">User service.</param>
        public MultitenantRepository(IRepository repository, UserService userService)
        {
            this.repository = repository;
            this.userService = userService;
        }

        /// <summary>
        /// Queries the specified eagerly loaded properties.
        /// </summary>
        /// <param name="eagerlyLoadedProperties">The eagerly loaded properties.</param>
        public IQueryable<T> Query<T>(params Expression<Func<T, object>>[] eagerlyLoadedProperties) where T : class, IIdentifiable
        {
            return IsMultitenant<T>()
                       ? QueryFiltered(eagerlyLoadedProperties)
                       : repository.Query(eagerlyLoadedProperties);
        }

        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="eagerlyLoadedProperties">The eagerly loaded properties.</param>
        public T Get<T>(int id, params Expression<Func<T, object>>[] eagerlyLoadedProperties) where T : class, IIdentifiable
        {
            return IsMultitenant<T>()
                       ? QueryFiltered(eagerlyLoadedProperties).First(x => x.Id == id)
                       : repository.Get(id, eagerlyLoadedProperties);
        }

        /// <summary>
        /// Saves the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void Save<T>(T instance) where T : class, IIdentifiable
        {
            if (IsMultitenant<T>())
            {
                var multitenant = (IOwned)instance;
                multitenant.OwnerId = userService.CurrentUser.Id;
            }

            repository.Save(instance);
        }

        /// <summary>
        /// Deletes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void Delete<T>(T instance) where T : class, IIdentifiable
        {
            if (IsMultitenant<T>() && ((IOwned)instance).OwnerId != userService.CurrentUser.Id)
            {
                throw new ApplicationException("An attempt to delete foreign multitenant data was made.");
            }

            repository.Delete(instance);
        }

        /// <summary>
        /// Determines whether given type supports multi tenancy.
        /// </summary>
        private bool IsMultitenant<T>() where T : class
        {
            return typeof(IOwned).IsAssignableFrom(typeof(T));
        }

        private IQueryable<T> QueryFiltered<T>(params Expression<Func<T, object>>[] eagerlyLoadedProperties)
        {
            var type = typeof(T);
            if (!queryFiltered.ContainsKey(type))
            {
                queryFiltered.Add(type, queryFilteredInternal.MakeGenericMethod(type));
            }
            queryFilteredArguments[0] = eagerlyLoadedProperties;
            return queryFiltered[type].Invoke(this, queryFilteredArguments) as IQueryable<T>;
        }

        /// <summary>
        /// Queries data for current tenant.
        /// </summary>
        private IQueryable<T> QueryFilteredInternal<T>(params Expression<Func<T, object>>[] eagerlyLoadedProperties) where T : class, IIdentifiable, IOwned
        {
            return repository
                .Query(eagerlyLoadedProperties)
                .Where(x => x.OwnerId == userService.CurrentUser.Id);
        }
    }
}
