using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.FrontendModels {
    public class Message {
        public string Color { get; set; } = "info";
        public string InnerHTML { get; set; } = "Welcome to Homebrew SD Setup!";

        public Message() { }
    }
}
