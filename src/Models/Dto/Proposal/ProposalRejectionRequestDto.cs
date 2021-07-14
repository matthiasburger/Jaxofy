using System.Collections.Generic;

namespace Jaxofy.Models.Dto.Proposal
{
    /// <summary>
    /// Proposal PUT endpoint request DTO for rejecting one or more proposals.
    /// </summary>
    public class ProposalRejectionRequestDto
    {
        /// <summary>
        /// The IDs of the <see cref="Proposal"/>s to reject.
        /// </summary>
        public List<long> ProposalIds { get; set; }
    }
}