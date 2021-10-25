using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using MiniORM;

namespace MiniORM
{
    internal class ChangeTracker<T> where T : class, new()
    {
        private readonly List<T> allEntities;
        private readonly List<T> added;
        private readonly List<T> removed;

        public ChangeTracker(IEnumerable<T> entities)
        {
            added = new List<T>();
            removed = new List<T>();
            allEntities = CloneEntities(entities);
        }

        public IReadOnlyCollection<T> AllEntities => allEntities.AsReadOnly();
        public IReadOnlyCollection<T> Added => added.AsReadOnly();
        public IReadOnlyCollection<T> Removed => removed.AsReadOnly();

        public void Add(T element) 
            => added.Add(element);


        public void Remove(T element)
            => removed.Add(element);

        private static List<T> CloneEntities(IEnumerable<T> entities)
        {
            List<T> clonedEntities = new List<T>();

            PropertyInfo[] propertiesToClone = typeof(T)
                .GetProperties()
                .Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
                .ToArray();

            foreach (var entity in entities)
            {
                T clonedEntity = Activator.CreateInstance<T>();

                foreach (var property in propertiesToClone)
                {
                    var value = property.GetValue(entities);
                    property.SetValue(clonedEntity, value);
                }

                clonedEntities.Add(clonedEntity);
            }

            return clonedEntities;
        }

        public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
        {
            List<T> modiffiedEntities = new List<T>();

            IEnumerable<PropertyInfo> primaryKeys = typeof(T)
                .GetProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>())
                .ToArray();

            foreach (var proxyEntity in AllEntities)
            {
                var primaryKeyValues = GetPrimaryKeyValues(primaryKeys, proxyEntity);

                T entity = dbSet
                    .Entities
                    .Single(e => GetPrimaryKeyValues(primaryKeys, e).SequenceEqual(primaryKeyValues));

                var isModified = IsModified(proxyEntity, entity);

                if (isModified)
                {
                    modiffiedEntities.Add(entity);
                }
            }

            return modiffiedEntities;
        }

        private static bool IsModified(T entity, T proxyEntoty)
        {
            var monitoredProperties = typeof(T)
                .GetProperties()
                .Where(p => DbContext.AllowedSqlTypes.Contains(p.PropertyType));

            var modifiedProperties = monitoredProperties
                .Where(pi => !Equals(pi.GetValue(entity), pi.GetValue(proxyEntoty)))
                .ToArray();

            bool isModified = modifiedProperties.Any();

            return isModified;
        }

        private static IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKeys, T entity)
        {
            IEnumerable<object> primaryKeyValues = primaryKeys
                .Select(pi => pi.GetValue(entity));

            return primaryKeyValues;
        }
    }
}