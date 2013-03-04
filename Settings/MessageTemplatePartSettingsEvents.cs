using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Environment.Extensions;

namespace DarkSky.Messaging.Settings {
    [OrchardFeature("DarkSky.Messaging")]
    public class MessageTemplatePartSettingsEvents : ContentDefinitionEditorEventsBase {
        
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition) {
            if (definition.PartDefinition.Name != "MessageTemplatePart")
                yield break;

            var model = definition.Settings.GetModel<MessageTemplatePartSettings>();
            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel) {
            if (builder.Name != "MessageTemplatePart")
                yield break;

            var model = new MessageTemplatePartSettings();

            if (updateModel.TryUpdateModel(model, "MessageTemplatePartSettings", null, null)) {
                builder.WithSetting("MessageTemplatePartSettings.DefaultParserId", model.DefaultParserId);

                yield return DefinitionTemplate(model);
            }
        }
    }
}