using System.Text;


namespace System
{
    public static class StringExtensions
    {
        public static byte[] ToHexBytes(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this))
                return null;

            var numberOfChars = @this.Length;
            if (numberOfChars % 2 != 0)
            {
                return null;
            }
            var bytes = new byte[numberOfChars / 2];
            for (int i = 0; i < numberOfChars; i++)
            {
                bytes[i / 2] = Convert.ToByte(@this.Substring(i, 2), 16);
            }
            return bytes;
        }


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
