using DasTeamRevolution.Models.Dto.Employee;
using DasTeamRevolution.Models.Dto.Supplier;
using DasTeamRevolution.Models.Dto.Vacancy;

namespace DasTeamRevolution.Models.Dto.Proposal
{
    using Data.Models;

    public class ProposalResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a dossier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets the current (most recent) <see cref="ProposalStateHistory"/> attributed to this <see cref="Proposal"/>.
        /// </summary>
        public ProposalStateResponseDto CurrentState { get; set; }

        /// <summary>
        /// ID of the <see cref="SupplierUser"/> in charge of this <see cref="Proposal"/>.
        /// </summary>
        public long? SupplierUserId { get; set; }

        /// <summary>
        /// The <see cref="Employee"/> associated with this <see cref="Proposal"/>.
        /// </summary>
        public long? EmployeeId { get; set; }

        /// <summary>
        /// Comment message string attached to this <see cref="Proposal"/>.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The ID of the <see cref="Vacancy"/> to which this <see cref="Proposal"/> belongs.
        /// </summary>
        public long? VacancyId { get; set; }

        public SlimmedSupplierResponseDto Supplier { get; set; }

        public SlimmedEmployeeResponseDto Employee { get; set; }
    }
}