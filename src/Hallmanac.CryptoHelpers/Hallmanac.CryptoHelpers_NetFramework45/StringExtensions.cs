using System;
using System.Text;


namespace Hallmanac.CryptoHelpers_Net45
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a hexadecimal string into a byte array. If the string is not formed in a
        /// hexadecimal format then there will be a <see cref="FormatException"/> thrown.
        /// </summary>
        public static byte[] ToHexBytes(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this))
                return null;

            var rootHexString = @this.Replace("-", "");

            var numberOfChars = rootHexString.Length;
            if (numberOfChars % 2 != 0 || numberOfChars < 1)
            {
                return null;
            }
            var byteCount = numberOfChars / 2;
            var bytes = new byte[byteCount];
            for (var i = 0; i < byteCount; i++)
            {
                bytes[i] = Convert.ToByte(rootHexString.Substring(i * 2, 2), 16);
            }
            return bytes;
        }


        /// <summary>
        /// Converts the string into a UTF8 Byte Array
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static byte[] ToUtf8Bytes(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                return null;
            }
            var enc = new UTF8Encoding(false).GetBytes(@this);
            return enc;
        }
    }
}
