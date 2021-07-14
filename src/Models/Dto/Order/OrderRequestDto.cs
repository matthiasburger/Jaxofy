using System.Collections.Generic;
using Jaxofy.Models.Dto.Assignment;

namespace Jaxofy.Models.Dto.Order
{
    /// <summary>
    /// Request DTO for creating <see cref="Order"/>s via the <see cref="OrdersController"/>.
    /// </summary>
    public class OrderRequestDto
    {
        /// <summary>
        /// The ID of the <see cref="Client"/> who placed this <see cref="Order"/>.
        /// </summary>
        public long ClientId { get; set; }

        /// <summary>
        /// Creates <see cref="Assignment"/>s from this list of <see cref="Proposal"/><c>.</c><see cref="Proposal.Id"/>
        /// and auto-assigns them to this <see cref="Order"/>.
        /// </summary>
        public ICollection<long> CreateFromProposals { get; set; }

        /// <summary>
        /// Existing <see cref="Assignment"/><c>.</c><see cref="Assignment.Id"/>s to link to the new <see cref="Order"/>.
        /// </summary>
        public ICollection<long> LinkAssignments { get; set; }

        /// <summary>
        /// Collection of <see cref="Assignment"/>s to create on the fly and auto-assign to this <see cref="Order"/>.
        /// </summary>
        public ICollection<AssignmentRequestDto> CreateAssignments { get; set; }
    }
}