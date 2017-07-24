﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Funqy.CSharp;


namespace Hallmanac.CryptoHelpers
{
    public enum AesKeySize
    {
        Size256 = 256,
        Size128 = 128,
        Size192 = 192
    }

    public class SymmetricEncryptionSvc
    {
        public FunqResult<Aes> SetupCipher(Aes cipher, string key, AesKeySize keySize)
        {
            if (cipher == null)
            {
                return FunqFactory.Fail("The given AES cipher object was null", (Aes) null);
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                return FunqFactory.Fail("The cipher creation key for the encryption was null or empty", (Aes)null);
            }

            var keyBytes = key.ToHexBytes();
            if (keyBytes == null || keyBytes.Length != (int)keySize)
            {
                return FunqFactory.Fail($"The cipher creation key for the encryption did not match the specified key size of {keySize}", (Aes)null);
            }

            cipher.KeySize = (int)keySize;
            cipher.BlockSize = 128; // This is the default block size for AES encryption and apparently should not be changed according to what I'm reading
            cipher.Padding = PaddingMode.PKCS7;
            cipher.Mode = CipherMode.CBC;
            cipher.Key = keyBytes;
            cipher.GenerateIV();
            return FunqFactory.Ok(cipher);
        }


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
    }
}