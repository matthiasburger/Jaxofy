namespace DasTeamRevolution.Models.Settings
{
    /// <summary>
    /// These are the hashing parameters for Argon2 that are meant to drive
    /// the slowness of the algorithm: the slower the safer. <para> </para>
    /// Password hashing times of 0.5s per attempt are good enough and fairly
    /// balanced.
    /// </summary>
    public class Argon2HashingParameters
    {
        /// <summary>
        /// The name of the settings section inside the appsettings.json file.
        /// </summary>
        public static string SectionName => "Argon2HashingParameters";
        
        /// <summary>
        /// How many iterations the Argon2 hashing should perform when hashing passwords.
        /// </summary>
        public int TimeCost { get; set; }
        
        /// <summary>
        /// The memory allocation cost in KiB.
        /// Argon2 will require at least this many KiB of RAM for hashing.
        /// </summary>
        public int MemoryCostKiB { get; set; }
        
        /// <summary>
        /// The amount of threads that must be allocated for hashing.
        /// The more there are, the more a potential attacker needs to spend
        /// for his VM in terms of available CPU cores ;)
        /// </summary>
        public int Parallelism { get; set; }

        /// <summary>
        /// Desired length of the salt bytes.
        /// </summary>
        public int SaltLength { get; set; } = 32;

        /// <summary>
        /// Desired length of the Argon2 hash bytes.
        /// </summary>
        public int HashLength { get; set; } = 64;
    }
}