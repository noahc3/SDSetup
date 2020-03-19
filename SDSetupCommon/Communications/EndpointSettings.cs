using SDSetupCommon.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SDSetupCommon.Communications {
    public class EndpointSettings {
        public static ServerInformation serverInformation;
        public static HttpClient HttpClient = new HttpClient();
    }
}
