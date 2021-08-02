using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.BundlerModels {
    public class BundlerProgress {
        public int Progress = 0;
        public int Total = 0;
        public string CurrentTask = "";
        public bool IsComplete = false;
        public bool Success = true;
        public DateTime CompletionTime;

        public BundlerProgress Copy() {
            return new BundlerProgress {
                Progress = this.Progress,
                Total = this.Total,
                CurrentTask = this.CurrentTask,
                IsComplete = this.IsComplete,
                Success = this.Success,
                CompletionTime = this.CompletionTime
            };
        }
    }
}
