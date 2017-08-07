using System.Text;


namespace System
{
    /// <summary>
    /// Extensions for the byte[] type
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Converts a given byte array into a hexadecimal string and removed all dashes by default (unless specified otherwise).
        /// </summary>
        public static string ToHexString(this byte[] @this, bool removeDashes = true)
        {
            if (@this == null)
            {
                return null;
            }
            var hex = BitConverter.ToString(@this);
            var hexString = removeDashes ? hex.Replace("-", "") : hex;
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


        /// <summary>
        /// Converts the current byte array into a Base64 string and returns the value. This is done using
        /// the Convert.ToBase64String method inside a try/catch block. Returns null if unsuccessful.
        /// </summary>
        public static string ToBase64String(this byte[] @this)
        {
            try
            {
                var result = Convert.ToBase64String(@this);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}