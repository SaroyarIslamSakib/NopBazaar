using FluentValidation;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.FaqManager.Admin.Validators;

public class FaqItemValidator : BaseNopValidator<FaqItemModel>
{
    public FaqItemValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.FaqManager.FaqItems.Fields.Question.Required"));

        RuleFor(x => x.Answer)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.FaqManager.FaqItems.Fields.Answer.Required"));

        SetDatabaseValidationRules<FaqItem>();
    }
}
