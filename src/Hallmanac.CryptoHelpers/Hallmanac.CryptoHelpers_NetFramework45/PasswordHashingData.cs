namespace Hallmanac.CryptoHelpers_NetFramework45
{
    public class PasswordHashingData
    {
        public int NumberOfIterations { get; set; } = 5000;

        public int SaltSize { get; set; } = 64;

        public string Salt { get; set; }

        public string HashedPassword { get; set; }
    }
}