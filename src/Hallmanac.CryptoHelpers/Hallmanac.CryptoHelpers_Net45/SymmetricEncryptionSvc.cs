﻿using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Hallmanac.CryptoHelpers
{
    /// <summary>
    ///     Sizes of keys used when doing encryption with AES standards
    /// </summary>
    public enum AesKeySize
    {
        Size256 = 256,
        Size128 = 128,
        Size192 = 192
    }

    /// <summary>
    ///     Block sizes used when creating custom RijndaelManaged encryption ciphers
    /// </summary>
    public enum BlockSize
    {
        Size256 = 256,
        Size128 = 128,
        Size192 = 192
    }


    /// <summary>
    ///     A service that provides helper methods to encrypt and decrypt text using symmetric encryption techniques.
    /// </summary>
    public class SymmetricEncryptionSvc : ISymmetricEncryptionSvc
    {
        /// <summary>
        ///     Sets up all the parameters and properties for the AES cipher used in the encryption.
        /// </summary>
        /// <param name="cipher">
        ///     The already created AES cipher. This should be created within a using statement and then handed
        ///     off to this method.
        /// </param>
        /// <param name="key">The (HEXADECIMAL) key used for encryption inside the cipher</param>
        /// <param name="keySize">Key size used for setting up the cipher</param>
        public CommandResult<Aes> SetupCipher(Aes cipher, string key, AesKeySize keySize)
        {
            if (cipher == null) return CommandResultFactory.Fail("The given AES cipher object was null", (Aes) null);
            if (String.IsNullOrWhiteSpace(key)) return CommandResultFactory.Fail("The cipher creation key for the encryption was null or empty", (Aes) null);

            byte[] keyBytes;
            try
            {
                keyBytes = key.ToHexBytes();
                if (keyBytes == null || keyBytes.Length != (int) keySize / 8)
                    return CommandResultFactory.Fail($"The cipher creation key for the encryption did not match the specified key size of {keySize}",
                        (Aes) null);
            }
            catch (FormatException)
            {
                return CommandResultFactory.Fail("The key was malformed and therefore threw a Format Exception when converting to a byte array.", (Aes) null);
            }
            catch (Exception e)
            {
                return CommandResultFactory.Fail($"There was an exception thrown while trying to create the cipher. It is as follows:\n\t{e.Message}", (Aes) null);
            }

            cipher.KeySize = (int) keySize;
            cipher.BlockSize = 128; // This is the default block size for AES encryption and apparently should not be changed according to what I'm reading
            cipher.Padding = PaddingMode.PKCS7;
            cipher.Mode = CipherMode.CBC;
            cipher.Key = keyBytes;
            cipher.GenerateIV();
            return CommandResultFactory.Ok(cipher);
        }


        /// <summary>
        ///     Encrypts the given text using the given key and key size for cipher creation.
        /// </summary>
        /// <param name="textForEncryption">Text to encrypt</param>
        /// <param name="key">The (HEXADECIMAL) encryption key to use for creating the cipher</param>
        /// <param name="keySize">Size of the key used for creating the cipher</param>
        public CommandResult<string> Encrypt(string textForEncryption, string key, AesKeySize keySize)
        {
            if (String.IsNullOrWhiteSpace(textForEncryption)) return CommandResultFactory.Fail("There was nothing to encrypt", (string) null);
            if (String.IsNullOrWhiteSpace(key)) return CommandResultFactory.Fail("The given encryption key was null or empty", textForEncryption);

            try
            {
                CommandResult<string> result;
                using(var aesCipher = Aes.Create())
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
                            return CommandResultFactory.Ok<string>(textResult);
                        });
                }

                return result;
            }
            catch (Exception e)
            {
                var message = $"An exception was thrown while trying to encrypt the text. It is as follows:\n{e.Message}";
                return CommandResultFactory.Fail(message, (string) null);
            }
        }


        /// <summary>
        ///     Decrypts the given encrypted text (cipher text) using the given key and key size.
        /// </summary>
        /// <param name="cipherText">The encrypted text to decrypt</param>
        /// <param name="key">The (HEXADECIMAL) key used to encrypt the text and that will be used to decrypt it</param>
        /// <param name="keySize">The size of the key for the creation of the cipher</param>
        public CommandResult<string> Decrypt(string cipherText, string key, AesKeySize keySize)
        {
            if (String.IsNullOrWhiteSpace(cipherText)) return CommandResultFactory.Fail("There was no text given to decrypt", (string) null);
            if (String.IsNullOrWhiteSpace(key)) return CommandResultFactory.Fail("Could not decrypt the text. The given key was null or empty.", (string) null);

            try
            {
                CommandResult<string> result;
                using(var aesCipher = Aes.Create())
                {
                    result = SetupCipher(aesCipher, key, keySize)
                        .Then(aes =>
                        {
                            var splitCipher = cipherText.Split('_');
                            if (splitCipher.Length != 2)
                                return CommandResultFactory.Fail("The given cipher text was not in the correct encrypted format for decryption",
                                    (string) null);
                            var initVector = splitCipher[0];
                            var encryptedString = splitCipher[1];
                            if (initVector.Length % 2 != 0 || encryptedString.Length % 2 != 0)
                                return CommandResultFactory.Fail("The given cipher text was not in the correct encrypted format for decryption",
                                    (string) null);
                            aes.IV = initVector.ToHexBytes();
                            var cryptoTransform = aes.CreateDecryptor();
                            var cipherBytes = encryptedString.ToHexBytes();
                            if (cipherBytes == null)
                                return CommandResultFactory.Fail(
                                    "The encrypted string could not be converted into a Hexadecimal Byte Array and therefore could not be decrypted.",
                                    (string) null);
                            var resultTextBytes = cryptoTransform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                            var resultText = resultTextBytes.ToUTF8String();

                            return resultText == null
                                ? CommandResultFactory.Fail("Could not convert the decrypted bytes into a string.", (string) null)
                                : CommandResultFactory.Ok<string>(resultText);
                        });
                }

                return result;
            }
            catch (Exception e)
            {
                var message = $"An exception was thrown while trying to decrypt the text. It is as follows:\n{e.Message}";
                return CommandResultFactory.Fail(message, (string) null);
            }
        }


        /// <summary>
        ///     Creates a RijndaelManaged cipher based on the given key material and the given block and key size.
        /// </summary>
        /// <param name="key">The (HEXADECIMAL) key used during the creation of the cipher for encryption</param>
        /// <param name="blockSize">Block size of the cipher</param>
        /// <param name="keySize">Key Size of the cipher</param>
        public CommandResult<RijndaelManaged> CreateCustomCipher(string key, BlockSize blockSize, AesKeySize keySize)
        {
            if (String.IsNullOrWhiteSpace(key)) return CommandResultFactory.Fail("There was no key provided for the cipher", (RijndaelManaged) null);

            byte[] byteKey;
            try
            {
                byteKey = key.ToHexBytes();
                if (byteKey == null || byteKey.Length != (int) keySize / 8) return CommandResultFactory.Fail($"The cipher creation key for the encryption did not match the specified key size of {keySize}", (RijndaelManaged) null);
            }
            catch (FormatException)
            {
                return CommandResultFactory.Fail("The key was malformed and therefore threw a Format Exception when converting to a byte array.", (RijndaelManaged) null);
            }
            catch (Exception e)
            {
                return CommandResultFactory.Fail($"There was an exception thrown while trying to create the cipher. It is as follows:\n\t{e.Message}", (RijndaelManaged) null);
            }

            var cipher = new RijndaelManaged
            {
                KeySize = (int) keySize,
                BlockSize = (int) blockSize,
                Padding = PaddingMode.ISO10126,
                Mode = CipherMode.CBC,
                Key = byteKey
            };
            return CommandResultFactory.Ok(cipher);
        }


        /// <summary>
        ///     Encrypts a string using the given key. To Decrypt you will need the proper initialization vector that gets randomly
        ///     generated for each encryption process (i.e. different every time the encryption is run). This will happen
        ///     automatically in
        ///     our Decrypt method on this class because we're prefixing those initialization vectors with the encrypted text.
        /// </summary>
        /// <param name="textForEncryption">Text value to be encrypted</param>
        /// <param name="key">
        ///     MUST be a hexadecimal string that is at least 16 bytes in length. Should be more like 32 bytes in
        ///     length
        /// </param>
        /// <param name="keySize">Size of the key used in creating the cipher</param>
        /// <param name="blockSize">Size of the block used in the creation of the Rijndael Managed cipher</param>
        public CommandResult<string> CustomEncrypt(string textForEncryption, string key, AesKeySize keySize, BlockSize blockSize)
        {
            if (String.IsNullOrWhiteSpace(textForEncryption)) return CommandResultFactory.Fail("There was nothing to encrypt", (string) null);
            if (String.IsNullOrWhiteSpace(key)) return CommandResultFactory.Fail("The given encryption key was null or empty", textForEncryption);

            try
            {
                var encryptResult = CreateCustomCipher(key, blockSize, keySize)
                    .Then(cipher =>
                    {
                        var initVector = cipher.IV.ToHexString();

                        // Create the encryptor, convert to bytes, and encrypt the plainText string
                        var cryptoTransform = cipher.CreateEncryptor();
                        var plainTextBytes = textForEncryption.ToUtf8Bytes();
                        var cipherTextBytes = cryptoTransform.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

                        // Get the Hexadecimal string of the cipherTextBytes, hash it, and prefix the Initialization Vector to it.
                        // We're using a hexadecimal string so that the cipherText can be used in URL's. Yes, there are other ways of doing that, but it's a style
                        // choice.
                        var cipherText = cipherTextBytes.ToHexString();
                        var encryptedText = initVector + "_" + cipherText;
                        return CommandResultFactory.Ok<string>(encryptedText);
                    });
                return encryptResult;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return CommandResultFactory.Fail($"An exception was thrown while attempting to encrypt the given text. It is as follows:\n\t{e.Message}", (string) null);
            }
        }


        /// <summary>
        ///     Decrypts the cipher text by using the given key material, key size, and block size.
        /// </summary>
        /// <param name="cipherText">Encrypted text for decryption</param>
        /// <param name="key">The (HEXADECIMAL) key used for decryption</param>
        /// <param name="keySize">size of key used in the cipher creation</param>
        /// <param name="blockSize">block size used in the creation of the cipher</param>
        /// <returns></returns>
        public CommandResult<string> CustomDecrypt(string cipherText, string key, AesKeySize keySize, BlockSize blockSize)
        {
            if (String.IsNullOrWhiteSpace(cipherText)) return CommandResultFactory.Fail("There was nothing to encrypt", (string) null);
            if (String.IsNullOrWhiteSpace(key)) return CommandResultFactory.Fail("The given encryption key was null or empty", cipherText);

            try
            {
                var result = CreateCustomCipher(key, blockSize, keySize)
                    .Then(cipher =>
                    {
                        var splitCipher = cipherText.Split('_');
                        if (splitCipher.Length != 2)
                            return CommandResultFactory.Fail("The given cipher text was not in the correct encrypted format for decryption",
                                (string) null);
                        var initVector = splitCipher[0];
                        var encryptedString = splitCipher[1];
                        if (initVector.Length % 2 != 0 || encryptedString.Length % 2 != 0)
                            return CommandResultFactory.Fail("The given cipher text was not in the correct encrypted format for decryption",
                                (string) null);
                        cipher.IV = initVector.ToHexBytes();
                        var cryptoTransform = cipher.CreateDecryptor();
                        var cipherBytes = encryptedString.ToHexBytes();
                        if (cipherBytes == null)
                            return CommandResultFactory.Fail(
                                "The encrypted string could not be converted into a Hexadecimal Byte Array and therefore could not be decrypted.",
                                (string) null);
                        var resultTextBytes = cryptoTransform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        var resultText = resultTextBytes.ToUTF8String();

                        return resultText == null
                            ? CommandResultFactory.Fail("Could not convert the decrypted bytes into a string.", (string) null)
                            : CommandResultFactory.Ok<string>(resultText);
                    });
                return result;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return CommandResultFactory.Fail($"An exception was thrown while attempting to decrypt the given text. It is as follows:\n\t{e.Message}", (string) null);
            }
        }


        /// <summary>
        ///     Checks the given text with the given key to see whether or not it's encrypted using any of the
        ///     encryption combinations
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        public bool IsEncrypted(string text, string key)
        {
            if (String.IsNullOrWhiteSpace(text) || String.IsNullOrWhiteSpace(key))
                return false;

            // AES encryption
            var keySize128 = Decrypt(text, key, AesKeySize.Size128);
            var keySize192 = Decrypt(text, key, AesKeySize.Size192);
            var keySize256 = Decrypt(text, key, AesKeySize.Size256);
            if (keySize128.IsSuccessful || keySize192.IsSuccessful || keySize256.IsSuccessful)
                return true;

            // RijndaelManaged encryption
            var key128Block128 = CustomDecrypt(text, key, AesKeySize.Size128, BlockSize.Size128);
            var key192Block128 = CustomDecrypt(text, key, AesKeySize.Size192, BlockSize.Size128);
            var key256Block128 = CustomDecrypt(text, key, AesKeySize.Size256, BlockSize.Size128);
            if (key128Block128.IsSuccessful || key192Block128.IsSuccessful || key256Block128.IsSuccessful)
                return true;

            var key128Block192 = CustomDecrypt(text, key, AesKeySize.Size128, BlockSize.Size192);
            var key192Block192 = CustomDecrypt(text, key, AesKeySize.Size192, BlockSize.Size192);
            var key256Block192 = CustomDecrypt(text, key, AesKeySize.Size256, BlockSize.Size192);
            if (key128Block192.IsSuccessful || key192Block192.IsSuccessful || key256Block192.IsSuccessful)
                return true;

            var key128Block256 = CustomDecrypt(text, key, AesKeySize.Size128, BlockSize.Size256);
            var key192Block256 = CustomDecrypt(text, key, AesKeySize.Size192, BlockSize.Size256);
            var key256Block256 = CustomDecrypt(text, key, AesKeySize.Size256, BlockSize.Size256);
            if (key128Block256.IsSuccessful || key192Block256.IsSuccessful || key256Block256.IsSuccessful)
                return true;
            return false;
        }
    }


    /// <summary>
    ///     A service that provides helper methods to encrypt and decrypt text using symmetric encryption techniques.
    /// </summary>
    public interface ISymmetricEncryptionSvc
    {
        /// <summary>
        ///     Sets up all the parameters and properties for the AES cipher used in the encryption.
        /// </summary>
        /// <param name="cipher">
        ///     The already created AES cipher. This should be created within a using statement and then handed
        ///     off to this method.
        /// </param>
        /// <param name="key">The (HEXADECIMAL) key used for encryption inside the cipher</param>
        /// <param name="keySize">Key size used for setting up the cipher</param>
        CommandResult<Aes> SetupCipher(Aes cipher, string key, AesKeySize keySize);

        /// <summary>
        ///     Encrypts the given text using the given key and key size for cipher creation.
        /// </summary>
        /// <param name="textForEncryption">Text to encrypt</param>
        /// <param name="key">The (HEXADECIMAL) encryption key to use for creating the cipher</param>
        /// <param name="keySize">Size of the key used for creating the cipher</param>
        CommandResult<string> Encrypt(string textForEncryption, string key, AesKeySize keySize);

        /// <summary>
        ///     Decrypts the given encrypted text (cipher text) using the given key and key size.
        /// </summary>
        /// <param name="cipherText">The encrypted text to decrypt</param>
        /// <param name="key">The (HEXADECIMAL) key used to encrypt the text and that will be used to decrypt it</param>
        /// <param name="keySize">The size of the key for the creation of the cipher</param>
        CommandResult<string> Decrypt(string cipherText, string key, AesKeySize keySize);


        /// <summary>
        ///     Creates a RijndaelManaged cipher based on the given key material and the given block and key size.
        /// </summary>
        /// <param name="key">The (HEXADECIMAL) key used during the creation of the cipher for encryption</param>
        /// <param name="blockSize">Block size of the cipher</param>
        /// <param name="keySize">Key Size of the cipher</param>
        CommandResult<RijndaelManaged> CreateCustomCipher(string key, BlockSize blockSize, AesKeySize keySize);


        /// <summary>
        ///     Encrypts a string using the given key. To Decrypt you will need the proper initialization vector that gets randomly
        ///     generated for each encryption process (i.e. different every time the encryption is run). This will happen
        ///     automatically in
        ///     our Decrypt method on this class because we're prefixing those initialization vectors with the encrypted text.
        /// </summary>
        /// <param name="textForEncryption">Text value to be encrypted</param>
        /// <param name="key">
        ///     MUST be a hexadecimal string that is at least 16 bytes in length. Should be more like 32 bytes in
        ///     length
        /// </param>
        /// <param name="keySize">Size of the key used in creating the cipher</param>
        /// <param name="blockSize">Size of the block used in the creation of the Rijndael Managed cipher</param>
        CommandResult<string> CustomEncrypt(string textForEncryption, string key, AesKeySize keySize, BlockSize blockSize);


        /// <summary>
        ///     Decrypts the cipher text by using the given key material, key size, and block size.
        /// </summary>
        /// <param name="cipherText">Encrypted text for decryption</param>
        /// <param name="key">The (HEXADECIMAL) key used for decryption</param>
        /// <param name="keySize">size of key used in the cipher creation</param>
        /// <param name="blockSize">block size used in the creation of the cipher</param>
        /// <returns></returns>
        CommandResult<string> CustomDecrypt(string cipherText, string key, AesKeySize keySize, BlockSize blockSize);


        /// <summary>
        ///     Checks the given text with the given key to see whether or not it's encrypted using any of the
        ///     encryption combinations
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        bool IsEncrypted(string text, string key);
    }
}