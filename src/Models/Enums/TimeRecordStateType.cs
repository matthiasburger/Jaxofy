using DasTeamRevolution.Data.Models;

namespace DasTeamRevolution.Models.Enums
{
    /// <summary>
    /// The type of state a <see cref="TimeRecord"/> can be in.
    /// </summary>
    public enum TimeRecordStateType : byte
    {
        Created = 0,
        Rejected = 1,
        Accepted = 2
    }
}