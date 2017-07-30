using System.Text;


namespace System
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Converts a given byte array into a hexadecimal string.
        /// </summary>
        public static string ToHexString(this byte[] @this)
        {
            if (@this == null)
            {
                return null;
            }
            var hex = BitConverter.ToString(@this);
            var hexString = hex.Replace("-", "");
            return hexString;
        }


        /// <summary>
        /// Converts a given byte array into a UTF8 string by calling the Encoding.UTF8.GetString(textValue) method.
        /// </summary>
        public static string ToUTF8String(this byte[] @this)
        {
            try
            {
                var result = Encoding.UTF8.GetString(@this);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}