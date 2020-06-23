using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SDSetupBackend.Data;
using MongoDB.Driver;
using GitLabApiClient.Models.Users.Responses;
using Microsoft.AspNetCore.Http;

namespace SDSetupBackend {
    public class Globals {
        public static string RootDirectory = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
    }
}
