namespace DasTeamRevolution.Data.Models.Base
{
    /// <summary>
    /// String constants for various database types (specific to SQL Server).
    /// </summary>
    public static class DatabaseType
    {
        public const string DateTime2NoPrecision = "datetime2(0)";
        public const string Date = "date";
        public const string SmallDateTime = "smalldatetime";
    }
}