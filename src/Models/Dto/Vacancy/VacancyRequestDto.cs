using System;
using Jaxofy.Models.Dto.PostalAddress;

namespace Jaxofy.Models.Dto.Vacancy
{
    public class VacancyRequestDto
    {
        /// <summary>
        /// Uniquely indexed slug string that is human-readable and allows easy identification of a <see cref="Vacancy"/> inside the db.
        /// Can be used for e.g. more readable, more memorable URLs. 
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// The <see cref="Client"/> to which this <see cref="Vacancy"/> is assigned.
        /// </summary>
        public long? ClientId { get; set; }

        /// <summary>
        /// The vacancy's <see cref="PostalAddress"/>.
        /// </summary>
        public PostalAddressRequestDto PostalAddress { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the vacancy is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Gets or sets the vacancies deadline
        /// </summary>
        public DateTime? Deadline { get; set; }
        
        /// <summary>
        /// Gets or sets the vacancies starting date. Value has no time precision.
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// Gets or sets the vacancies ending date. Value has no time precision.
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Gets or sets the vacancy-title
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets a description for the vacancy
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets a job-profile-id for the vacancy
        /// </summary>
        public long? JobProfileId { get; set; }
        
        /// <summary>
        /// Gets or sets the amount of employees the vacancy requires
        /// </summary>
        public int? RequiredAmountEmployees { get; set; }
    }
}