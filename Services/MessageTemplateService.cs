using System.Collections.Generic;
using System.Linq;
using DarkSky.Messaging.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace DarkSky.Messaging.Services {
    public interface IMessageTemplateService : IDependency {
        IEnumerable<MessageTemplatePart> GetLayouts();
        IEnumerable<MessageTemplatePart> GetTemplates();
        IEnumerable<MessageTemplatePart> GetTemplatesWithLayout(int layoutId);
        MessageTemplatePart GetTemplate(int id);
        string ParseTemplate(MessageTemplatePart template, ParseTemplateContext context);
        IEnumerable<IParserEngine> GetParsers();
        IParserEngine GetParser(string id);
        IParserEngine SelectParser(MessageTemplatePart template);
    }

    [OrchardFeature("DarkSky.Messaging")]
    public class MessageTemplateService : Component, IMessageTemplateService {
        private readonly IContentManager _contentManager;
        private readonly IEnumerable<IParserEngine> _parsers;
        private readonly IOrchardServices _services;

        public MessageTemplateService(IEnumerable<IParserEngine> parsers, IOrchardServices services) {
            _contentManager = services.ContentManager;
            _parsers = parsers;
            _services = services;
        }

        public IEnumerable<MessageTemplatePart> GetLayouts() {
            return _contentManager.Query<MessageTemplatePart, MessageTemplatePartRecord>().Where(x => x.IsLayout).List();
        }

        public IEnumerable<MessageTemplatePart> GetTemplates() {
            return _contentManager.Query<MessageTemplatePart, MessageTemplatePartRecord>().Where(x => !x.IsLayout).List();
        }

        public IEnumerable<MessageTemplatePart> GetTemplatesWithLayout(int layoutId) {
            return _contentManager.Query<MessageTemplatePart, MessageTemplatePartRecord>().Where(x => x.LayoutId == layoutId).List();
        }

        public MessageTemplatePart GetTemplate(int id) {
            return _contentManager.Get<MessageTemplatePart>(id);
        }

        public string ParseTemplate(MessageTemplatePart template, ParseTemplateContext context) {
            var parser = SelectParser(template);
            return parser.ParseTemplate(template, context);
        }

        public IParserEngine GetParser(string id) {
            return _parsers.SingleOrDefault(x => x.Id == id);
        }

        public IParserEngine SelectParser(MessageTemplatePart template) {
            var parserId = template.DefaultParserId;
            IParserEngine parser = null;

            if (!string.IsNullOrWhiteSpace(parserId)) {
                parser = GetParser(parserId);
            }

            if (parser == null) {
                parserId = _services.WorkContext.CurrentSite.As<MessagingSiteSettingsPart>().DefaultParserId;
                parser = GetParser(parserId);
            }

            return parser ?? _parsers.First();
        }

        public IEnumerable<IParserEngine> GetParsers() {
            return _parsers;
        }
    }
}