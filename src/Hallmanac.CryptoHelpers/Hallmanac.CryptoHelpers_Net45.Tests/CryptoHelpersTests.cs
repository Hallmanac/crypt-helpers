using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoHelpers45 = Hallmanac.CryptoHelpers.CryptoHelpers;


namespace Hallmanac.CryptoHelpers_Net45.Tests
{
    [TestClass]
    public class CryptoHelpersTests
    {
        [TestMethod]
        public void Should_Properly_Hash_Password()
        {
            var crypto = new CryptoHelpers45();
            const string password = "This is my Password!! 1234";
            var hashedPasswordResult = crypto.PasswordHashing.HashPassword(password);

            Assert.IsTrue(hashedPasswordResult.IsSuccessful);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(hashedPasswordResult.Value.HashedPassword));
        }


        [TestMethod]
        public void Comparing_Passwords_Should_Result_In_Equality()
        {
            var crypto = new CryptoHelpers45();
            const string originalPassword = "This is my Password!! 1234";
            var hashedPasswordResult = crypto.PasswordHashing.HashPassword(originalPassword);

            const string comparePassword = "This is my Password!! 1234";
            var comparePasswordResult = crypto.PasswordHashing.ComparePasswords(comparePassword, hashedPasswordResult.Value);

            Assert.IsTrue(comparePasswordResult.IsSuccessful);
        }
    }
}
