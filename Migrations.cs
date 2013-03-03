using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace DarkSky.Messaging {
    [OrchardFeature("DarkSky.Messaging")]
    public class Migrations : DataMigrationImpl {
         public int Create() {

             SchemaBuilder.CreateTable("MessageTemplatePartRecord", table => table
                 .ContentPartRecord()
                 .Column<string>("Title", c => c.WithLength(256))
                 .Column<string>("Subject", c => c.WithLength(256))
                 .Column<string>("Text", c => c.Unlimited())
                 .Column<int>("LayoutId", c => c.Nullable())
                 .Column<bool>("IsLayout", c => c.NotNull()));

             ContentDefinitionManager.AlterPartDefinition("MessageTemplatePart", part => part.Attachable());
             ContentDefinitionManager.AlterTypeDefinition("EmailTemplate", type => type
                 .WithPart("CommonPart", part => part
                    .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False"))
                 .WithPart("MessageTemplatePart")
                 .DisplayedAs("Email Template")
                 .Draftable()
                 .Creatable());

             return 1;
         }
    }
}