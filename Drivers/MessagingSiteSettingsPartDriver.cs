using DarkSky.Messaging.Models;
using DarkSky.Messaging.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace DarkSky.Messaging.Drivers {
    [OrchardFeature("DarkSky.Messaging")]
    public class MessagingSiteSettingsPartDriver : ContentPartDriver<MessagingSiteSettingsPart> {
        
        protected override string Prefix {
            get { return "MessagingSiteSettings"; }
        }

        protected override DriverResult Editor(MessagingSiteSettingsPart part, dynamic shapeHelper) {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(MessagingSiteSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            var viewModel = new MessageTemplateSettingsViewModel();

            if (updater != null) {
                if (updater.TryUpdateModel(viewModel, Prefix, null, null)) {
                    part.DefaultParserId = viewModel.ParserId;
                }
            }
            else {
                viewModel.ParserId = part.DefaultParserId;
            }

            return ContentShape("Parts_MessagingSiteSettings_Edit", () => 
                shapeHelper.EditorTemplate(TemplateName: "Parts/MessagingSiteSettings", Model: viewModel, Prefix: Prefix))
                .OnGroup("Messaging");
        }

        protected override void Importing(MessagingSiteSettingsPart part, ImportContentContext context) {
            context.ImportAttribute(part.PartDefinition.Name, "DefaultParserEngine", x => part.DefaultParserId = x);
        }

        protected override void Exporting(MessagingSiteSettingsPart part, ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("DefaultParserEngine", part.DefaultParserId);
        }
    }
}