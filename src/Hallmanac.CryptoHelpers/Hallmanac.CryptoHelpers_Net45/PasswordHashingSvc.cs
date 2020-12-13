using System;
using System.Security.Cryptography;

namespace Hallmanac.CryptoHelpers
{
    /// <summary>
    ///     Provides strong hashing services using the standards from RFC2898 with key stretching and multiple hashing
    ///     iterations on a SHA512 algorithm.
    /// </summary>
    public class PasswordHashingSvc : IPasswordHashingSvc
    {
        private const int MinIterationRange = 8000;
        private const int MaxIterationRange = 15000;
        private const int MinSaltSize = 64;
        private const int MaxSaltSize = 96;
        private const int MinPasswordSize = 8;
        private const int MaxPasswordSize = 1024;
        private const int AppHashIterations = 6000;
        private const int KeyLength = 64;

        private readonly string _globalApplicationSalt;


        /// <summary>
        ///     Creates a new instance of the PasswordEncryptionSvc. If the application has a global application salt, then pass
        ///     that into this constructor otherwise the hashing will not match up.
        /// </summary>
        /// <param name="globalApplicationSalt">
        ///     The Global Application Salt adds an extra layer of security to the password hashing
        ///     process by hashing each password with this salt first and then proceeding to the normal route of hashing a
        ///     password. The
        ///     globalApplicationSalt itself is intended to be kept secret (i.e. stored in something like Azure Key Vault) which
        ///     means that
        ///     even if someone were able to gain access to passwords they would not be able to brute force the password because,
        ///     in theory,
        ///     they would not have access to the global application salt.
        /// </param>
        public PasswordHashingSvc(string globalApplicationSalt = null)
        {
            _globalApplicationSalt = globalApplicationSalt;
        }


        /// <summary>
        ///     Computes a hash for a given password and returns a <see cref="PasswordHashingData" /> object to hold the elements
        ///     that made up the
        ///     hashed password
        /// </summary>
        /// <param name="givenPassword"></param>
        public CommandResult<PasswordHashingData> HashPassword(string givenPassword)
        {
            if (String.IsNullOrWhiteSpace(givenPassword)) return CommandResultFactory.Fail("The given password was null or empty", (PasswordHashingData) null);

            var hashData = new PasswordHashingData();
            var rand = new Random();

            // Set the hash data
            hashData.NumberOfIterations = rand.Next(MinIterationRange, MaxIterationRange);
            hashData.SaltSize = rand.Next(MinSaltSize, MaxSaltSize);
            var saltByteArray = GetRandomSalt(hashData.SaltSize);
            hashData.Salt = saltByteArray.ToHexString();

            // Run initial hash at an application level
            var appHashedPassword = GetAppLevelPasswordHash(givenPassword);

            // Take the output of the initial hashing and run it through proper hashing with key stretching
            var hashedPasswordResult =
                ComputePasswordAndSaltBytes(saltByteArray, appHashedPassword, hashData.NumberOfIterations)
                    .Then(computedBytes =>
                    {
                        var hashedPassword = computedBytes.ToHexString();
                        return CommandResultFactory.Ok<string>(hashedPassword);
                    });
            if (hashedPasswordResult.IsFailure) return CommandResultFactory.Fail(hashedPasswordResult.Message, hashData);
            hashData.HashedPassword = hashedPasswordResult.Value;
            return CommandResultFactory.Ok(hashData);
        }


        /// <summary>
        ///     This will compute a hash for the given password and salt using the iterationCount parameter to determine how many
        ///     times the
        ///     hashing will occur on the password + salt.
        /// </summary>
        /// <param name="salt">The salt that is added on to the end of the password</param>
        /// <param name="password">The password to be hashed</param>
        /// <param name="iterationCount">The number of times the password+salt will have a hash computed</param>
        /// <returns></returns>
        public CommandResult<byte[]> ComputePasswordAndSaltBytes(byte[] salt, string password, int iterationCount = MinIterationRange)
        {
            if (String.IsNullOrWhiteSpace(password)) return CommandResultFactory.Fail("Password was null", (byte[]) null);
            if (salt == null || salt.Length < 1) return CommandResultFactory.Fail("The salt did not meet the minimum length", (byte[]) null);
            if (salt.Length > MaxSaltSize) return CommandResultFactory.Fail("The salt length was greater than the maximum allowed", (byte[]) null);

            var convertedPassword = password.ToUtf8Bytes();
            if (convertedPassword.Length > MaxPasswordSize)
                return CommandResultFactory.Fail($"The password length was greater than the maximum allowed. Please make it less than {MaxPasswordSize}",
                    (byte[]) null);

            if (convertedPassword.Length < MinPasswordSize)
                return CommandResultFactory.Fail($"The password length was less than the minimum allowed. Please make it greater than {MinPasswordSize}",
                    (byte[]) null);

            try
            {
                var resultValue = new Rfc2898(convertedPassword, salt, iterationCount).GetDerivedKeyBytes_PBKDF2_HMACSHA512(KeyLength);
                return CommandResultFactory.Ok(resultValue);
            }
            catch (IterationsLessThanRecommended)
            {
                return CommandResultFactory.Fail("The number of hash iterations did not meet minimum standards", (byte[]) null);
            }
            catch (SaltLessThanRecommended)
            {
                return CommandResultFactory.Fail("The salt length is less than the minimum standards", (byte[]) null);
            }
            catch (Exception e)
            {
                return CommandResultFactory.Fail(
                    $"There was an unspecified error that ocurred from an exception being thrown. The exception is as follows:\n{e.Message}",
                    (byte[]) null);
            }
        }


