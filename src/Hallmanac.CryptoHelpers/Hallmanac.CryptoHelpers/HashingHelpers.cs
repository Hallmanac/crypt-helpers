using System;
using System.Security.Cryptography;


namespace Hallmanac.CryptoHelpers
{
    /// <summary>
    /// Helper methods that allow for easily hashing data using various algorithms and returning strings
    /// </summary>
    public class HashingHelpers : IHashingHelpers
    {
        /// <summary>
        /// This will compute a basic MD5 hash for uses within this Encryption Service class. This hash is not recommended for
        /// true security use cases.
        /// </summary>
        public string ComputeMD5Hash(string textToHash, string salt = "")
        {
            if (string.IsNullOrEmpty(textToHash))
            {
                return null;
            }
            var hashAlgorithm = MD5.Create();
            var hash = hashAlgorithm.ComputeHash((textToHash + salt).ToUtf8Bytes());
            var hashHexString = hash.ToHexString();
            return hashHexString;
        }


        /// <summary>
        /// This will compute a basic SHA1 hash for uses within this Encryption Service class. This hash is not recommended for
        /// true security use cases.
        /// </summary>
        public string ComputeSha1Hash(string textToHash, string salt = "")
        {
            if (string.IsNullOrEmpty(textToHash))
            {
                return null;
            }
            var hashAlgorithm = SHA1.Create();
            var hash = hashAlgorithm.ComputeHash((textToHash + salt).ToUtf8Bytes());
            var hashHexString = hash.ToHexString();
            return hashHexString;
        }


        /// <summary>
        /// This will compute a basic SHA256 hash for uses within this Encryption Service class. This hash is not recommended for
        /// password use cases. Use the <see cref="IPasswordHashingSvc"/> to acheive proper password hashing
        /// </summary>
        public string ComputeSha256Hash(string textToHash, string salt = "")
        {
            if (string.IsNullOrEmpty(textToHash))
            {
                return null;
            }
            var hashAlgorithm = SHA256.Create();
            var hash = hashAlgorithm.ComputeHash((textToHash + salt).ToUtf8Bytes());
            var hashHexString = hash.ToHexString();
            return hashHexString;
        }


        /// <summary>
        /// This will compute a basic SHA384 hash for uses within this Encryption Service class. This hash is not recommended for
        /// password use cases. Use the <see cref="IPasswordHashingSvc"/> to acheive proper password hashing
        /// </summary>
        public string ComputeSha384Hash(string textToHash, string salt = "")
        {
            if (string.IsNullOrEmpty(textToHash))
            {
                return null;
            }
            var hashAlgorithm = SHA384.Create();
            var hash = hashAlgorithm.ComputeHash((textToHash + salt).ToUtf8Bytes());
            var hashHexString = hash.ToHexString();
            return hashHexString;
        }


        /// <summary>
        /// This will compute a basic SHA512 hash for uses within this Encryption Service class. This hash is not recommended for
        /// password use cases. Use the <see cref="IPasswordHashingSvc"/> to acheive proper password hashing
        /// </summary>
        public string ComputeSha512Hash(string textToHash, string salt = "")
        {
            if (string.IsNullOrEmpty(textToHash))
            {
                return null;
            }
            var hashAlgorithm = SHA512.Create();
            var hash = hashAlgorithm.ComputeHash((textToHash + salt).ToUtf8Bytes());
            var hashHexString = hash.ToHexString();
            return hashHexString;
        }


        /// <summary>
        /// Computes an HMAC hash based on the MD5 algorithm using the given key and returns a string in hexidecmal format
        /// </summary>
        /// <param name="text">Text to Hash</param>
        /// <param name="key">Key to use for the HMAC part</param>
        public string ComputeHmacMD5ForHex(string text, string key)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            var hmacMd5 = new HMACMD5(key.ToUtf8Bytes());
            var hash = hmacMd5.ComputeHash(text.ToUtf8Bytes());
            var hashText = hash.ToHexString();
            return hashText;
        }


