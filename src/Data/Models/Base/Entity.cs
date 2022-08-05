using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jaxofy.Data.Models.Base
{
    /// <summary>
    /// Base entity class. Uniquely identifies every domain model in the DB
    /// via the primary key, which is always a <c>bigint</c> called "Id".
    /// </summary>
    public class Entity : IEntity
    {
        /// <summary>
        /// Entity ID and DB Primary Key.
        /// </summary>
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public long Id { get; set; }
    }
}