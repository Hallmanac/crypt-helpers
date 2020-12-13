using System;

using Hallmanac.CryptoHelpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Hallmanac.CryptoHelpers_Net45.Tests
{
    [TestClass]
    public class RandomGeneratorsTests
    {
        [TestMethod]
        public void Should_Generate_Random_Bytes()
        {
            var rg = new RandomGenerators();
            var bytesContainer = new byte[32];
            rg.GenerateRandomBytes(bytesContainer);

            Assert.IsTrue(bytesContainer.Length == 32);
        }


        [TestMethod]
        public void Should_Generate_Random_Hexadecimal_String_From_Byte_Array_Length()
        {
            var rg = new RandomGenerators();
            const int byteLength = 4;
            var hexString = rg.GenerateHexKeyFromByteLength(byteLength);

            Assert.IsNotNull(hexString);
            Assert.IsTrue(hexString.Length == byteLength * 2);
        }


        [TestMethod]
        public void Should_Generate_Random_Base64_String_From_Byte_Array_Length()
        {
            var rg = new RandomGenerators();
            const int byteLength = 32;
            var base64String = rg.GenerateBase64KeyFromByteLength(byteLength);
            bool isBase64;
            byte[] fromBase64String;
            try
            {
                fromBase64String = Convert.FromBase64String(base64String);
                isBase64 = true;
            }
            catch (Exception)
            {
                isBase64 = false;
                fromBase64String = null;
            }

            Assert.IsNotNull(base64String);
            Assert.IsTrue(isBase64);
            Assert.IsNotNull(fromBase64String);
        }


        [TestMethod]
        public void Should_Generate_Random_64_Bit_Number()
        {
            var rg = new RandomGenerators();
            var number64Bit = rg.GenerateRandom64BitNumberFromByteLength(32);

            Assert.IsTrue(number64Bit > 0);
            Assert.IsTrue(number64Bit.ToString().Length < 20);
        }


        [TestMethod]
        public void Should_Generate_Random_32_Bit_Number()
        {
            var rg = new RandomGenerators();
            var number32Bit = rg.GenerateRandom32BitNumberFromByteLength(16);

            Assert.IsTrue(number32Bit > 0);
            Assert.IsTrue(number32Bit.ToString().Length < 11);
        }


        [TestMethod]
        public void Should_Generate_512Bit_Hexadecimal_String()
        {
            var rg = new RandomGenerators();
            var str = rg.Generate512BitKey();

            Assert.IsNotNull(str);
            Assert.IsTrue(str.Length == 128);
        }


        [TestMethod]
        public void Should_Generate_256Bit_Hexadecimal_String()
        {
            var rg = new RandomGenerators();
            var str = rg.Generate256BitKey();

            Assert.IsNotNull(str);
            Assert.IsTrue(str.Length == 64);
        }


        [TestMethod]
        public void Should_Generate_192Bit_Hexadecimal_String()
        {
            var rg = new RandomGenerators();
            var str = rg.Generate192BitKey();

            Assert.IsNotNull(str);
            Assert.IsTrue(str.Length == 48);

        }

        
        [TestMethod]
        public void Should_Generate_128Bit_Hexadecimal_String()
        {
            var rg = new RandomGenerators();
            var str = rg.Generate128BitKey();

            Assert.IsNotNull(str);
            Assert.IsTrue(str.Length == 32);
        }
    }
}
