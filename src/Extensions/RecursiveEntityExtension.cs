using System.Linq;
using System.Collections.Generic;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Extensions
{
    public static class RecursiveEntityExtension
    {
        public static TEntity AddChild<TEntity>(this TEntity parent, TEntity child) 
            where TEntity : RecursiveEntity<TEntity, long>
        {
            child.Parent = parent;
            parent.Children ??= new HashSet<TEntity>();
            parent.Children.Add(child);
            return parent;
        }
        
        public static TEntity AddChildren<TEntity>(this TEntity parent, IEnumerable<TEntity> children) 
            where TEntity : RecursiveEntity<TEntity, long>
        {
            children.ToList().ForEach(c => parent.AddChild(c));
            return parent;
        }
    }
}