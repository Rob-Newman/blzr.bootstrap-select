using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blzr.BootstrapSelect;

namespace WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBootstrapSelect();
            //builder.Services.AddBootstrapSelect(defaults =>
            //    {
            //        defaults.ShowSearch = true;
            //        defaults.SearchPlaceholderText = "Find";
            //        defaults.ShowSearchThreshold = 10;
            //        defaults.SearchNotFoundText = "Can't find any";
            //        defaults.DelayValueChangedCallUntilClose = true;
            //        defaults.SelectedTextFormat = SelectedTextFormats.CountGreaterThan;
            //        defaults.SelectedTextFormatCount = 2;
            //        defaults.MultiSelectedText = "{0} selected";
            //        defaults.ShowPlaceholder = true;
            //        defaults.MultiPlaceholderText = "Pick some";
            //        defaults.SinglePlaceholderText = "Pick one";
            //    });

            await builder.Build().RunAsync();
        }
    }
}
