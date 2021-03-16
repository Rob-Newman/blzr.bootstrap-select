using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blzr.BootstrapSelect
{
    public static class Config
    {
        public static IServiceCollection AddBootstrapSelect(this IServiceCollection serviceCollection, Action<BootstrapSelectDefaults> defaultOptions = null)
        {
            // If options handler is not defined we will get an exception so
            // we need to initialize and empty action.
            if (defaultOptions == null)
            {
                defaultOptions = (e) => { };
            }

            serviceCollection.AddSingleton(defaultOptions);
            serviceCollection.AddSingleton<BootstrapSelectDefaults>();

            return serviceCollection;
        }
    }
}
