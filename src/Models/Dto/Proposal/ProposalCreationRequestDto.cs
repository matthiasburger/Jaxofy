using System;
using System.ComponentModel.DataAnnotations;

using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Data.Models.ValidationAttributes;
using DasTeamRevolution.Models.Dto.Document;

namespace DasTeamRevolution.Models.Dto.Proposal
{
    /// <summary>
    /// DTO for <see cref="Proposal"/> creation POST requests.
    /// </summary>
    public class ProposalCreationRequestDto
    {
        /// <summary>
        /// The ID of the <see cref="Vacancy"/> for which an <see cref="Employee"/>
        /// (from the <see cref="Supplier"/>'s pool of <see cref="PoolEmployee"/>s)
        /// needs to be proposed.
        /// </summary>
        [Required, RequiredGreaterThanZero(ErrorMessage = "Value of {0} needs to be greater than 0.")]
        public long VacancyId { get; set; }

        /// <summary>
        /// ID of the <see cref="PoolEmployee"/> to propose.
        /// </summary>
        [Required, RequiredGreaterThanZero(ErrorMessage = "Value of {0} needs to be greater than 0.")]
        public long PoolEmployeeId { get; set; }

        /// <summary>
        /// Comment to attach to the <see cref="Proposal"/>.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The ID of the <see cref="PoolEmployeeDocument"/>s
        /// to associate with this <see cref="Proposal"/>.
        /// </summary>
        public long[] PoolEmployeeDocumentIds { get; set; } = Array.Empty<long>();

        /// <summary>
        /// Attachments to directly include/upload and associate with the <see cref="Proposal"/>.
        /// </summary>
        public DocumentUploadDto[] ProposalDocuments { get; set; } = Array.Empty<DocumentUploadDto>();
    }
}