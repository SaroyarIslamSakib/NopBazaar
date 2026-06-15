using FluentValidation;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.FaqManager.Admin.Validators;

public class FaqGroupValidator : BaseNopValidator<FaqGroupModel>
{
    public FaqGroupValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.FaqManager.FaqGroups.Fields.Name.Required"));

        SetDatabaseValidationRules<FaqGroup>();
    }
}