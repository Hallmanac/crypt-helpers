using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Hallmanac.CryptoHelpers_Net45.Tests
{
    [TestClass]
    public class ByteArrayExtensionTests
    {
        private const string EncryptionKey256 = "5DBE90D461C035138A6D2ABBEB16087AC6C12A6F8B70696C486E96E617239810";

        private const string EncryptionKey256WithDashes =
            "5D-BE-90-D4-61-C0-35-13-8A-6D-2A-BB-EB-16-08-7A-C6-C1-2A-6F-8B-70-69-6C-48-6E-96-E6-17-23-98-10";

        private const string TextToEncrypt = "The lazy fox jumped over the brown cow";


        [TestMethod]
        public void Should_Convert_Hexadecimal_Byte_Array_Into_Hexadecimal_String()
        {
            byte[] hexBytes;
            try
            {
                hexBytes = EncryptionKey256.ToHexBytes();
            }
            catch (FormatException)
            {
                hexBytes = null;
            }

            var result = hexBytes.ToHexString();

            Assert.AreEqual(EncryptionKey256, result);
        }


        [TestMethod]
        public void Should_Convert_Hexadecimal_Byte_Array_Into_Hexadecimal_String_And_Include_Dashes()
        {
            byte[] hexBytes;
            try
            {
                hexBytes = EncryptionKey256WithDashes.ToHexBytes();
            }
            catch (FormatException)
            {
                hexBytes = null;
            }

            var result = hexBytes.ToHexString(false);

            Assert.AreEqual(EncryptionKey256WithDashes, result);
        }


        [TestMethod]
        public void Should_Convert_Byte_Array_To_UTF8_String()
        {
            var bytes = TextToEncrypt.ToUtf8Bytes();

            var result = bytes.ToUTF8String();

            Assert.AreEqual(TextToEncrypt, result);
        }
    }
}