using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Hallmanac.CryptoHelpers_Net45.Tests
{
    [TestClass]
    public class StringExtensionTests
    {
        private const string EncryptionKey256 = "5DBE90D461C035138A6D2ABBEB16087AC6C12A6F8B70696C486E96E617239810";
        private const string EncryptionKey256WithDashes = "5D-BE-90-D4-61-C0-35-13-8A-6D-2A-BB-EB-16-08-7A-C6-C1-2A-6F-8B-70-69-6C-48-6E-96-E6-17-23-98-10";
        private const string TextToEncrypt = "The lazy fox jumped over the brown cow";


        [TestMethod]
        public void Should_Convert_String_To_Hexadecimal_Byte_Array()
        {
            var result = EncryptionKey256.ToHexBytes();

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void ToHexBytes_Should_Throw_Format_Exception_When_Not_Hexadecimal()
        {
            Assert.ThrowsException<FormatException>(() =>
            {
                var result = TextToEncrypt.ToHexBytes();
            });
        }


        [TestMethod]
        public void ToHexBytes_Should_Remove_All_Dashes()
        {
            var result = EncryptionKey256WithDashes.ToHexBytes();

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void ToUtf8Bytes_Should_Convert_String_To_Byte_Array()
        {
            var result = TextToEncrypt.ToUtf8Bytes();

            Assert.IsNotNull(result);
        }
    }
}