        /// <summary>
        ///     Computes a hash based on the HMACSHA1 algorithm using the given key and returns a string in hexadecimal format.
        /// </summary>
        public string ComputeHmacSha1ForHex(string textToHash, string key)
        {
            if (string.IsNullOrEmpty(textToHash))
            {
                return null;
            }
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            var keyBytes = key.ToUtf8Bytes();
            var textBytes = textToHash.ToUtf8Bytes();
            var hmacSha1 = new HMACSHA1(keyBytes);
            var hash = hmacSha1.ComputeHash(textBytes);
            var hashToHexString = hash.ToHexString();
            return hashToHexString;
        }


        /// <summary>
        ///     Computes a hash based on the HMACSHA256 algorithm using the given key and returns a string in hexadecimal format.
        /// </summary>
        public string ComputeHmacSha256ForHex(string textToHash, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            if (string.IsNullOrEmpty(textToHash))
            {
                return null;
            }
            var hmacsha256 = new HMACSHA256(key.ToUtf8Bytes());
            var hash = hmacsha256.ComputeHash(textToHash.ToUtf8Bytes());
            var hashToHexString = hash.ToHexString();
            return hashToHexString;
        }



        /// <summary>
        ///     Computes a hash based on the HMACSHA384 algorithm using the given key and returns a string in hexadecimal format.
        /// </summary>
        public string ComputeHmacSha384ForHex(string textToHash, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            if (string.IsNullOrEmpty(textToHash))
            {
                return null;
            }
            var hmacsha384 = new HMACSHA384(key.ToUtf8Bytes());
            var hash = hmacsha384.ComputeHash(textToHash.ToUtf8Bytes());
            var hashToHexString = hash.ToHexString();
            return hashToHexString;
        }


        /// <summary>
        ///     Computes a hash based on the HMACSHA512 algorithm using the given key and returns a string in hexadecimal format.
        /// </summary>
        public string ComputeHmacSha512ForHex(string textToHash, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            if (string.IsNullOrEmpty(textToHash))
            {
                return null;
            }
            var hmacsha512 = new HMACSHA512(key.ToUtf8Bytes());
            var hash = hmacsha512.ComputeHash(textToHash.ToUtf8Bytes());
            var hashToHexString = hash.ToHexString();
            return hashToHexString;
        }


