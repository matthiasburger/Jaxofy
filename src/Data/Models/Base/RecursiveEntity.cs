using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasTeamRevolution.Data.Models.Base
{
    public abstract class RecursiveEntity<TEntity, TKey> : IEntity<TKey>, IRecursiveEntity<TEntity, TKey>
        where TEntity : RecursiveEntity<TEntity, TKey>
    {
        [Key] public TKey Id { get; set; }

        public virtual TEntity Parent { get; set; }
        public virtual ICollection<TEntity> Children { get; set; }

        public Dictionary<long, TEntity> GetParentObjectDictionary()
        {
            TEntity parent = Parent;
            Dictionary<long, TEntity> entityDictionary = new();

            long counter = 0L;
            while (parent != null)
            {
                entityDictionary.Add(counter, parent);
                parent = parent.Parent;
                ++counter;
            }

            return entityDictionary;
        }
    }
}