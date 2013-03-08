using System.Web;
using DarkSky.Messaging.Rules;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Messaging.Events;
using Orchard.Messaging.Models;
using Orchard.Tokens;

namespace DarkSky.Messaging.Services {
    [OrchardFeature("DarkSky.Messaging")]
    public class TemplatedMessageHandler : IMessageEventHandler {
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly ITokenizer _tokenizer;
        private readonly IOrchardServices _services;

        public TemplatedMessageHandler(IMessageTemplateService messageTemplateService, ITokenizer tokenizer, IOrchardServices services) {
            _messageTemplateService = messageTemplateService;
            _tokenizer = tokenizer;
            _services = services;
        }

        public void Sending(MessageContext context) {
            if (context.MessagePrepared || context.Type != TemplatedMessageActions.MessageType)
                return;

            var templateId = int.Parse(context.Properties["TemplateId"]);
            var template = _messageTemplateService.GetTemplate(templateId);
            var body = _messageTemplateService.ParseTemplate(template, new ParseTemplateContext {
                ViewBag = context.Properties
            });

            context.MailMessage.Subject = _tokenizer.Replace(template.Subject, context.Properties, ReplaceOptions.Default);
            context.MailMessage.Body = body;
            context.MessagePrepared = true;
            context.Properties["BaseUrl"] = VirtualPathUtility.RemoveTrailingSlash(_services.WorkContext.CurrentSite.BaseUrl);
        }

        public void Sent(MessageContext context) {
        }
    }
}