        /// <summary>
        ///     Generates a random byte array with a length that is equivalent to the given "saltLength" parameter.
        /// </summary>
        /// <param name="saltLength">The length of the salt to be generated.</param>
        public byte[] GetRandomSalt(int saltLength)
        {
            var salt = new byte[saltLength];
            RandomNumberGenerator.Create().GetBytes(salt);
            return salt;
        }


        /// <summary>
        ///     Hashes the given plain-text password using the global application hash. If there is no global application hash then
        ///     the given password
        ///     is returned simply hashed without the global app salt.
        /// </summary>
        /// <param name="givenPassword"></param>
        public string GetAppLevelPasswordHash(string givenPassword)
        {
            if (String.IsNullOrWhiteSpace(givenPassword)) return givenPassword;

            if (String.IsNullOrWhiteSpace(_globalApplicationSalt))
            {
                var hash512Password = ComputeSha512HexString(givenPassword);
                return hash512Password;
            }

            var appSaltByteArray = _globalApplicationSalt.ToHexBytes();
            var hashedPasswordResult = ComputePasswordAndSaltBytes(appSaltByteArray, givenPassword, AppHashIterations)
                .Then(byteResult =>
                {
                    var hashedPassword = byteResult.ToHexString();
                    return CommandResultFactory.Ok<string>(hashedPassword);
                });
            return hashedPasswordResult.IsFailure ? givenPassword : hashedPasswordResult.Value;
        }


        /// <summary>
        ///     Compares a given (plain text) password with the (already hashed) password that is inside the hashData object.
        /// </summary>
        /// <param name="givenPassword">Plain text password to compare with</param>
        /// <param name="hashData">
        ///     The <see cref="PasswordHashingData" /> object that contains salt size, current hashed password, etc. for use in the
        ///     comparison
        ///     of the two passwords
        /// </param>
        public CommandResult ComparePasswords(string givenPassword, PasswordHashingData hashData)
        {
            if (String.IsNullOrWhiteSpace(givenPassword) || String.IsNullOrWhiteSpace(hashData?.HashedPassword) ||
                String.IsNullOrWhiteSpace(hashData.Salt))
                return CommandResultFactory.Fail("The given data to compare passwords was invalid.", false);

            var saltByteArray = hashData.Salt.ToHexBytes();

            // Run initial hash at an application level
            var appHashedPassword = GetAppLevelPasswordHash(givenPassword);

            // Take the output of the initial hashing and run it through proper hashing with key stretching
            var hashedPasswordResult =
                ComputePasswordAndSaltBytes(saltByteArray, appHashedPassword, hashData.NumberOfIterations)
                    .Then(computedBytes =>
                    {
                        var hashedPassword = computedBytes.ToHexString();
                        return CommandResultFactory.Ok<string>(hashedPassword);
                    });
            if (hashedPasswordResult.IsFailure)
                return CommandResultFactory.Fail(
                    $"Computing the hash for the given password was not successful due to the following:\n{hashedPasswordResult.Message}");

            return hashedPasswordResult.Value == hashData.HashedPassword
                ? CommandResultFactory.Ok()
                : CommandResultFactory.Fail("Passwords did not match");
        }


        /// <summary>
        ///     Generates a random byte array key based on the byte length given and returns it as a hexadecimal string.
        /// </summary>
        /// <param name="byteLength">Length of Byte array used in the random generator</param>
        /// <returns>Hexadecimal text representation of the randomly generated bytes.</returns>
        [Obsolete("This method is available on the RandomGenerators class and will be removed from this class in future iterations. It is recommended that you change calls to this method to the RandomGenerators.GenerateHexKeyFromByteLength(...) method")]
        public string GenerateHexKeyFromByteLength(int byteLength)
        {
            var key = new byte[byteLength];
            GenRandomBytes(key);
            return key.ToHexString();
        }


