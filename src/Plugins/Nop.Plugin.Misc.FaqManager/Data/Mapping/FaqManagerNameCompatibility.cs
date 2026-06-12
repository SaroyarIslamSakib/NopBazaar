using Nop.Data.Mapping;
using Nop.Plugin.Misc.FaqManager.Domain;

namespace Nop.Plugin.Misc.FaqManager.Data.Mapping;

public class FaqManagerNameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new() { [typeof(FaqGroup)] = "FaqGroup" };
    public Dictionary<(Type, string), string> ColumnName => [];
}