namespace DasTeamRevolution.Models.Dto.ClientGroup
{
    public class ClientGroupRequestDto
    {
        /// <summary>
        /// The top-level parent <see cref="Data.Models.ClientHeader"/> associated with this <see cref="ClientGroup"/>.
        /// </summary>
        public long? HeaderId { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the clients group
        /// </summary>
        public string Name { get; set; }
    }
}