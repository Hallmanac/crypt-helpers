using System;
using System.Security.Cryptography;

using Funqy.CSharp;


namespace Hallmanac.CryptoHelpers
{
    public enum AesKeySize
    {
        Size256 = 256,
        Size128 = 128,
        Size192 = 192
    }


    public class SymmetricEncryptionSvc : ISymmetricEncryptionSvc
    {
        /// <summary>
        /// Sets up all the parameters and properties for the AES cipher used in the encryption.
        /// </summary>
        /// <param name="cipher">The already created AES cipher. This should be created within a using statement and then handed off to this method.</param>
        /// <param name="key">The (HEXADECIMAL) key used for encryption inside the cipher</param>
        /// <param name="keySize">Key size used for setting up the cipher</param>
        public FunqResult<Aes> SetupCipher(Aes cipher, string key, AesKeySize keySize)
        {
            if (cipher == null)
            {
                return FunqFactory.Fail("The given AES cipher object was null", (Aes)null);
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                return FunqFactory.Fail("The cipher creation key for the encryption was null or empty", (Aes)null);
            }

            byte[] keyBytes;
            try
            {
                keyBytes = key.ToHexBytes();
                if (keyBytes == null || keyBytes.Length != (int)keySize / 8)
                {
                    return FunqFactory.Fail($"The cipher creation key for the encryption did not match the specified key size of {keySize}",
                                            (Aes)null);
                }
            }
            catch (FormatException)
            {
                return FunqFactory.Fail("The key was malformed and therefore threw a Format Exception when converting to a byte array.", (Aes)null);
            }
            catch (Exception e)
            {
                return FunqFactory.Fail($"There was an exception thrown while trying to create the cipher. It is as follows:\n\t{e.Message}", (Aes)null);
            }

            cipher.KeySize = (int)keySize;
            cipher.BlockSize = 128; // This is the default block size for AES encryption and apparently should not be changed according to what I'm reading
            cipher.Padding = PaddingMode.PKCS7;
            cipher.Mode = CipherMode.CBC;
            cipher.Key = keyBytes;
            cipher.GenerateIV();
            return FunqFactory.Ok(cipher);
        }


        /// <summary>
        /// Encrypts the given text using the given key and key size for cipher creation.
        /// </summary>
        /// <param name="textForEncryption">Text to encrypt</param>
        /// <param name="key">The (HEXADECIMAL) encryption key to use for creating the cipher</param>
        /// <param name="keySize">Size of the key used for creating the cipher</param>
        public FunqResult<string> Encrypt(string textForEncryption, string key, AesKeySize keySize)
        {
            if (string.IsNullOrWhiteSpace(textForEncryption))
            {
                return FunqFactory.Fail("There was nothing to encrypt", (string)null);
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                return FunqFactory.Fail("The given encryption key was null or empty", textForEncryption);
            }

            try
            {
                FunqResult<string> result;
                using (var aesCipher = Aes.Create())
                {
                    result = SetupCipher(aesCipher, key, keySize)
                        .Then(aes =>
                        {
                            var initVector = aes.IV.ToHexString();
                            var textForEncryptBytes = textForEncryption.ToUtf8Bytes();
                            var cryptoTransform = aes.CreateEncryptor();
                            var cipherTextBytes = cryptoTransform.TransformFinalBlock(textForEncryptBytes, 0, textForEncryption.Length);
                            var cipherText = cipherTextBytes.ToHexString();
                            var textResult = $"{initVector}_{cipherText}";
                            return FunqFactory.Ok<string>(textResult);
                        });
                }
                return result;
            }
            catch (Exception e)
            {
                var message = $"An exception was thrown while trying to encrypt the text. It is as follows:\n{e.Message}";
                return FunqFactory.Fail(message, (string)null);
            }
        }



        /// <summary>
        /// Decrypts the given encrypted text (cipher text) using the given key and key size.
        /// </summary>
        /// <param name="cipherText">The encrypted text to decrypt</param>
        /// <param name="key">The (HEXADECIMAL) key used to encrypt the text and that will be used to decrypt it</param>
        /// <param name="keySize">The size of the key for the creation of the cipher</param>
        public FunqResult<string> Decrypt(string cipherText, string key, AesKeySize keySize)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
            {
                return FunqFactory.Fail("There was no text given to decrypt", (string)null);
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                return FunqFactory.Fail("Could not decrypt the text. The given key was null or empty.", (string)null);
            }

            try
            {
                FunqResult<string> result;
                using (var aesCipher = Aes.Create())
                {
                    result = SetupCipher(aesCipher, key, keySize)
                        .Then(aes =>
                        {
                            var splitCipher = cipherText.Split('_');
                            if (splitCipher.Length != 2)
                            {
                                return FunqFactory.Fail("The given cipher text was not in the correct encrypted format for decryption",
                                                        (string)null);
                            }
                            var initVector = splitCipher[0];
                            var encryptedString = splitCipher[1];
                            if (initVector.Length % 2 != 0 || encryptedString.Length % 2 != 0)
                            {
                                return FunqFactory.Fail("The given cipher text was not in the correct encrypted format for decryption",
                                                        (string)null);
                            }
                            aes.IV = initVector.ToHexBytes();
                            var cryptoTransform = aes.CreateDecryptor();
                            var cipherBytes = encryptedString.ToHexBytes();
                            if (cipherBytes == null)
                            {
                                return FunqFactory.Fail(
                                    "The encrypted string could not be converted into a Hexadecimal Byte Array and therefore could not be decrypted.",
                                    (string)null);
                            }
                            var resultTextBytes = cryptoTransform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                            var resultText = resultTextBytes.ToUTF8String();

                            return resultText == null
                                ? FunqFactory.Fail("Could not convert the decrypted bytes into a string.", (string)null)
                                : FunqFactory.Ok<string>(resultText);
                        });
                }
                return result;
            }
            catch (Exception e)
            {
                var message = $"An exception was thrown while trying to decrypt the text. It is as follows:\n{e.Message}";
                return FunqFactory.Fail(message, (string)null);
            }
        }
    }


    public interface ISymmetricEncryptionSvc
    {
        /// <summary>
        /// Sets up all the parameters and properties for the AES cipher used in the encryption.
        /// </summary>
        /// <param name="cipher">The already created AES cipher. This should be created within a using statement and then handed off to this method.</param>
        /// <param name="key">The (HEXADECIMAL) key used for encryption inside the cipher</param>
        /// <param name="keySize">Key size used for setting up the cipher</param>
        FunqResult<Aes> SetupCipher(Aes cipher, string key, AesKeySize keySize);


        /// <summary>
        /// Encrypts the given text using the given key and key size for cipher creation.
        /// </summary>
        /// <param name="textForEncryption">Text to encrypt</param>
        /// <param name="key">The (HEXADECIMAL) encryption key to use for creating the cipher</param>
        /// <param name="keySize">Size of the key used for creating the cipher</param>
        FunqResult<string> Encrypt(string textForEncryption, string key, AesKeySize keySize);


        /// <summary>
        /// Decrypts the given encrypted text (cipher text) using the given key and key size.
        /// </summary>
        /// <param name="cipherText">The encrypted text to decrypt</param>
        /// <param name="key">The (HEXADECIMAL) key used to encrypt the text and that will be used to decrypt it</param>
        /// <param name="keySize">The size of the key for the creation of the cipher</param>
        FunqResult<string> Decrypt(string cipherText, string key, AesKeySize keySize);
    }
}
