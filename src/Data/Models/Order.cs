using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// An order.
    /// </summary>
    [Table("Order")]
    public class Order : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies an order.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        /// <summary>
        /// The ID of the <see cref="Client"/> who placed this <see cref="Order"/>.
        /// </summary>
        [Column("ClientId", Order = 1)]
        public long ClientId { get; set; }
        
        /// <summary>
        /// The <see cref="Client"/> who placed this <see cref="Order"/>.
        /// </summary>
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        
        /// <summary>
        /// Order creation metadata.
        /// </summary>
        [Column("Creation", Order = 2)]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// Last order's modification metadata.
        /// </summary>
        [Column("LastModification", Order = 3)]
        public RecordModification LastModification { get; set; }
        
        /// <summary>
        /// The work assignments that resulted out of this <see cref="Order"/>.
        /// </summary>
        [Column("Assignments", Order = 4)]
        public ICollection<Assignment> Assignments { get; set; }
        
        /// <summary>
        /// todo: discuss a functional order-id like ClientId + Ascending Number + Year ?
        /// so we got sth like 07-000245-2021
        /// when we debug, we see what order in which year for what client it was
        /// </summary>
        [Column("OrderNumber", Order = 5)]
        public string OrderNumber { get; set; }
        
        /// <summary>
        /// Sets the date, when the order was sent
        /// </summary>
        [Column("OrderedOn", Order = 6)]
        public DateTime? OrderedOn { get; set; }

        /// <summary>
        /// Sets the vacancies start-date
        /// </summary>
        [Column("StartDate", Order = 7)]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Sets the vacancies end-date
        /// </summary>
        [Column("EndDate", Order = 8)]
        public DateTime EndDate { get; set; }
    }
}