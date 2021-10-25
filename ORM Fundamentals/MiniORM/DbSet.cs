using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MiniORM
{
	class DbSet<TEntity> : ICollection<TEntity> where TEntity: class, new()
    {
        internal DbSet(IEnumerable<TEntity> entities)
        {
            Entities = entities.ToList();
            ChangeTracker = new ChangeTracker<TEntity>(entities);
        }

        internal IList<TEntity> Entities { get; }

        internal ChangeTracker<TEntity> ChangeTracker { get; }

        public int Count 
            => Entities.Count();

        public bool IsReadOnly
            => this.Entities.IsReadOnly;

        public void Add(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null");
            }

            this.Entities.Add(item);
            this.ChangeTracker.Add(item);
        }

        public void Clear()
        {
            while (Entities.Any())
            {
                var entity = Entities.First();
                this.Remove(entity);
            }
        }

        public bool Contains(TEntity item)
        => this.Entities.Contains(item);

        public void CopyTo(TEntity[] array, int arrayIndex)
        => this.Entities.CopyTo(array, arrayIndex);

        public bool Remove(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null!");
            }

            bool removedSuccessfully = this.Entities.Remove(item);

            if (removedSuccessfully)
            {
                this.ChangeTracker.Remove(item);
            }

            return removedSuccessfully;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Remove(entity);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return this.Entities.GetEnumerator();
        }

    }
}