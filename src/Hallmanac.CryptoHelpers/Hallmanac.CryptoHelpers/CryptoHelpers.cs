namespace Hallmanac.CryptoHelpers
{
    /// <summary>
    /// A set of classes that contain helper functions that relate to cryptography such as password hashing and symmetric encryption.
    /// </summary>
    public class CryptoHelpers : ICryptoHelpers
    {
        /// <summary>
        /// Constructor for a set of classes that contain helper functions that relate to cryptography such as password hashing and symmetric encryption.
        /// </summary>
        public CryptoHelpers(): this(new PasswordHashingSvc(), new SymmetricEncryptionSvc(), new RandomGenerators(), new HashingHelpers()){}


        /// <summary>
        /// Constructor for a set of classes that contain helper functions that relate to cryptography such as password hashing and symmetric encryption.
        /// </summary>
        public CryptoHelpers(IPasswordHashingSvc passwordHashingSvc, ISymmetricEncryptionSvc encryption, IRandomGenerators randomGenerators, IHashingHelpers hashingHelpers)
        {
            PasswordHashing = passwordHashingSvc;
            Encryption = encryption;
            RandomGenerators = randomGenerators;
            HashingHelpers = hashingHelpers;
        }


        /// <summary>
        /// Provides strong hashing services using using the standards from FRFC2898 with key stretching and multiple hashing iterations on a SHA512 algorthim. 
        /// </summary>
        public IPasswordHashingSvc PasswordHashing { get; set; }

        /// <summary>
        /// A service that provides helper methods to encrypt and decrypt text using symmetric encryption techniques. 
        /// </summary>
        public ISymmetricEncryptionSvc Encryption { get; set; }

        /// <summary>
        /// A series of helper methods to generate random data such as random bytes, random 32 bit number, random 64 bit number, etc.
        /// </summary>
        public IRandomGenerators RandomGenerators { get; set; }

        /// <summary>
        /// Helper methods that allow for easily hashing data using various algorithms and returning strings
        /// </summary>
        public IHashingHelpers HashingHelpers { get; set; }
    }


    /// <summary>
    /// A set of classes that contain helper functions that relate to cryptography such as password hashing and symmetric encryption.
    /// </summary>
    public interface ICryptoHelpers
    {
        /// <summary>
        /// Provides strong hashing services using using the standards from RFC2898 with key stretching and multiple hashing iterations on a SHA512 algorthim. 
        /// </summary>
        IPasswordHashingSvc PasswordHashing { get; set; }

        /// <summary>
        /// A service that provides helper methods to encrypt and decrypt text using symmetric encryption techniques. 
        /// </summary>
        ISymmetricEncryptionSvc Encryption { get; set; }

        /// <summary>
        /// A series of helper methods to generate random data such as random bytes, random 32 bit number, random 64 bit number, etc.
        /// </summary>
        IRandomGenerators RandomGenerators { get; set; }

        /// <summary>
        /// Helper methods that allow for easily hashing data using various algorithms and returning strings
        /// </summary>
        IHashingHelpers HashingHelpers { get; set; }
    }
}
