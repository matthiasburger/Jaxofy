using System;
using System.Collections.Generic;
using DasTeamRevolution.Controllers;
using DasTeamRevolution.Models.Dto.RecordCreation;
using DasTeamRevolution.Models.Dto.RecordModification;

namespace DasTeamRevolution.Models.Dto.Order
{
    /// <summary>
    /// <see cref="OrdersController"/> OData endpoint response DTO.
    /// </summary>
    public class OrderResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies an order.
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// The ID of the <see cref="Client"/> who placed this <see cref="Order"/>.
        /// </summary>
        public long ClientId { get; set; }
        
        /// <summary>
        /// Order creation metadata.
        /// </summary>
        public RecordCreationResponseDto Creation { get; set; }

        /// <summary>
        /// Last order's modification metadata.
        /// </summary>
        public RecordModificationResponseDto LastModification { get; set; }
        
        /// <summary>
        /// The IDs of the <see cref="Assignment"/>s associated with this <see cref="Order"/>.
        /// </summary>
        public ICollection<long> AssignmentIds { get; set; }
        
        /// <summary>
        /// Gets the date, when the order was sent
        /// </summary>
        public DateTime? OrderedOn { get; set; }

        /// <summary>
        /// Gets the vacancies start-date
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Gets the vacancies end-date
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Returns the amount of assignments for this order
        /// </summary>
        public long AssignmentsCount { get; set; }
    }
}