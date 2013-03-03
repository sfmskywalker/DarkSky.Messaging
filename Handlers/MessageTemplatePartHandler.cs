using DarkSky.Messaging.Models;
using DarkSky.Messaging.Services;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;

namespace DarkSky.Messaging.Handlers {
    [OrchardFeature("DarkSky.Messaging")]
    public class MessageTemplatePartHandler : ContentHandler {
        private readonly IMessageTemplateService _messageTemplateService;

        public MessageTemplatePartHandler(IRepository<MessageTemplatePartRecord> repository, IMessageTemplateService messageTemplateService) {
            _messageTemplateService = messageTemplateService;
            Filters.Add(StorageFilter.For(repository));
            OnActivated<MessageTemplatePart>(PropertyHandlers);
        }

        private void PropertyHandlers(ActivatedContentContext context, MessageTemplatePart part) {
            part.LayoutField.Loader(x => part.Record.LayoutId != null ? _messageTemplateService.GetTemplate(part.Record.LayoutId.Value) : null);
            part.LayoutField.Setter(x => { part.Record.LayoutId = x != null ? x.Id : default(int?); return x; });
        }
    }
}