using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SDSetupBlazor {
    public static class biskeytool {
        private readonly static byte[] KeyblobKeySrc = { 0xDF, 0x20, 0x6F, 0x59, 0x44, 0x54, 0xEF, 0xDC, 0x70, 0x74, 0x48, 0x3B, 0x0D, 0xED, 0x9F, 0xD3 };
        private readonly static byte[] ConsoleKeySrc = { 0x4F, 0x02, 0x5F, 0x0E, 0xB6, 0x6D, 0x11, 0x0E, 0xDC, 0x32, 0x7D, 0x41, 0x86, 0xC2, 0xF4, 0x78 };
        private readonly static byte[] RetailAesKekSrc = { 0xE2, 0xD6, 0xB8, 0x7A, 0x11, 0x9C, 0xB8, 0x80, 0xE8, 0x22, 0x88, 0x8A, 0x46, 0xFB, 0xA1, 0x95 };
        private readonly static byte[] AesKekSrc = { 0x4D, 0x87, 0x09, 0x86, 0xC4, 0x5D, 0x20, 0x72, 0x2F, 0xBA, 0x10, 0x53, 0xDA, 0x92, 0xE8, 0xA9 };
        private readonly static byte[] AesKeySrc = { 0x89, 0x61, 0x5E, 0xE0, 0x5C, 0x31, 0xB6, 0x80, 0x5F, 0xE5, 0x8F, 0x3D, 0xA2, 0x4F, 0x7A, 0xA8 };
        private readonly static byte[] BisKekSrc = { 0x34, 0xC1, 0xA0, 0xC4, 0x82, 0x58, 0xF8, 0xB4, 0xFA, 0x9E, 0x5E, 0x6A, 0xDA, 0xFC, 0x7E, 0x4F };
        private readonly static byte[] BisKeySrc0 = { 0xF8, 0x3F, 0x38, 0x6E, 0x2C, 0xD2, 0xCA, 0x32, 0xA8, 0x9A, 0xB9, 0xAA, 0x29, 0xBF, 0xC7, 0x48,
                                                           0x7D, 0x92, 0xB0, 0x3A, 0xA8, 0xBF, 0xDE, 0xE1, 0xA7, 0x4C, 0x3B, 0x6E, 0x35, 0xCB, 0x71, 0x06 };
        private readonly static byte[] BisKeySrc1 = { 0x41, 0x00, 0x30, 0x49, 0xDD, 0xCC, 0xC0, 0x65, 0x64, 0x7A, 0x7E, 0xB4, 0x1E, 0xED, 0x9C, 0x5F,
                                                           0x44, 0x42, 0x4E, 0xDA, 0xB4, 0x9D, 0xFC, 0xD9, 0x87, 0x77, 0x24, 0x9A, 0xDC, 0x9F, 0x7C, 0xA4 };
        private readonly static byte[] BisKeySrc2 = { 0x52, 0xC2, 0xE9, 0xEB, 0x09, 0xE3, 0xEE, 0x29, 0x32, 0xA1, 0x0C, 0x1F, 0xB6, 0xA0, 0x92, 0x6C,
                                                           0x4D, 0x12, 0xE1, 0x4B, 0x2A, 0x47, 0x4C, 0x1C, 0x09, 0xCB, 0x03, 0x59, 0xF0, 0x15, 0xF4, 0xE4 };

        private static string X(this byte[] input) =>
        BitConverter.ToString(input).Replace("-", "").ToUpper();

        private static byte[] B(this string x) {
            int c(char b) => b - (b < 58 ? 48 : 55);

            var arr = new byte[x.Length >> 1];

            for (int i = 0; i < x.Length >> 1; ++i)
                arr[i] = (byte)((c(x[i << 1]) << 4) + (c(x[(i << 1) + 1])));

            return arr;
        }

        private static string GetKeys(byte[] Key, byte Sect) {
            string resp = "";
            resp += String.Format("BIS Key {0} (Crypt): {1}\n", Sect, Key.X().Substring(0, 32));
            resp += String.Format("BIS Key {0} (Tweak): {1}\n", Sect, Key.X().Substring(32));
            return resp;
        }

        private static byte[] EBC(byte[] Data, byte[] Key) {
            Aes crypto = Aes.Create();
            crypto.Mode = CipherMode.ECB;
            crypto.Key = Key;
            crypto.Padding = PaddingMode.None;

            return crypto.CreateDecryptor().TransformFinalBlock(Data, 0, Data.Length);
        }

        public static string deriveBisKeys(string sbk, string tsec) {
            byte[]
            DevKF1 = EBC(KeyblobKeySrc, tsec.B()),
            DevKF2 = EBC(DevKF1, sbk.B()),
            DevKey = EBC(ConsoleKeySrc, DevKF2),
            S00Kek = EBC(RetailAesKekSrc, DevKey),
            S00Key = EBC(BisKeySrc0, S00Kek),
            OSSKF1 = EBC(AesKekSrc, DevKey),
            OSSKF2 = EBC(BisKekSrc, OSSKF1),
            OSSKey = EBC(AesKeySrc, OSSKF2),
            S01Key = EBC(BisKeySrc1, OSSKey),
            S02Key = EBC(BisKeySrc2, OSSKey);
            
            string resp = "";
            resp += GetKeys(S00Key, 0);
            resp += GetKeys(S01Key, 1);
            resp += GetKeys(S02Key, 2);
            resp += GetKeys(S02Key, 3);

            return resp;
        }
    }
}
