using Hallmanac.CryptoHelpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Hallmanac.CryptoHelpers_Net45.Tests
{
    [TestClass]
    public class HashingHelpersTests
    {
        private const string TextToHash = "The Cow jumped over the moon in 5 tries while cursing $%*(@ up a storm";

        [TestMethod]
        public void Should_Create_MD5_Hash_For_Value()
        {
            var hh = new HashingHelpers();
            var hashed = hh.ComputeMD5Hash(TextToHash);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(hashed));
            Assert.AreEqual(hashed.Length, 32);
        }


        [TestMethod]
        public void ComputeSha1Hash_Should_Return_Sha1_String()
        {
            var hh = new HashingHelpers();
            var hashed = hh.ComputeSha1Hash(TextToHash);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(hashed));
            Assert.AreEqual(hashed.Length, 40);
        }


        [TestMethod]
        public void ComputeSha256Hash_Should_Return_Sha256_String()
        {
            var hh = new HashingHelpers();
            var hashed = hh.ComputeSha256Hash(TextToHash);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(hashed));
            Assert.AreEqual(hashed.Length, 64);
        }


        [TestMethod]
        public void ComputeSha384Hash_Should_Return_Sha384_String()
        {
            var hh = new HashingHelpers();
            var hashed = hh.ComputeSha384Hash(TextToHash);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(hashed));
            Assert.AreEqual(hashed.Length, 96);
        }


        [TestMethod]
        public void ComputeSha512Hash_Should_Return_Sha512_String()
        {
            var hh = new HashingHelpers();
            var hashed = hh.ComputeSha512Hash(TextToHash);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(hashed));
            Assert.AreEqual(hashed.Length, 128);
        }
    }
}
