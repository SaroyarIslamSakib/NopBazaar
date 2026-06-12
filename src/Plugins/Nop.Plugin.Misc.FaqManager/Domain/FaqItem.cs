using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Misc.FaqManager.Domain;

public class FaqItem : BaseEntity, ILocalizedEntity
{
    public int FaqGroupId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public bool Published { get; set; }
    public int DisplayOrder { get; set; }
}