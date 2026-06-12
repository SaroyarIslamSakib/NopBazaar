using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.FaqManager.Domain;

namespace Nop.Plugin.Misc.FaqManager.Data.Mapping.Builders;

public class FaqItemBuilder : NopEntityBuilder<FaqItem>
{
    #region Methods

    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(FaqItem.Question)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(FaqItem.Answer)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(FaqItem.FaqGroupId)).AsInt32().ForeignKey<FaqGroup>();
    }

    #endregion
}