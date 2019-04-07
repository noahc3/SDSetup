using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace sdsetup_backend
{
    public class DeletingFileStream : FileStream {
        Timer timeoutTimer;
        string uuid;

        public DeletingFileStream(string path, FileMode mode, string uuid) : base(path, mode) {
            this.uuid = uuid;
        }

        public void Timeout(int ms) {
            timeoutTimer = new Timer(ms);
            timeoutTimer.Elapsed += (object sender, ElapsedEventArgs e) => {
                timeoutTimer.Stop();
                timeoutTimer.Dispose();
                //HACK: I shouldn't have to do this
                Program.generatedZips.Remove(uuid);
                this.Dispose();
            };
            timeoutTimer.Start();
        }

        public void StopTimeout() {
            if (timeoutTimer != null) {
                timeoutTimer.Stop();
                timeoutTimer.Dispose();
                timeoutTimer = null;
            }
        }

        public override void Close() {
            base.Close();
            File.Delete(Name);
        }

        ~DeletingFileStream() {
            base.Dispose();
            File.Delete(Name);
        }

        
    }
}
