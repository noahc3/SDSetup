using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBlazor {
    public class CFW {
        public string name;
        public string id;
        public bool enabled;
        public CFWOption[] options;

        public CFW(string name, string id, bool enabled, CFWOption[] options) {
            this.name = name;
            this.id = id;
            this.enabled = enabled;
            this.options = options;
        }
    }

    public class CFWOption {
        public string name;
        public string id;
        public bool enabled;

        public CFWOption(string name, string id, bool enabled) {
            this.name = name;
            this.id = id;
            this.enabled = enabled;
        }
    }

    public class Package {
        public string id = "";
        public string name = "";
        public string authors = "";
        public string category = "";
        public string subcategory = "";
        public string version = "";
        public string source = "";
        public string dlSource = "";

        public bool enabledByDefault = false;
        public Artifact[] artifacts = null;

        public Package() {

        }

        public Package(string id, string name, string authors, string category, string subcategory, string version, string source, bool enabledByDefault, Artifact[] artifacts) {
            this.id = id;
            this.name = name;
            this.authors = authors;
            this.category = category;
            this.subcategory = subcategory;
            this.enabledByDefault = enabledByDefault;
            this.artifacts = artifacts;
            this.version = version;
            this.source = source;
        }

        public Package(string id, string name, string authors, string category, string subcategory, string version, string source, string dlSource, bool enabledByDefault, Artifact[] artifacts) {
            this.id = id;
            this.name = name;
            this.authors = authors;
            this.category = category;
            this.subcategory = subcategory;
            this.enabledByDefault = enabledByDefault;
            this.artifacts = artifacts;
            this.version = version;
            this.source = source;
            this.dlSource = dlSource;
        }
    }

    public class Artifact {
        public string url = "";
        public string dir = "/";
        public string filename = "unknown";
        public string diskLocation;

        public Artifact() {

        }

        public Artifact(string url, string dir, string filename) {
            this.url = url;
            this.dir = dir;
            this.filename = filename;
        }

        public Artifact(string url, string dir, string filename, string diskLocation) {
            this.url = url;
            this.dir = dir;
            this.filename = filename;
        }
    }

    public class JSArtifact {
        public string url = "";
        public string path = "";
        public string filename = "";

        public JSArtifact() {
            
        }

        public JSArtifact(string url, string path, string filename) {
            this.url = url;
            this.path = path;
            this.filename = filename;
        }
    }
    
}
