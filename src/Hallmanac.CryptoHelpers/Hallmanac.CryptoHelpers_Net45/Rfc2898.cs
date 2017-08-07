/*
 * This class was brought in originally from https://github.com/Thashiznets/PWDTK.NET with some minor formatting modifications and 
 * the removal of a public static method. 
 */

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace Hallmanac.CryptoHelpers
{
    /// <summary>
    /// Implementation of the Rfc2898 PBKDF2 specification located here http://www.ietf.org/rfc/rfc2898.txt using HMACSHA512 as the underlying PRF
    /// Made by thashiznets@yahoo.com.au
    /// v1.0.0.0
    /// </summary>
    public class Rfc2898
    {
        //I made the variable names match the definition in RFC2898 - PBKDF2 where possible, so you can trace the code functionality back to the specification
        private readonly HMACSHA512 _hmacsha512Obj;
        private readonly Int32 _hLen;
        private readonly Byte[] _p;
        private readonly Byte[] _s;
        private readonly Int32 _c;
        private Int32 _dkLen;

        //Minimum rcommended itereations in Rfc2898
        private const int CMinIterations = 1000;
        //Minimum recommended salt length in Rfc2898
        private const int CMinSaltLength = 8;

        /// <summary>
        /// Rfc2898 constructor to create Rfc2898 object ready to perform Rfc2898 functionality
        /// </summary>
        /// <param name="password">The Password to be hashed and is also the HMAC key</param>
        /// <param name="salt">Salt to be concatenated with the password</param>
        /// <param name="iterations">Number of iterations to perform HMACSHA Hashing for PBKDF2</param>
        public Rfc2898(Byte[] password, Byte[] salt, Int32 iterations)
        {
            if (iterations < CMinIterations)
            {
                throw new IterationsLessThanRecommended();
            }

            if (salt.Length < CMinSaltLength)
            {
                throw new SaltLessThanRecommended();
            }

            _hmacsha512Obj = new HMACSHA512(password);
            _hLen = _hmacsha512Obj.HashSize / 8;
            _p = password;
            _s = salt;
            _c = iterations;
        }

        /// <summary>
        /// Rfc2898 constructor to create Rfc2898 object ready to perform Rfc2898 functionality
        /// </summary>
        /// <param name="password">The Password to be hashed and is also the HMAC key</param>
        /// <param name="salt">Salt to be concatenated with the password</param>
        /// <param name="iterations">Number of iterations to perform HMACSHA Hashing for PBKDF2</param>
        public Rfc2898(String password, Byte[] salt, Int32 iterations) : this(new UTF8Encoding(false).GetBytes(password), salt, iterations)
        {

        }

        /// <summary>
        /// Rfc2898 constructor to create Rfc2898 object ready to perform Rfc2898 functionality
        /// </summary>
        /// <param name="password">The Password to be hashed and is also the HMAC key</param>
        /// <param name="salt">Salt to be concatenated with the password</param>
        /// <param name="iterations">Number of iterations to perform HMACSHA Hashing for PBKDF2</param>
        public Rfc2898(String password, String salt, Int32 iterations) : this(new UTF8Encoding(false).GetBytes(password), new UTF8Encoding(false).GetBytes(salt), iterations)
        {

        }


        /// <summary>
        /// Derive Key Bytes using PBKDF2 specification listed in Rfc2898 and HMACSHA512 as the underlying PRF (Psuedo Random Function)
        /// </summary>
        /// <param name="keyLength">Length in Bytes of Derived Key</param>
        /// <returns>Derived Key</returns>
        public Byte[] GetDerivedKeyBytes_PBKDF2_HMACSHA512(int keyLength)
        {
            //no need to throw exception for dkLen too long as per spec because dkLen cannot be larger than Int32.MaxValue so not worth the overhead to check
            _dkLen = keyLength;

            var l = Math.Ceiling((Double)_dkLen / _hLen);

            var finalBlock = new Byte[0];

            for (var i = 1; i <= l; i++)
            {
                //Concatenate each block from F into the final block (T_1..T_l)
                finalBlock = PMergeByteArrays(finalBlock, F(_p, _s, _c, i));
            }

            //returning DK note r not used as dkLen bytes of the final concatenated block returned rather than <0...r-1> substring of final intermediate block + prior blocks as per spec
            return finalBlock.Take(_dkLen).ToArray();

        }

        /// <summary>
        /// Main Function F as defined in Rfc2898 PBKDF2 spec
        /// </summary>
        private Byte[] F(Byte[] p, Byte[] s, Int32 c, Int32 i)
        {

            //Salt and Block number Int(i) concatenated as per spec
            var si = PMergeByteArrays(s, Int(i));

            //Initial hash (U_1) using password and salt concatenated with Int(i) as per spec
            var temp = Prf(p, si);

            //Output block filled with initial hash value or U_1 as per spec
            var uC = temp;

            for (var cLoop = 1; cLoop < c; cLoop++)
            {
                //rehashing the password using the previous hash value as salt as per spec
                temp = Prf(p, temp);

                for (var j = 0; j < temp.Length; j++)
                {
                    //xor each byte of the each hash block with each byte of the output block as per spec
                    uC[j] ^= temp[j];
                }
            }

            //return a T_i block for concatenation to create the final block as per spec
            return uC;
        }

        /// <summary>
        /// PRF function as defined in Rfc2898 PBKDF2 spec
        /// </summary>
        private Byte[] Prf(Byte[] p, Byte[] s)
        {
            //HMACSHA512 Hashing, better than the HMACSHA1 in Microsofts implementation ;)
            return _hmacsha512Obj.ComputeHash(PMergeByteArrays(p, s));
        }

        /// <summary>
        /// This method returns the 4 octet encoded Int32 with most significant bit first as per spec
        /// </summary>
        private static Byte[] Int(Int32 i)
        {
            var I = BitConverter.GetBytes(i);

            //Make sure most significant bit is first
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(I);
            }

            return I;
        }

        /// <summary>
        /// Merge two arrays into a new array
        /// </summary>
        private static Byte[] PMergeByteArrays(Byte[] source1, Byte[] source2)
        {
            //Most efficient way to merge two arrays this according to http://stackoverflow.com/questions/415291/best-way-to-combine-two-or-more-byte-arrays-in-c-sharp
            var buffer = new Byte[source1.Length + source2.Length];
            Buffer.BlockCopy(source1, 0, buffer, 0, source1.Length);
            Buffer.BlockCopy(source2, 0, buffer, source1.Length, source2.Length);

            return buffer;
        }
    }


    public class IterationsLessThanRecommended : Exception
    {
        public IterationsLessThanRecommended() : base("Iteration count is less than the 1000 recommended in Rfc2898")
        {

        }
    }

    public class SaltLessThanRecommended : Exception
    {
        public SaltLessThanRecommended() : base("Salt is less than the 8 byte size recommended in Rfc2898")
        {

        }
    }
}
