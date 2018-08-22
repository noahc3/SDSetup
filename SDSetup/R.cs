using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SDSetupManifestGenerator {
    public static class R {
        public static readonly string wd = Path.Combine(Environment.CurrentDirectory, "data\\");
        public static readonly string m = Environment.CurrentDirectory + "\\manifest4.json";
    }
}
