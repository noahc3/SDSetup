using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace SDSetupBlazor
{
    public static class G {
        public static string[] autofillBlacklist = new string[] { "cfw", "cfwoptions", "payloads", "pctools", "cfwaddons" };
        public static string[] autodlBlacklist = new string[] { "cfw", "cfwoptions", "cfwaddons" };
        public static Dictionary<string, bool> selectedPackages = new Dictionary<string, bool>();
        public static Dictionary<string, Package> packages = new Dictionary<string, Package>();
        public static Dictionary<string, Dictionary<string, List<Package>>> categories = new Dictionary<string, Dictionary<string, List<Package>>>();

        public static CFWOption reinx_nogc = new CFWOption("Disable Gamecard Slot", "reinx_nogc", false);

        public static bool donotcontinue = false;

        [JSInvokable]
        public static void allowContinue() {
            donotcontinue = false;
        }

        public static CFW[] cfws = new CFW[] {
            new CFW("SX OS", "sxos", false, null),
            new CFW("Atmosphere + Hekate", "atmos", false, null),
            new CFW("ReiNX", "reinx", false, new CFWOption[] {
                reinx_nogc
            }),
        };

        public static void Init() {
            foreach (KeyValuePair<string, Package> k in packages) {
                if (!categories.ContainsKey(k.Value.category)) categories[k.Value.category] = new Dictionary<string, List<Package>>();
                if (!categories[k.Value.category].ContainsKey(k.Value.subcategory)) categories[k.Value.category][k.Value.subcategory] = new List<Package>();
                if (!selectedPackages.ContainsKey(k.Key)) selectedPackages[k.Key] = k.Value.enabledByDefault;
                categories[k.Value.category][k.Value.subcategory].Add(k.Value);
            }
        }
    }
}
