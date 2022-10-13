using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.Klaytnscope.Interfaces;
using Nomis.Klaytnscope.Interfaces.Settings;
using Nomis.Blockchain.Abstractions.Settings;
using Nomis.Utils.Extensions;

namespace Nomis.Klaytnscope.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Klaytnscope service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddKlaytnscopeService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSettings<KlaytnscopeSettings>(configuration);
            var settings = configuration.GetSettings<ApiVisibilitySettings>();
            if (settings.KlaytnAPIEnabled)
            {
                return services
                    .AddTransient<IKlaytnscopeClient, KlaytnscopeClient>()
                    .AddTransientInfrastructureService<IKlaytnscopeService, KlaytnscopeService>();
            }
            else
            {
                return services;
            }
        }
    }
}