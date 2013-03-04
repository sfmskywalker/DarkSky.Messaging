using System.Linq;
using System.Xml;
using DarkSky.Messaging.Extensions;
using DarkSky.Messaging.Models;
using DarkSky.Messaging.Services;
using DarkSky.Messaging.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace DarkSky.Messaging.Drivers {
    [OrchardFeature("DarkSky.Messaging")]
    public class MessageTemplatePartDriver : ContentPartDriver<MessageTemplatePart> {
        private readonly IContentManager _contentManager;
        private readonly IMessageTemplateService _messageTemplateService;

        public MessageTemplatePartDriver(IContentManager contentManager, IMessageTemplateService messageTemplateService) {
            _contentManager = contentManager;
            _messageTemplateService = messageTemplateService;
        }

        protected override string Prefix {
            get { return "MessageTemplate"; }
        }

        protected override DriverResult Editor(MessageTemplatePart part, dynamic shapeHelper) {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(MessageTemplatePart part, IUpdateModel updater, dynamic shapeHelper) {
            var viewModel = new MessageTemplateViewModel {
                ExpectedParser = _messageTemplateService.SelectParser(part),
                Layouts = _messageTemplateService.GetLayouts().Where(x => x.Id != part.Id).ToList()
            };

            if (updater != null) {
                if (updater.TryUpdateModel(viewModel, Prefix, null, null)) {
                    part.Title = viewModel.Title.TrimSafe();
                    part.Subject = viewModel.Subject.TrimSafe();
                    part.Text = viewModel.Text;
                    part.Layout = viewModel.LayoutId != null ? _contentManager.Get<MessageTemplatePart>(viewModel.LayoutId.Value) : null;
                    part.IsLayout = viewModel.IsLayout;
                }
            }
            else {
                viewModel.Title = part.Title;
                viewModel.Subject = part.Subject;
                viewModel.Text = part.Text;
                viewModel.LayoutId = part.Record.LayoutId;
                viewModel.IsLayout = part.IsLayout;
            }

            return ContentShape("Parts_MessageTemplate_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/MessageTemplate", Model: viewModel, Prefix: Prefix));
        }

        protected override void Importing(MessageTemplatePart part, ImportContentContext context) {
            context.ImportAttribute(part.PartDefinition.Name, "Title", x => part.Title = x);
            context.ImportAttribute(part.PartDefinition.Name, "Subject", x => part.Subject = x);
            context.ImportAttribute(part.PartDefinition.Name, "Text", x => part.Text = x);
            context.ImportAttribute(part.PartDefinition.Name, "IsLayout", x => part.IsLayout = XmlConvert.ToBoolean(x));
            context.ImportAttribute(part.PartDefinition.Name, "Layout", x => {
                var layout = context.GetItemFromSession(x);

                if (layout != null && layout.Is<MessageTemplatePart>()) {
                    part.Layout = layout.As<MessageTemplatePart>();
                }
            });
        }

        protected override void Exporting(MessageTemplatePart part, ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Title", part.Title);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Subject", part.Subject);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Text", part.Text);
            context.Element(part.PartDefinition.Name).SetAttributeValue("IsLayout", part.IsLayout);

            if(part.Layout != null)
                context.Element(part.PartDefinition.Name).SetAttributeValue("Layout", context.ContentManager.GetItemMetadata(part.Layout).Identity.ToString());
        }
    }
}