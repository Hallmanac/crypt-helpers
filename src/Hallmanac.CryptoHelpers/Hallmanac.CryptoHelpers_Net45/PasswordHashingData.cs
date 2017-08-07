namespace Hallmanac.CryptoHelpers
{
    /// <summary>
    /// Class type to hold data about the hashing of a password. For example, the Number of Iterations
    /// the hashing went through, the byte array size of the salt, the string value of the salt,
    /// and the string value of the actual hashed password itself.
    /// </summary>
    public class PasswordHashingData
    {
        /// <summary>
        /// Number of hashing iterations the password went through.
        /// </summary>
        public int NumberOfIterations { get; set; } = 5000;

        /// <summary>
        /// Size of byte array for the Salt of the password 
        /// </summary>
        public int SaltSize { get; set; } = 64;

        /// <summary>
        /// The string value of the salt itself (typically saved alongside the password in a database)
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// The string value of the password after it has gone through all the hashing and salting
        /// </summary>
        public string HashedPassword { get; set; }
    }
}