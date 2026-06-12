using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Misc.FaqManager.Domain;

public class FaqGroup : BaseEntity, ILocalizedEntity
{
    public string Name { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public bool Published { get; set; }
    public int DisplayOrder { get; set; }
}