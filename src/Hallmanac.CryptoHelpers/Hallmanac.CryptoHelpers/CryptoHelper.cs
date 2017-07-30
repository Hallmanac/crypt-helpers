using System;

namespace Hallmanac.CryptoHelpers
{
    public class CryptoHelper
    {
        /// <summary>
        /// Provides strong hashing services using using the standards from RFC2898 with key stretching and multiple hashing iterations on a SHA512 algorthim. 
        /// </summary>
        public IPasswordHashingSvc PasswordHashing { get; set; }
    }
}
