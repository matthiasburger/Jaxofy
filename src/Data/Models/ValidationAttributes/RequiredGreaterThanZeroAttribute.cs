using System.ComponentModel.DataAnnotations;

namespace DasTeamRevolution.Data.Models.ValidationAttributes
{
    public class RequiredGreaterThanZeroAttribute : ValidationAttribute
    {
        /// <summary>
        /// Designed for value-type number checks for Required
        /// </summary>
        /// <param name="value">The integer value of the selection</param>
        /// <returns>True if value is greater than zero</returns>
        public override bool IsValid(object value)
        {
            // return true if value is a non-null number > 0, otherwise return false
            return value != null && int.TryParse(value.ToString(), out int i) && i > 0;
        }
    }
}