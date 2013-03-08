using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DarkSky.Messaging.Models;
using DarkSky.Messaging.Services;
using Orchard.Environment.Extensions;
using Orchard.Logging;
using Xipton.Razor.Core;

namespace DarkSky.Messaging.Parsers {
    [OrchardFeature("DarkSky.Messaging.Parsers.Razor")]
    public class RazorParserEngine : ParserEngineBase {
        private readonly IRazorMachine _razorMachine;

        public RazorParserEngine(IRazorMachine razorMachine) {
            _razorMachine = razorMachine;
        }

        public override string DisplayText {
            get { return "Razor Engine"; }
        }

        public override string LayoutBeacon {
            get { return "@RenderBody()"; }
        }

        public override string ParseTemplate(MessageTemplatePart template, ParseTemplateContext context) {
            var layout = template.Layout;
            var templateContent = template.Text;
            var viewBag = context.ViewBag;

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
                var tmpl = _razorMachine.ExecuteContent(templateContent, context.Model, viewBag);
                return tmpl.Result;
            }
            catch (TemplateCompileException ex) {
                Logger.Log(LogLevel.Error, ex, "Failed to parse the {0} Razor template with layout {1}", template.Title, layout != null ? layout.Title : "[none]");
                return BuildErrorContent(ex, template, layout);
            }   
        }

        private static string BuildErrorContent(Exception ex, MessageTemplatePart templatePart, MessageTemplatePart layout) {
            var sb = new StringBuilder();
            var currentException = ex;

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