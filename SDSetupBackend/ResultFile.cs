using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SDSetupBackend {
    public class ResultFile {
        Timer timeoutTimer;
        string uuid = "";
        string path = "";
        int timeoutMillis = 0;
        List<ActionFileStream> handles = new List<ActionFileStream>();

        public ResultFile(string path, string uuid, int timeoutMillis) {
            this.uuid = uuid;
            this.path = path;
            this.timeoutMillis = timeoutMillis;
            SetTimeout();
        }

        public FileStream GetHandle() {
            timeoutTimer?.Stop();
            ActionFileStream stream = new ActionFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, new Action<ActionFileStream>((str) => { OnHandleClosed(str); }));
            handles.Add(stream);
            return stream;
        }

        public void OnHandleClosed(ActionFileStream stream) {
            if (handles.Contains(stream)) {
                handles.Remove(stream);
                if (handles.Count == 0) {
                    SetTimeout();
                }
            }
        }

        public void SetTimeout() {
            timeoutTimer = new Timer(timeoutMillis);
            timeoutTimer.Elapsed += (object sender, ElapsedEventArgs e) => {
                //HACK: I shouldn't have to do this
                Program.generatedZips.Remove(uuid);

                timeoutTimer.Stop();
                File.Delete(path);
                timeoutTimer.Dispose();
            };
            timeoutTimer.Start();
        }
    }
}
