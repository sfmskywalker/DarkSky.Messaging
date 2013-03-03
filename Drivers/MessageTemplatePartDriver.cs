using System.Linq;
using DarkSky.Messaging.Extensions;
using DarkSky.Messaging.Models;
using DarkSky.Messaging.Services;
using DarkSky.Messaging.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
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
    }
}