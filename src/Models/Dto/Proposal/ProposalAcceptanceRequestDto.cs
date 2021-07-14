using System.Collections.Generic;

namespace DasTeamRevolution.Models.Dto.Proposal
{
    /// <summary>
    /// Proposal PUT endpoint request DTO for accepting one or more proposals.
    /// </summary>
    public class ProposalAcceptanceRequestDto
    {
        /// <summary>
        /// The IDs of the <see cref="Proposal"/>s to accept.
        /// </summary>
        public List<long> ProposalIds { get; set; }
    }
}