using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Hallmanac.CryptoHelpers_Net45.Tests
{
    [TestClass]
    public class PasswordHashingTests
    {
        [TestMethod]
        public void ComputePasswordAndSaltBytes_Should_Generate_Sha512_ByteArray()
        {
            var rg = new RandomGenerators();
            var phs = new PasswordHashingSvc(rg.Generate512BitKey());

            var salt = new byte[32];
            rg.GenerateRandomBytes(salt);
            var rand = new Random();
            var computeResult = phs.ComputePasswordAndSaltBytes(salt, "my password 1234 #*@", rand.Next(8200, 14000));

            Assert.IsTrue(computeResult.IsSuccessful);
            Assert.AreEqual(computeResult.Value.Length, 64);
        }


        [TestMethod]
        public void HashPassword_Should_Return_Success_And_PasswordHashingData_Object()
        {
            var rg = new RandomGenerators();
            var phs = new PasswordHashingSvc(rg.Generate512BitKey());

            const string password = "This is my test password 1234 #*@a";
            var hashed = phs.HashPassword(password);

            Assert.IsTrue(hashed.IsSuccessful);
            Assert.AreEqual(hashed.Value.HashedPassword.Length, 128);
        }


        [TestMethod]
        public void HashPassword_Should_Return_Different_HashedValues_For_The_Same_Value_Computed_Twice()
        {
            var rg = new RandomGenerators();
            var phs = new PasswordHashingSvc(rg.Generate512BitKey());

            const string password = "This is my test password 1234 #*@a";
            var hashedResult1 = phs.HashPassword(password);
            var hashedResult2 = phs.HashPassword(password);

            Assert.AreNotEqual(hashedResult1.Value.HashedPassword, hashedResult2.Value.HashedPassword);
        }
    }
}
