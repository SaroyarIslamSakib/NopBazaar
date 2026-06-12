using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.FaqManager.Domain;

namespace Nop.Plugin.Misc.FaqManager.Data.Mapping.Builders;

public class FaqGroupBuilder : NopEntityBuilder<FaqGroup>
{
    #region Methods

    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(FaqGroup.Name)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(FaqGroup.ProductId)).AsInt32().ForeignKey<Product>();
    }

    #endregion
}