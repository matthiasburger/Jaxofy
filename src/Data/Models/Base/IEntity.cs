namespace Jaxofy.Data.Models.Base
{
    /// <summary>
    /// Generic entity that is uniquely identifiable in the db
    /// via the <typeparamref name="T"/> type-parameter as a
    /// primary key type (whose column is also called "Id").
    /// </summary>
    /// <typeparam name="T">Primary key type.</typeparam>
    public interface IEntity<T>
    {
        /// <summary>
        /// The entity's unique row identifier (Primary Key in the DB).
        /// </summary>
        T Id { get; set; }
    }

    /// <summary>
    /// <see cref="IEntity{T}"/> variant for uniquely identifying rows in the DB via their <c>bigint</c> primary key.
    /// </summary>
    public interface IEntity : IEntity<long>
    {
    }
}