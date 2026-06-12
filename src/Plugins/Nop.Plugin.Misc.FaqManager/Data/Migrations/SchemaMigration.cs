using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.FaqManager.Domain;

namespace Nop.Plugin.Misc.FaqManager.Data.Migrations;

[NopMigration("2026-06-12 00:00:00", "Misc.FaqManager schema", MigrationProcessType.Installation)]
public class SchemaMigration : Migration
{
    public override void Up()
    {
        this.CreateTableIfNotExists<FaqGroup>();
        this.CreateTableIfNotExists<FaqItem>();
    }

    public override void Down()
    {
        this.DeleteTableIfExists<FaqItem>();
        this.DeleteTableIfExists<FaqGroup>();
    }
}