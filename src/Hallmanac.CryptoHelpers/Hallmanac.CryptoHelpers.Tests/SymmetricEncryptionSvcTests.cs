using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hallmanac.CryptoHelpers.Tests
{
    [TestClass]
    public class SymmetricEncryptionSvcTests
    {
        private const string EncryptionKey256 = "5DBE90D461C035138A6D2ABBEB16087AC6C12A6F8B70696C486E96E617239810";

        [TestMethod]
        public void Should_Encrypt_String()
        {
            var textToEncrypt = "The lazy fox jumped over the brown cow";
            var symEncrypt = new SymmetricEncryptionSvc();

            var expected = "FCDBB7CF7AFCE440899AE9D2949B1110C8289EF602A290467968FFF76EABA94B_4D220909BD9B34192F288931A347B1A0109931F76D7475286CA095A7F80CBF5F42BBAFEDD27ACE52C600AC1D7E061ABE1D46EFCAB6EF75531D4F40B84558082C";
            var result = symEncrypt.Encrypt(textToEncrypt, EncryptionKey256, AesKeySize.Size256);

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(expected, result.Value);
        }
    }
}
