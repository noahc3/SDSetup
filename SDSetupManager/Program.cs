using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;

namespace SDSetupManager {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddAuthentication
            //builder.Services.AddOidcAuthentication(options => {
            //    // Configure your authentication provider options here.
            //    // For more information, see https://aka.ms/blazor-standalone-auth
            //    options.ProviderOptions.Authority = "https://login.microsoftonline.com/";
            //    options.ProviderOptions.ClientId = "33333333-3333-3333-33333333333333333";
            //});

            await builder.Build().RunAsync();
        }
    }
}
