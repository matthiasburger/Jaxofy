using DasTeamRevolution.Data.Models;

namespace DasTeamRevolution.Models.Enums
{
    /// <summary>
    /// The various states a <see cref="Vacancy"/> can be in.
    /// </summary>
    public enum VacancyStateType : byte
    {
        Created = 0,
        Rejected = 1,
        Accepted = 2,
        Expired = 3
    }
}