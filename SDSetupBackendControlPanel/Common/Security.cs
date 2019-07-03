/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using SDSetupBackendControlPanel.Types;

namespace SDSetupBackendControlPanel.Common {
    class Security {

        private protected static string MasterPassword;

        public static string SHA256Sum(string value) {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create()) {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static string MD5Sum(string value) {
            string output;

            using (MD5 hash = MD5.Create()) {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                output = BitConverter.ToString(result).Replace("-", "");
            }

            return output;
        }

        public static string EncryptData(string data, string password) {

            byte[] key;
            byte[] content = Encoding.UTF8.GetBytes(data);

            using (MD5CryptoServiceProvider hash = new MD5CryptoServiceProvider()) {
                 key = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider()) {
                tdes.Mode = CipherMode.ECB;
                tdes.Key = key;

                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform transform = tdes.CreateEncryptor();

                byte[] result = transform.TransformFinalBlock(content, 0, content.Length);

                tdes.Clear();

                return BitConverter.ToString(result).Replace("-", "");
            }
        }

        public static string DecryptData(string data, string password) {
            byte[] key;
            byte[] content = data.SplitInParts(2).Select(b => Convert.ToByte(b, 16)).ToArray();
            

            using (MD5CryptoServiceProvider hash = new MD5CryptoServiceProvider()) {
                key = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider()) {
                tdes.Key = key;
                tdes.Mode = CipherMode.ECB;

                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform transform = tdes.CreateDecryptor();

                byte[] result = transform.TransformFinalBlock(content, 0, content.Length);

                tdes.Clear();

                return Encoding.UTF8.GetString(result);
            }
        }

        public static Config EncryptSensitiveData(Config conf) {
            conf = new Config(conf);

            if (String.IsNullOrWhiteSpace(MasterPassword)) {

                if (String.IsNullOrWhiteSpace(conf.MasterPasswordHash)) {
                    //there is no master password hash to compare to, therefor the content should not be encrypted. This should never happen!
                    throw new CryptographicUnexpectedOperationException("No master key hash to compare to, therefore correct password cannot be determined.");
                }

                //request MP from user
                GetMasterPasswordFromUser(conf.MasterPasswordHash);
            }

            if (conf.CryptoSanityCheck != Config._CryptoSanityCheck) {
                //the sanity check is not the correct value, meaning the content is already encrypted or the content was modified. This should never happen!
                throw new CryptographicUnexpectedOperationException("Content already encrypted or content corrupted. Sanity check failed with unexpected string: " + conf.CryptoSanityCheck);
            }

            conf.MasterPasswordHash = SHA256Sum(MasterPassword);
            conf.CryptoSanityCheck = EncryptData(conf.CryptoSanityCheck, MasterPassword);

            foreach (ServerConfig k in conf.Servers.Values) {
                if (!k.AskForPasswordEachRun) k.Password = EncryptData(k.Password, MasterPassword);
                if (!k.AskForPrivateKeyPassphraseEachRun) k.PrivateKeyPassphrase = EncryptData(k.PrivateKeyPassphrase, MasterPassword);
                k.Username = EncryptData(k.Username, MasterPassword);
                k.PrivateKeyPath = EncryptData(k.PrivateKeyPath, MasterPassword);
            }

            return conf;
        }

        public static Config DecryptSensitiveData(Config conf) {
            conf = new Config(conf);

            

            if (String.IsNullOrWhiteSpace(MasterPassword)) {

                if (String.IsNullOrWhiteSpace(conf.MasterPasswordHash)) {
                    //there is no master password hash to compare to, therefor the content should not be encrypted. This should never happen!
                    throw new CryptographicUnexpectedOperationException("No master key hash to compare to, therefore correct password cannot be determined.");
                }

                //request MP from user
                GetMasterPasswordFromUser(conf.MasterPasswordHash);
            }

            conf.CryptoSanityCheck = DecryptData(conf.CryptoSanityCheck, MasterPassword);

            if (conf.CryptoSanityCheck != Config._CryptoSanityCheck) {
                //the sanity check did not decrypt to the correct value, meaning the content was already decrypted or the content is corrupted. This should never happen!
                //note: the master password was already verified as correct earlier.
                throw new CryptographicUnexpectedOperationException("Content already decrypted or content corrupted. Sanity check failed with unexpected string: " + conf.CryptoSanityCheck);
            }

            foreach (ServerConfig k in conf.Servers.Values) {
                if (!k.AskForPasswordEachRun) k.Password = DecryptData(k.Password, MasterPassword);
                if (!k.AskForPrivateKeyPassphraseEachRun) k.PrivateKeyPassphrase = DecryptData(k.PrivateKeyPassphrase, MasterPassword);
                k.Username = DecryptData(k.Username, MasterPassword);
                k.PrivateKeyPath = DecryptData(k.PrivateKeyPath, MasterPassword);
            }

            return conf;
        }

        public static void GetMasterPasswordFromUser(string PasswordHash) {
            while (true) {
                GetMasterPassword gmp = new GetMasterPassword(PasswordHash);
                gmp.ShowDialog();
                if (gmp.DialogResult == System.Windows.Forms.DialogResult.OK) {
                    MasterPassword = gmp.MasterPassword;
                    return;
                }
            }
            
        }
    }

    static class StringExtensions {

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength) {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

    }
}
