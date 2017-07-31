namespace Hallmanac.CryptoHelpers_Net45
{
    /// <summary>
    /// A set of classes that contain helper functions that relate to cryptography such as password hashing and symmetric encryption.
    /// </summary>
    public class CryptoHelper : ICryptoHelper
    {


        /// <summary>
        /// Provides strong hashing services using using the standards from RFC2898 with key stretching and multiple hashing iterations on a SHA512 algorthim. 
        /// </summary>
        public IPasswordHashingSvc PasswordHashing { get; set; }

        /// <summary>
        /// A service that provides helper methods to encrypt and decrypt text using symmetric encryption techniques. 
        /// </summary>
        public ISymmetricEncryptionSvc Encryption { get; set; }
    }


    /// <summary>
    /// A set of classes that contain helper functions that relate to cryptography such as password hashing and symmetric encryption.
    /// </summary>
    public interface ICryptoHelper
    {
        /// <summary>
        /// Provides strong hashing services using using the standards from RFC2898 with key stretching and multiple hashing iterations on a SHA512 algorthim. 
        /// </summary>
        IPasswordHashingSvc PasswordHashing { get; set; }

        /// <summary>
        /// A service that provides helper methods to encrypt and decrypt text using symmetric encryption techniques. 
        /// </summary>
        ISymmetricEncryptionSvc Encryption { get; set; }
    }
}
