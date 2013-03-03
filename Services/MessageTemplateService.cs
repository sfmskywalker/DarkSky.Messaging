using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DarkSky.Messaging.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.UI.Notify;
using Xipton.Razor.Core;
using LogLevel = Orchard.Logging.LogLevel;

namespace DarkSky.Messaging.Services {
    public interface IMessageTemplateService : IDependency {
        IEnumerable<MessageTemplatePart> GetLayouts();
        IEnumerable<MessageTemplatePart> GetTemplates();
        IEnumerable<MessageTemplatePart> GetTemplatesWithLayout(int layoutId);
        MessageTemplatePart GetTemplate(int id);
        string ParseTemplate(MessageTemplatePart template, object model = null, object viewBag = null);
    }

    [OrchardFeature("DarkSky.Messaging")]
    public class MessageTemplateService : Component, IMessageTemplateService {
        private readonly IContentManager _contentManager;
        private readonly IRazorMachine _razorMachine;
        private readonly INotifier _notifier;

        public MessageTemplateService(IContentManager contentManager, IRazorMachine razorMachine, INotifier notifier) {
            _contentManager = contentManager;
            _razorMachine = razorMachine;
            _notifier = notifier;
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

        public string ParseTemplate(MessageTemplatePart templatePart, object model = null, object viewBag = null) {
            var layout = templatePart.Layout;
            var templateContent = templatePart.Text;

            if (layout != null) {
                _razorMachine.RegisterLayout("~/shared/_layout.cshtml", layout.Text);
                templateContent = "@{ Layout = \"_layout\"; }\r\n" + templateContent;
            }

            try {
                // Convert viewBag to string/object pairs if required
                if (viewBag != null) {
                    if (viewBag is IEnumerable<KeyValuePair<string, string>>)
                        viewBag = ((IEnumerable<KeyValuePair<string, string>>) viewBag).Select(x => new KeyValuePair<string, object>(x.Key, x.Value)).ToDictionary(x => x.Key, x => x.Value);
                }
                var template = _razorMachine.ExecuteContent(templateContent, model, viewBag);
                return template.Result;
            }
            catch (TemplateCompileException ex) {
                Logger.Log(LogLevel.Error, ex, "Failed to parse the {0} Razor template with layout {1}", templatePart.Title, layout != null ? layout.Title : "[none]");
                _notifier.Error(T("Failed to parse the {0} Razor template with layout {1}", templatePart.Title, layout != null ? layout.Title : "[none]"));
                var sb = new StringBuilder();
                var currentException = (Exception)ex;

                while (currentException != null) {
                    sb.AppendLine(currentException.Message);
                    currentException = currentException.InnerException;
                }

                sb.AppendFormat("\r\nTemplate ({0}):\r\n", templatePart.Title);
                sb.AppendLine(templatePart.Text);

                if (layout != null) {
                    sb.AppendFormat("\r\nLayout ({0}):\r\n", layout.Title);
                    sb.AppendLine(layout.Text);
                }
                return sb.ToString();
            }   
        }
    }
}