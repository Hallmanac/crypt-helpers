using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Hallmanac.CryptoHelpers_Net45.Tests
{
    [TestClass]
    public class SymmetricEncryptionSvcTests
    {
        private const string EncryptionKey256 = "5DBE90D461C035138A6D2ABBEB16087AC6C12A6F8B70696C486E96E617239810";
        private const string TextToEncrypt = "The lazy fox jumped over the brown cow";

        [TestMethod]
        public void Should_Encrypt_And_Decrypt_String()
        {
            var symEncrypt = new SymmetricEncryptionSvc();

            var encryptedResult = symEncrypt.Encrypt(TextToEncrypt, EncryptionKey256, AesKeySize.Size256);
            var decryptedResult = symEncrypt.Decrypt(encryptedResult.Value, EncryptionKey256, AesKeySize.Size256);

            Assert.IsTrue(encryptedResult.IsSuccessful);
            Assert.IsTrue(decryptedResult.IsSuccessful);
            Assert.AreEqual(decryptedResult.Value, TextToEncrypt);
            Assert.AreEqual(encryptedResult.Value.Split('_').Length, 2);
        }


        [TestMethod]
        public void Should_Encrypt_And_Decrypt_String_Using_Custom_RijndaelManaged_Method()
        {
            var encryptSvc = new SymmetricEncryptionSvc();
            var encryptedResult = encryptSvc.CustomEncrypt(TextToEncrypt, EncryptionKey256, AesKeySize.Size256, BlockSize.Size256);
            var decryptedResult = encryptSvc.CustomDecrypt(encryptedResult.Value, EncryptionKey256, AesKeySize.Size256, BlockSize.Size256);

            Assert.IsTrue(encryptedResult.IsSuccessful);
            Assert.IsTrue(encryptedResult.IsSuccessful);
            Assert.IsTrue(decryptedResult.IsSuccessful);
            Assert.AreEqual(decryptedResult.Value, TextToEncrypt);
            Assert.AreEqual(encryptedResult.Value.Split('_').Length, 2);
        }


        [TestMethod]
        public void Should_Decrypt_Text_That_Was_Encrypted_By_Legacy_SymEnc_Library()
        {
            var encryptSvc = new SymmetricEncryptionSvc();
            const string preEncryptedText = "FCDBB7CF7AFCE440899AE9D2949B1110C8289EF602A290467968FFF76EABA94B_4D220909BD9B34192F288931A347B1A0109931F76D7475286CA095A7F80CBF5F42BBAFEDD27ACE52C600AC1D7E061ABE1D46EFCAB6EF75531D4F40B84558082C";
            var decryptedPreText = encryptSvc.CustomDecrypt(preEncryptedText, EncryptionKey256, AesKeySize.Size256, BlockSize.Size256);

            Assert.IsTrue(decryptedPreText.IsSuccessful);
            Assert.AreEqual(decryptedPreText.Value, TextToEncrypt);
        }
    }
}