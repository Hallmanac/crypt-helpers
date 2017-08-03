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
        }
    }
}
