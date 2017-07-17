using System;


namespace System
{
    public static class ByteArrayExtensions
    {
        ///// <summary>
        /////     Converts a given byte array into a hexadecimal string.
        ///// </summary>
        public static string BytesToHexString(this byte[] @this)
        {
            if (@this == null)
            {
                return null;
            }
            var hex = BitConverter.ToString(@this);
            var hexString = hex.Replace("-", "");
            return hexString;
        }
    }
}
