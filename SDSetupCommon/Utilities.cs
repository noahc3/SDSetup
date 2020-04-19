using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SDSetupCommon {
    public class Utilities {
        public static Guid CreateCryptographicallySecureGuid() {
            using (var provider = RandomNumberGenerator.Create()) {
                var bytes = new byte[16];
                provider.GetBytes(bytes);

                return new Guid(bytes);
            }
        }

        public static Guid CreateGuid() {
            return Guid.NewGuid();
        }
    }
}
