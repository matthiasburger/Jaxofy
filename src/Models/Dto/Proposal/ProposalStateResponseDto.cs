using Jaxofy.Models.Dto.RecordCreation;
using Jaxofy.Models.Enums;

namespace Jaxofy.Models.Dto.Proposal
{
    public class ProposalStateResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a dossier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The <see cref="ProposalStateType"/>.
        /// </summary>
        public ProposalStateType StateType { get; set; }

        /// <summary>
        /// The dossier state modification entry's author and timestamp.
        /// </summary>
        public RecordCreationResponseDto Creation { get; set; }
    }
}