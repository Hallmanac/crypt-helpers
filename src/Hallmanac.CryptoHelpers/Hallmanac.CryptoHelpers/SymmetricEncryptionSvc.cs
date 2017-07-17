using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hallmanac.CryptoHelpers
{
    public enum AesKeySize
    {
        Size128 = 128,
        Size192 = 192,
        Size256 = 256
    }

    public class SymmetricEncryptionSvc
    {
        public Aes CreateCipher(string key, AesKeySize keySize)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("The cipher creation key for the encryption was null or empty");
            }

            var keyBytes = key.ToHexBytes();
            if (keyBytes == null || keyBytes.Length != (int)keySize)
            {
                throw new InvalidOperationException($"The cipher creation key for the encryption did not match the specified key size of {keySize}");
            }

            var cipher = Aes.Create();
            cipher.KeySize = (int)keySize;
            cipher.BlockSize = 128;
            cipher.Padding = PaddingMode.PKCS7;
            cipher.Mode = CipherMode.CBC;
            cipher.Key = keyBytes;
            return cipher;
        }
    }
}
