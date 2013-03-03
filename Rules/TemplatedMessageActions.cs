using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DarkSky.Messaging.Extensions;
using DarkSky.Messaging.Models;
using DarkSky.Messaging.Services;
using Orchard.Environment.Extensions;
using Orchard.Messaging.Services;
using Orchard.Localization;
using Orchard.Rules.Models;
using Orchard.Rules.Services;

namespace DarkSky.Messaging.Rules {
    [OrchardFeature("DarkSky.Messaging.Rules")]
    public class TemplatedMessageActions : IActionProvider {
        private readonly IMessageManager _messageManager;
        private readonly IMessageTemplateService _messageTemplateService;
        public const string MessageType = "ActionTemplatedMessage";

        public TemplatedMessageActions(
            IMessageManager messageManager, IMessageTemplateService messageTemplateService) {
            _messageManager = messageManager;
            _messageTemplateService = messageTemplateService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeActionContext describe) {
            
            describe.For("Messaging", T("Messaging"), T("Send Templated Messages"))
                .Element(
                    "SendTemplatedMessage", T("Send templated message"), T("Sends a templated message using a selected layout through the selected channel."), Send,
                    DislayAction, "ActionTemplatedMessage");
        }

        private LocalizedString DislayAction(ActionContext context) {
            var layoutId = context.Properties.GetValue("TemplateId").AsInt32();
            var channel = context.Properties.GetValue("Channel") ?? "unknown";
            var layout = layoutId != null ? _messageTemplateService.GetTemplate(layoutId.Value) : default(MessageTemplatePart);
            return T("Send a templated message using the [{0}] template and the [{1}] channel", layout != null ? layout.Title : "unkown", channel);
        }

        private bool Send(ActionContext context) {
            var channel = context.Properties.GetValue("Channel");
            var recipient = context.Properties.GetValue("Recipient");
            var dataTokens = ParseDataTokens(context.Properties);

            if(channel == null)
                throw new InvalidOperationException("No channel has been specified");

            if(recipient == null)
                throw new InvalidOperationException("No recipient has been specified");

            var properties = dataTokens.ToDictionary(x => x.Key, x => x.Value);

            foreach (var token in context.Tokens) {
                properties[token.Key] = token.Value.ToString();
            }

            properties["Recipient"] = recipient;
            properties["Channel"] = context.Properties.GetValue("Channel");
            properties["TemplateId"] = context.Properties.GetValue("TemplateId");

            _messageManager.Send(new[] {recipient}, MessageType, channel, properties);
            return true;
        }

        private static IEnumerable<KeyValuePair<string, string>> ParseDataTokens(IDictionary<string, string> source) {
            var dictionary = new Dictionary<string, string>();
            var text = source["DataTokens"];

            if (string.IsNullOrWhiteSpace(text))
                return dictionary;

            var pairs = Regex.Split(text, "\\n", RegexOptions.Multiline);
            foreach (var pair in pairs) {
                var items = pair.Split(new[] {':'}, 2);

                if (items.Length == 2) {
                    var key = items[0];
                    var value = items[1];

                    dictionary[key] = value;
                }
            }

            return dictionary;
        }
    }
}