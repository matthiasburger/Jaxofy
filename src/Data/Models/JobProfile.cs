using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    public class JobProfile : IEntity<long>
    {
        /// <summary>
        /// The Id of the job portal
        /// </summary>
        [Key, Column("Id", Order = 0)] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        /// <summary>
        /// The client-supplier-id this job portal belongs to 
        /// </summary>
        [Column("ClientSupplierId", Order = 1)] 
        public long ClientSupplierId { get; set; }

        /// <summary>
        /// The client-supplier this job portal belongs to 
        /// </summary>
        [ForeignKey("ClientSupplierId")]
        public ClientSupplier ClientSupplier { get; set; }

        /// <summary>
        /// The title of this job portal
        /// </summary>
        [Column("Title", Order = 2)] 
        public string Title { get; set; }

        /// <summary>
        /// The factor of this job portal
        /// </summary>
        [Column("Factor", Order = 3)]
        public decimal Factor { get; set; }
    }
}