using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Logical union of one or more customers/branches.
    /// </summary>
    [Table("Client")]
    public class Client : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a customer.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The <see cref="ClientGroup"/> to which this <see cref="Client"/> belongs.
        /// </summary>
        [Column("GroupId", Order = 1)]
        public long? GroupId { get; set; }
        
        [ForeignKey("GroupId")]
        public ClientGroup Group { get; set; }

        /// <summary>
        /// DB row creation metadata.
        /// </summary>
        [Column("CreationId", Order = 2)]
        public long? CreationId { get; set; }
        
        [ForeignKey("CreationId")]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        [Column("LastModificationId", Order = 3)]
        public long? LastModificationId { get; set; }
        
        [ForeignKey("LastModificationId")]
        public RecordModification LastModification { get; set; }

        /// <summary>
        /// The vacancies assigned to this <see cref="Client"/>.
        /// </summary>
        [Column("Vacancies", Order = 4)]
        public ICollection<Vacancy> Vacancies { get; set; }

        [ForeignKey("PostalAddressId")]
        public PostalAddress PostalAddress { get; set; }
        
        /// <summary>
        /// The branch's postal address.
        /// </summary>
        [Column("PostalAddressId", Order = 5)]
        public long? PostalAddressId { get; set; }

        /// <summary>
        /// Customer/branch email address. 
        /// </summary>
        [Column("Email", Order = 6)]
        public string Email { get; set; }

        /// <summary>
        /// The branch's phone number.
        /// </summary>
        [Column("Phone", Order = 7)]
        public string Phone { get; set; }

        /// <summary>
        /// FAX
        /// </summary>
        [Column("Fax", Order = 8)]
        public string Fax { get; set; }

        /// <summary>
        /// The <see cref="Order"/>s that this <see cref="Client"/> has placed so far.
        /// </summary>
        [Column("Orders", Order = 9)]
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public ICollection<ClientUserSetting> ClientUserSettings { get; set; } = new List<ClientUserSetting>();
    }
}