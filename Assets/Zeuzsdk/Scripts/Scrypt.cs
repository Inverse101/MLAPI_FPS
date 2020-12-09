//from https://github.com/jclopes/libscrypt

using System;
using System.Collections.Generic;
using System.Linq;

namespace libscrypt
{
    public static class SCrypt
    {
        public static byte[] PBKDF2Sha256GetBytes(byte[] password, byte[] salt, int dklen, int iterationCount)
        {
            //TODO: wait until https://raw.githubusercontent.com/dotnet/corefx/master/src/System.Security.Cryptography.Algorithms/src/System/Security/Cryptography/Rfc2898DeriveBytes.cs
            // has been integrated into mono, then scrap libscrypt
            /* usage: 
                byte[] bytes;
                using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    bytes = deriveBytes.GetBytes(PBKDF2SubkeyLength);
                }

             /*******/

            using (var hmac = new System.Security.Cryptography.HMACSHA256(password))
            {
                int hashLength = hmac.HashSize / 8;
                if ((hmac.HashSize & 7) != 0)
                    hashLength++;
                int keyLength = dklen / hashLength;
                if ((long)dklen > (0xFFFFFFFFL * hashLength) || dklen < 0)
                    throw new ArgumentOutOfRangeException("dklen");
                if (dklen % hashLength != 0)
                    keyLength++;
                byte[] extendedkey = new byte[salt.Length + 4];
                Buffer.BlockCopy(salt, 0, extendedkey, 0, salt.Length);
                using (var ms = new System.IO.MemoryStream())
                {
                    for (int i = 0; i < keyLength; i++)
                    {
                        extendedkey[salt.Length] = (byte)(((i + 1) >> 24) & 0xFF);
                        extendedkey[salt.Length + 1] = (byte)(((i + 1) >> 16) & 0xFF);
                        extendedkey[salt.Length + 2] = (byte)(((i + 1) >> 8) & 0xFF);
                        extendedkey[salt.Length + 3] = (byte)(((i + 1)) & 0xFF);
                        byte[] u = hmac.ComputeHash(extendedkey);
                        Array.Clear(extendedkey, salt.Length, 4);
                        byte[] f = u;
                        for (int j = 1; j < iterationCount; j++)
                        {
                            u = hmac.ComputeHash(u);
                            for (int k = 0; k < f.Length; k++)
                            {
                                f[k] ^= u[k];
                            }
                        }
                        ms.Write(f, 0, f.Length);
                        Array.Clear(u, 0, u.Length);
                        Array.Clear(f, 0, f.Length);
                    }
                    byte[] dk = new byte[dklen];
                    ms.Position = 0;
                    ms.Read(dk, 0, dklen);
                    ms.Position = 0;
                    for (long i = 0; i < ms.Length; i++)
                    {
                        ms.WriteByte(0);
                    }
                    Array.Clear(extendedkey, 0, extendedkey.Length);
                    return dk;
                }
            }
        }
             
        public static void blockmix_salsa8(UInt32[] BY, int Yi, int r, UInt32[] x, UInt32[] _X)
        {
            Array.ConstrainedCopy(BY, (2 * r - 1) * 16, _X, 0, 16);
            for (var i = 0; i < 2 * r; i++)
            {
                blockxor(BY, i * 16, _X, 16);
                salsa20_8(_X, x);
                Array.ConstrainedCopy(_X, 0, BY, Yi + (i * 16), 16);
            }

            for (var i = 0; i < r; i++)
            {
                Array.ConstrainedCopy(BY, Yi + (i * 2) * 16, BY, (i * 16), 16);
            }

            for (var i = 0; i < r; i++)
            {
                Array.ConstrainedCopy(BY, Yi + (i * 2 + 1) * 16, BY, (i + r) * 16, 16);
            }
        }

