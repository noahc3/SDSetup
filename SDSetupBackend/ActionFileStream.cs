/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace SDSetupBackend
{
    public class ActionFileStream : FileStream {

        private Action<ActionFileStream> OnClosed;

        public ActionFileStream(string path, FileMode mode, FileAccess access, FileShare share, Action<ActionFileStream> onClosed) : base(path, mode, access, share) {
            this.OnClosed = onClosed;
        }

        public override void Close() {
            OnClosed(this);
            base.Close();
        }

        ~ActionFileStream() {
            OnClosed(this);
            base.Dispose();
        }

        
    }
}
