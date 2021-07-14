using System.Collections.Generic;

namespace DasTeamRevolution.Data.Models.Base
{
    /// <summary>
    ///  It's an entity that is suitable for representing tree-type data structures.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRecursiveEntity<TEntity> where TEntity : IEntity
    {
        TEntity Parent { get; set; }
        ICollection<TEntity> Children { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRecursiveEntity<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        TEntity Parent { get; set; }
        ICollection<TEntity> Children { get; set; }
    }
}