        private static void salsa20_8(UInt32[] B, UInt32[] x)
        {
            Array.ConstrainedCopy(B, 0, x, 0, 16);

            for (var i = 8; i > 0; i -= 2)
            {
                x[4] ^= R(x[0] + x[12], 7);
                x[8] ^= R(x[4] + x[0], 9);
                x[12] ^= R(x[8] + x[4], 13);
                x[0] ^= R(x[12] + x[8], 18);
                x[9] ^= R(x[5] + x[1], 7);
                x[13] ^= R(x[9] + x[5], 9);
                x[1] ^= R(x[13] + x[9], 13);
                x[5] ^= R(x[1] + x[13], 18);
                x[14] ^= R(x[10] + x[6], 7);
                x[2] ^= R(x[14] + x[10], 9);
                x[6] ^= R(x[2] + x[14], 13);
                x[10] ^= R(x[6] + x[2], 18);
                x[3] ^= R(x[15] + x[11], 7);
                x[7] ^= R(x[3] + x[15], 9);
                x[11] ^= R(x[7] + x[3], 13);
                x[15] ^= R(x[11] + x[7], 18);
                x[1] ^= R(x[0] + x[3], 7);
                x[2] ^= R(x[1] + x[0], 9);
                x[3] ^= R(x[2] + x[1], 13);
                x[0] ^= R(x[3] + x[2], 18);
                x[6] ^= R(x[5] + x[4], 7);
                x[7] ^= R(x[6] + x[5], 9);
                x[4] ^= R(x[7] + x[6], 13);
                x[5] ^= R(x[4] + x[7], 18);
                x[11] ^= R(x[10] + x[9], 7);
                x[8] ^= R(x[11] + x[10], 9);
                x[9] ^= R(x[8] + x[11], 13);
                x[10] ^= R(x[9] + x[8], 18);
                x[12] ^= R(x[15] + x[14], 7);
                x[13] ^= R(x[12] + x[15], 9);
                x[14] ^= R(x[13] + x[12], 13);
                x[15] ^= R(x[14] + x[13], 18);
            }

            for (var i = 0; i < 16; ++i)
            {
                B[i] += x[i];
            }
        }

        private static void blockxor(UInt32[] S, int Si, UInt32[] D, int len)
        {
            for (var i = 0; i < len; i++)
            {
                D[i] ^= S[Si + i];
            }
        }

        private static UInt32 R(UInt32 a, int b)
        {
            return (a << b) | (a >> (32 - b));
        }

        // N = Cpu cost, r = Memory cost, p = parallelization cost, dkLen = output key length
        public static byte[] scrypt(byte[] password, byte[] salt, int N, int r, int p, int dkLen)
        {
            var b = PBKDF2Sha256GetBytes(password, salt, p * 128 * r, 1);
            var B = new UInt32[p * 32 * r];

            for (var i = 0; i < B.Length; i++)
            {
                var j = i * 4;
                B[i] = ((b[j + 3] & (UInt32)0xff) << 24) |
                       ((b[j + 2] & (UInt32)0xff) << 16) |
                       ((b[j + 1] & (UInt32)0xff) << 8) |
                       ((b[j + 0] & (UInt32)0xff) << 0);
            }

            var XY = new UInt32[64 * r];
            var V = new UInt32[32 * r * N];

            int Yi = 32 * r;

            // scratch space
            var x = new UInt32[16];       // salsa20_8
            var _X = new UInt32[16];      // blockmix_salsa8

            for (var ii = 0; ii < p; ii++)
            {
                int Bi = ii * 32 * r;

                Array.ConstrainedCopy(B, Bi, XY, 0, Yi);            // ROMix - 1

                for (var i = 0; i < N; i++)
                {                                                   // ROMix - 2
                    Array.ConstrainedCopy(XY, 0, V, i * Yi, Yi);    // ROMix - 3
                    blockmix_salsa8(XY, Yi, r, x, _X);              // ROMix - 4
                }

                for (var i = 0; i < N; i++)
                {                // ROMix - 6
                    var offset = (2 * r - 1) * 16;                  // ROMix - 7
                    var j = XY[offset] & ((UInt32)N - 1);
                    blockxor(V, (int)(j * Yi), XY, Yi);             // ROMix - 8 (inner)
                    blockmix_salsa8(XY, Yi, r, x, _X);              // ROMix - 9 (outer)
                }

                Array.ConstrainedCopy(XY, 0, B, Bi, Yi);            // ROMix - 10
            }
            var bb = new List<byte>();

            for (var i = 0; i < B.Length; i++)
            {
                bb.Add((byte)((B[i] >> 0) & 0xff));
                bb.Add((byte)((B[i] >> 8) & 0xff));
                bb.Add((byte)((B[i] >> 16) & 0xff));
                bb.Add((byte)((B[i] >> 24) & 0xff));
            }

            var derivedKey = PBKDF2Sha256GetBytes(password, bb.ToArray(), dkLen, 1);

            return derivedKey;
        }
    }
}

