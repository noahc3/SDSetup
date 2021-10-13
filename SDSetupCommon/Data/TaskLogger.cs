using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data {
    public class TaskLogger {
        public string Title = "";
        public string BundlerUuid = "";
        public string ClientUuid = "";
        public string LoggerId = "";
        public DateTime StartTime = DateTime.UtcNow;
        public DateTime CompletionTime;
        public List<String> log = new List<String>();
        public bool failed = false;
        public bool success = false;
        public bool complete = false;

        public TaskLogger() {
            this.LoggerId 
                = new DateTimeOffset(StartTime).ToUnixTimeMilliseconds()
                + "-"
                + Utilities.CreateGuid().ToCleanString();
        }

        public TaskLogger(string Title) : this() {
            this.Title = Title;
        }

        public TaskLogger(string Title, string BundlerUuid, string ClientUuid = "") : this() {
            this.Title = Title;
            this.BundlerUuid = BundlerUuid;
            this.ClientUuid = ClientUuid;
        }

        public void LogInfo(string message) {
            Log("INFO", message);
        }

        public void LogWarning(string message) {
            Log("WARN", message);
        }

        public void LogError(string message, bool markFailed = true) {
            Log("FAIL", message);
            if (markFailed) MarkFailed();
        }

        public void MarkFailed() {
            complete = true;
            failed = true;
            CompletionTime = DateTime.UtcNow;
        }

        public void MarkSuccess() {
            complete = true;
            success = true;
            CompletionTime = DateTime.UtcNow;
        }

        public void Log(string tag, string message) {
            log.Add($"[{tag}] {message}");
        }
    }
}
