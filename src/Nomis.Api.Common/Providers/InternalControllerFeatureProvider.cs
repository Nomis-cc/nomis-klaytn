using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Nomis.Blockchain.Abstractions.Settings;

namespace Nomis.Api.Common.Providers
{
    /// <summary>
    /// Провайдер для проверки, является ли тип контроллером.
    /// </summary>
    public class InternalControllerFeatureProvider :
        ControllerFeatureProvider
    {
        /// <summary>
        /// Суффикс названия контроллера.
        /// </summary>
        private const string ControllerSuffix = "Controller";

        private readonly ApiVisibilitySettings _apiVisibilitySettings;

        /// <summary>
        /// Initialize <see cref="InternalControllerFeatureProvider"/>.
        /// </summary>
        /// <param name="apiVisibilitySettings"><see cref="ApiVisibilitySettings"/>.</param>
        public InternalControllerFeatureProvider(
            ApiVisibilitySettings apiVisibilitySettings)
        {
            _apiVisibilitySettings = apiVisibilitySettings;
        }

        /// <inheritdoc/>
        protected override bool IsController(TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
            {
                return false;
            }

            if (!typeInfo.Name.EndsWith(ControllerSuffix, StringComparison.OrdinalIgnoreCase) &&
                !typeInfo.IsDefined(typeof(ControllerAttribute)))
            {
                return false;
            }

            return SetVisibility(typeInfo.Name);
        }

        private bool SetVisibility(string controllerName)
        {
            switch (controllerName)
            {
                case "KlaytnController":
                    return _apiVisibilitySettings.KlaytnAPIEnabled;
                case nameof(DefaultController):
                    return true;
            }

            return false;
        }
    }
}