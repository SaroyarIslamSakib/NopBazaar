using FluentValidation;
using Nop.Plugin.Widgets.ManufacturerShowcase.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.ManufacturerShowcase.Validators;

public class ConfigurationValidator : BaseNopValidator<ConfigurationModel>
{
    #region Ctor

    public ConfigurationValidator(ILocalizationService localizationService)
    {
        RuleFor(model => model.DisplayNumber)
            .GreaterThan(0)
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.ManufacturerShowcase.DisplayNumber.Required"));
    }

    #endregion
}