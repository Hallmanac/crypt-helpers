﻿using System;
using System.Security.Cryptography;


namespace Hallmanac.CryptoHelpers
{
    /// <summary>
    /// A series of helper methods to generate random data such as random bytes, random 32 bit number, random 64 bit number, etc.
    /// </summary>
    public class RandomGenerators : IRandomGenerators
    {
        /// <summary>
        /// Generates random, non-zero bytes using the <see cref="System.Security.Cryptography.RandomNumberGenerator"/> class. This
        /// method mutates the given byte array. 
        /// </summary>
        /// <param name="data">Byte Array to hold the randomly generated bytes</param>
        public void GenerateRandomBytes(byte[] data)
        {
            if (data == null)
                return;

            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);
        }


        /// <summary>
        /// Generates a random byte array key based on the byte length given and returns it as a hexadecimal string.
        /// </summary>
        /// <param name="byteLength">Length of Byte array used in the random generator</param>
        /// <returns>Hexadecimal text representation of the randomly generated bytes.</returns>
        public string GenerateHexKeyFromByteLength(int byteLength)
        {
            var key = new byte[byteLength];
            GenerateRandomBytes(key);
            return key.ToHexString();
        }


        /// <summary>
        ///   Generates a random byte array key based on the byte length given and returns it as a Base64 encoded string.
        /// </summary>
        /// <param name="byteLength">Length of Byte array used in the random generator</param>
        /// <returns>Base64 encoded text representation of the randomly generated bytes.</returns>
        public string GenerateBase64KeyFromByteLength(int byteLength)
        {
            var key = new byte[byteLength];
            GenerateRandomBytes(key);
            return Convert.ToBase64String(key);
        }


        /// <summary>
        /// Generates a crypto random 64 bit number (does NOT use the .NET random number generator).
        /// </summary>
        /// <param name="byteLength"></param>
        /// <returns>A 19 digit random number</returns>
        public long GenerateRandom64BitNumberFromByteLength(int byteLength)
        {
            try
            {
                if (byteLength < 8)
                {
                    byteLength = 8;
                }
                var key = new byte[byteLength];
                GenerateRandomBytes(key);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(key);
                }
                var number = Math.Abs(BitConverter.ToInt64(key, 0));
                return number;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        /// <summary>
        /// Generates a crypto random 32 bit number (does NOT use the .NET random number generator).
        /// </summary>
        /// <param name="byteLength"></param>
        /// <returns>a ten digit random number</returns>
        public int GenerateRandom32BitNumberFromByteLength(int byteLength)
        {
            try
            {
                if (byteLength < 8)
                {
                    byteLength = 8;
                }
                var key = new byte[byteLength];
                GenerateRandomBytes(key);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(key);
                }
                var number = Math.Abs(BitConverter.ToInt32(key, 0));
                return number;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        /// <summary>
        /// Generates a random 512 bit key (in a byte array) and returns it as a hexadecimal string
        /// </summary>
        /// <returns></returns>
        public string Generate512BitKey()
        {
            var buffer = new byte[64];
            GenerateRandomBytes(buffer);
            return buffer.ToHexString();
        }


        /// <summary>
        ///     Generates a random 256 bit key (in a byte array) and returns it as a hexadecimal string.
        /// </summary>
        /// <returns>A hexadecimal string based on the randomly generated 256 bit key byte array</returns>
        public string Generate256BitKey()
        {
            var buffer = new byte[32];
            GenerateRandomBytes(buffer);
            return buffer.ToHexString();
        }


        /// <summary>
        /// Generates a random 128 bit key (in a byte array) and returns it as a hexadecimal string.
        /// </summary>
        /// <returns>A hexadecimal string based on the randomly generated 128 bit key byte array</returns>
        public string Generate192BitKey()
        {
            var buffer = new byte[24];
            GenerateRandomBytes(buffer);
            return buffer.ToHexString();
        }


        /// <summary>
        /// Generates a random 128 bit key (in a byte array) and returns it as a hexadecimal string.
        /// </summary>
        /// <returns>A hexadecimal string based on the randomly generated 128 bit key byte array</returns>
        public string Generate128BitKey()
        {
            var buffer = new byte[16];
            GenerateRandomBytes(buffer);
            return buffer.ToHexString();
        }
    }


    /// <summary>
    /// A series of helper methods to generate random data such as random bytes, random 32 bit number, random 64 bit number, etc.
    /// </summary>
    public interface IRandomGenerators
    {
        /// <summary>
        /// Generates random, non-zero bytes using the RNGCryptoServiceProvider
        /// </summary>
        /// <param name="data">Byte Array to hold the randomly generated bytes</param>
        void GenerateRandomBytes(byte[] data);


        /// <summary>
        /// Generates a random byte array key based on the byte length given and returns it as a hexadecimal string.
        /// </summary>
        /// <param name="byteLength">Length of Byte array used in the random generator</param>
        /// <returns>Hexadecimal text representation of the randomly generated bytes.</returns>
        string GenerateHexKeyFromByteLength(int byteLength);


        /// <summary>
        ///   Generates a random byte array key based on the byte length given and returns it as a Base64 encoded string.
        /// </summary>
        /// <param name="byteLength">Length of Byte array used in the random generator</param>
        /// <returns>Base64 encoded text representation of the randomly generated bytes.</returns>
        string GenerateBase64KeyFromByteLength(int byteLength);


        /// <summary>
        /// Generates a crypto random 64 bit number (does NOT use the .NET random number generator).
        /// </summary>
        /// <param name="byteLength"></param>
        /// <returns>A 19 digit random number</returns>
        long GenerateRandom64BitNumberFromByteLength(int byteLength);


        /// <summary>
        /// Generates a crypto random 32 bit number (does NOT use the .NET random number generator).
        /// </summary>
        /// <param name="byteLength"></param>
        /// <returns>a ten digit random number</returns>
        int GenerateRandom32BitNumberFromByteLength(int byteLength);


        /// <summary>
        ///     Generates a random 256 bit key (in a byte array) and returns it as a hexadecimal string.
        /// </summary>
        /// <returns>A hexadecimal string based on the randomly generated 256 bit key byte array</returns>
        string Generate256BitKey();


        /// <summary>
        /// Generates a random 128 bit key (in a byte array) and returns it as a hexadecimal string.
        /// </summary>
        /// <returns>A hexadecimal string based on the randomly generated 128 bit key byte array</returns>
        string Generate192BitKey();


        /// <summary>
        /// Generates a random 128 bit key (in a byte array) and returns it as a hexadecimal string.
        /// </summary>
        /// <returns>A hexadecimal string based on the randomly generated 128 bit key byte array</returns>
        string Generate128BitKey();


        /// <summary>
        /// Generates a random 512 bit key (in a byte array) and returns it as a hexadecimal string
        /// </summary>
        /// <returns></returns>
        string Generate512BitKey();
    }
}