        /// <summary>
        ///     Generates random, non-zero bytes using the RandomNumberGenerator
        /// </summary>
        /// <param name="buffer">Length of random bytes to be generated.</param>
        [Obsolete("This method is available on the RandomGenerators class and will be removed from this class in future iterations. It is recommended that you change calls to this method to the equivalent method on the RandomGenerators classs.")]
        public void GenerateRandomBytes(byte[] buffer)
        {
            if (buffer == null)
                return;
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
        }


        /// <summary>
        ///     Computes a hash based on the HMACSHA512 algorithm using the given key.
        /// </summary>
        [Obsolete("This method is available on the HashingHelpers class under the same method name. Please refactor calls to this method to use the HashingHelpers class method")]
        public string ComputeSha512ToHexString(string textToHash)
        {
            if (String.IsNullOrEmpty(textToHash))
                return null;
            var sha512Cng = SHA512.Create();
            var hashBytes = sha512Cng.ComputeHash(textToHash.ToUtf8Bytes());
            var hashToHexString = hashBytes.ToHexString();
            return hashToHexString;
        }


        private static string ComputeSha512HexString(string textToHash)
        {
            if (String.IsNullOrEmpty(textToHash))
                return null;
            var sha512Cng = SHA512.Create();
            var hashBytes = sha512Cng.ComputeHash(textToHash.ToUtf8Bytes());
            var hashToHexString = hashBytes.ToHexString();
            return hashToHexString;
        }


        private static void GenRandomBytes(byte[] buffer)
        {
            if (buffer == null)
                return;
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
        }
    }


    /// <summary>
    ///     Provides strong hashing services using using the standards from RFC2898 with key stretching and multiple hashing
    ///     iterations on a SHA512 algorithm.
    /// </summary>
    public interface IPasswordHashingSvc
    {
        /// <summary>
        ///     Compares a given (plain text) password with the (already hashed) password that is inside the hashData object.
        /// </summary>
        /// <param name="givenPassword">Plain text password to compare with</param>
        /// <param name="hashData">
        ///     The <see cref="PasswordHashingData" /> object that contains salt size, current hashed password, etc. for use in the
        ///     comparison
        ///     of the two passwords
        /// </param>
        CommandResult ComparePasswords(string givenPassword, PasswordHashingData hashData);

        /// <summary>
        ///     This will compute a hash for the given password and salt using the iterationCount parameter to determine how many
        ///     times the
        ///     hashing will occur on the password + salt.
        /// </summary>
        /// <param name="salt">The salt that is added on to the end of the password</param>
        /// <param name="password">The password to be hashed</param>
        /// <param name="iterationCount">The number of times the password+salt will have a hash computed</param>
        /// <returns></returns>
        CommandResult<byte[]> ComputePasswordAndSaltBytes(byte[] salt, string password, int iterationCount = 8000);

        /// <summary>
        ///     Computes a hash based on the HMACSHA512 algorithm using the given key.
        /// </summary>
        [Obsolete("This method is available on the HashingHelpers class under the same method name. Please refactor calls to this method to use the HashingHelpers class method")]
        string ComputeSha512ToHexString(string textToHash);

        /// <summary>
        ///     Generates a random byte array key based on the byte length given and returns it as a hexadecimal string.
        /// </summary>
        /// <param name="byteLength">Length of Byte array used in the random generator</param>
        /// <returns>Hexadecimal text representation of the randomly generated bytes.</returns>
        [Obsolete("This method is available on the RandomGenerators class and will be removed from this class in future iterations. It is recommended that you change calls to this method to the RandomGenerators.GenerateHexKeyFromByteLength(...) method")]
        string GenerateHexKeyFromByteLength(int byteLength);

        /// <summary>
        ///     Generates random, non-zero bytes using the RandomNumberGenerator
        /// </summary>
        /// <param name="buffer">Length of random bytes to be generated.</param>
        [Obsolete("This method is available on the RandomGenerators class and will be removed from this class in future iterations. It is recommended that you change calls to this method to the equivalent method on the RandomGenerators classs.")]
        void GenerateRandomBytes(byte[] buffer);

        /// <summary>
        ///     Hashes the given plain-text password using the global application hash. If there is no global application hash then
        ///     the given password
        ///     is returned simply hashed without the global app salt.
        /// </summary>
        /// <param name="givenPassword"></param>
        string GetAppLevelPasswordHash(string givenPassword);

        /// <summary>
        ///     Generates a random byte array with a length that is equivalent to the given "saltLength" parameter.
        /// </summary>
        /// <param name="saltLength">The length of the salt to be generated.</param>
        byte[] GetRandomSalt(int saltLength);

        /// <summary>
        ///     Computes a hash for a given password and returns a <see cref="PasswordHashingData" /> object to hold the elements
        ///     that made up the
        ///     hashed password
        /// </summary>
        /// <param name="givenPassword"></param>
        CommandResult<PasswordHashingData> HashPassword(string givenPassword);
    }
}