        /// <summary>
        ///     Computes a hash based on the HMACMD5 algorithm using the given key and returns a Base64 encoded string.
        /// </summary>
        public string ComputeHmacMD5ForBase64(string textToEncode, string key)
        {
            if (string.IsNullOrWhiteSpace(textToEncode) || string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            var hmacmd5 = new HMACMD5(key.ToUtf8Bytes());
            var hash = hmacmd5.ComputeHash(textToEncode.ToUtf8Bytes());
            var base64String = hash.ToBase64String();
            return base64String;
        }


        /// <summary>
        ///     Computes a hash based on the HMACSHA1 algorithm using the given key and returns a Base64 encoded string.
        /// </summary>
        public string ComputeHmacSha1ForBase64(string textToEncode, string key)
        {
            if (string.IsNullOrWhiteSpace(textToEncode) || string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            var hmacSha1 = new HMACSHA1(key.ToUtf8Bytes());
            var hash = hmacSha1.ComputeHash(textToEncode.ToUtf8Bytes());
            var base64String = hash.ToBase64String();
            return base64String;
        }


        /// <summary>
        ///     Computes a hash based on the HMACSHA384 algorithm using the given key and returns a Base64 encoded string.
        /// </summary>
        public string ComputeHmacSha384ForBase64(string textToEncode, string key)
        {
            if (string.IsNullOrWhiteSpace(textToEncode) || string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            var hmacsha384 = new HMACSHA384(key.ToUtf8Bytes());
            var hash = hmacsha384.ComputeHash(textToEncode.ToUtf8Bytes());
            var base64String = hash.ToBase64String();
            return base64String;
        }


        /// <summary>
        ///     Computes a hash based on the HMACSHA512 algorithm using the given key and returns a Base64 encoded string.
        /// </summary>
        public string ComputeHmacSha512ForBase64(string textToEncode, string key)
        {
            if (string.IsNullOrWhiteSpace(textToEncode) || string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            var hmacsha512 = new HMACSHA512(key.ToUtf8Bytes());
            var hash = hmacsha512.ComputeHash(textToEncode.ToUtf8Bytes());
            var base64String = hash.ToBase64String();
            return base64String;
        }
    }


    /// <summary>
    /// Helper methods that allow for easily hashing data using various algorithms and returning strings
    /// </summary>
    public interface IHashingHelpers
    {
        /// <summary>
        /// This will compute a basic MD5 hash for uses within this Encryption Service class. This hash is not recommended for
        /// true security use cases.
        /// </summary>
        string ComputeMD5Hash(string textToHash, string salt = "");


        /// <summary>
        /// This will compute a basic SHA1 hash for uses within this Encryption Service class. This hash is not recommended for
        /// true security use cases.
        /// </summary>
        string ComputeSha1Hash(string textToHash, string salt = "");


        /// <summary>
        /// This will compute a basic SHA256 hash for uses within this Encryption Service class. This hash is not recommended for
        /// password use cases. Use the <see cref="IPasswordHashingSvc"/> to acheive proper password hashing
        /// </summary>
        string ComputeSha256Hash(string textToHash, string salt = "");


        /// <summary>
        /// This will compute a basic SHA384 hash for uses within this Encryption Service class. This hash is not recommended for
        /// password use cases. Use the <see cref="IPasswordHashingSvc"/> to acheive proper password hashing
        /// </summary>
        string ComputeSha384Hash(string textToHash, string salt = "");


        /// <summary>
        /// This will compute a basic SHA512 hash for uses within this Encryption Service class. This hash is not recommended for
        /// password use cases. Use the <see cref="IPasswordHashingSvc"/> to acheive proper password hashing
        /// </summary>
        string ComputeSha512Hash(string textToHash, string salt = "");


        /// <summary>
        /// Computes an HMAC hash based on the MD5 algorithm using the given key
        /// </summary>
        /// <param name="text">Text to Hash</param>
        /// <param name="key">Key to use for the HMAC part</param>
        string ComputeHmacMD5ForHex(string text, string key);


        /// <summary>
        ///     Computes a hash based on the HMACSHA1 algorithm using the given key.
        /// </summary>
        string ComputeHmacSha1ForHex(string textToHash, string key);


        /// <summary>
        ///     Computes a hash based on the HMACSHA256 algorithm using the given key.
        /// </summary>
        string ComputeHmacSha256ForHex(string textToHash, string key);


        /// <summary>
        ///     Computes a hash based on the HMACSHA384 algorithm using the given key.
        /// </summary>
        string ComputeHmacSha384ForHex(string textToHash, string key);


        /// <summary>
        ///     Computes a hash based on the HMACSHA512 algorithm using the given key.
        /// </summary>
        string ComputeHmacSha512ForHex(string textToHash, string key);


        /// <summary>
        ///     Computes a hash based on the HMACMD5 algorithm using the given key and returns a Base64 encoded string.
        /// </summary>
        string ComputeHmacMD5ForBase64(string textToEncode, string key);


        /// <summary>
        ///     Computes a hash based on the HMACSHA1 algorithm using the given key and returns a Base64 encoded string.
        /// </summary>
        string ComputeHmacSha1ForBase64(string textToEncode, string key);


        /// <summary>
        ///     Computes a hash based on the HMACSHA384 algorithm using the given key and returns a Base64 encoded string.
        /// </summary>
        string ComputeHmacSha384ForBase64(string textToEncode, string key);


        /// <summary>
        ///     Computes a hash based on the HMACSHA512 algorithm using the given key and returns a Base64 encoded string.
        /// </summary>
        string ComputeHmacSha512ForBase64(string textToEncode, string key);
    }
}