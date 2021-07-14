using System.Collections.Generic;

using DasTeamRevolution.Models.Dto.Client;
using DasTeamRevolution.Models.Dto.RecordCreation;
using DasTeamRevolution.Models.Dto.RecordModification;

namespace DasTeamRevolution.Models.Dto.ClientGroup
{
    public class ClientGroupResponseDto
    {
        public long Id { get; set; }
        
        /// <summary>
        /// DB row creation metadata.
        /// </summary>
        public RecordCreationResponseDto Creation { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        public RecordModificationResponseDto LastModification { get; set; }
        
        /// <summary>
        /// The top-level parent <see cref="Data.Models.ClientHeader"/> associated with this <see cref="ClientGroup"/>.
        /// </summary>
        public long? HeaderId { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the clients group
        /// </summary>
        public string Name { get; set; }
        
        
        /// <summary>
        /// The <see cref="Client"/>s that belong to this <see cref="ClientGroup"/>.
        /// </summary>
        public IEnumerable<ClientResponseDto> Clients { get; set; }

    }
}