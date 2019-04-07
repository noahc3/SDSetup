using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdsetup_backend
{
    public static class Extensions {
        public static string NewLine(this string str, object line) {
            str = str + line.ToString() + "\n";
            return str;
        }
    }
}
