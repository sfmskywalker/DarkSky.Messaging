using System.Collections.Generic;
using System.Linq;
using System.Text;
using DarkSky.Messaging.Models;
using DarkSky.Messaging.Services;

namespace DarkSky.Messaging.Parsers {
	public class SimpleTextParserEngine : ParserEngineBase {
		public override string DisplayText {
			get { return "Simple Text Parser"; }
		}

		public override string LayoutBeacon {
			get { return "[Body]"; }
		}

		public override string ParseTemplate(MessageTemplatePart template, ParseTemplateContext context) {
			var layout = template.Layout;
			var templateContent = new StringBuilder(template.Text);
			var viewBag = context.ViewBag;

			if (layout != null) {
				templateContent = new StringBuilder(layout.Text.Replace(LayoutBeacon, templateContent.ToString()));
			}

			if (viewBag != null) {
				var variables = viewBag as IEnumerable<KeyValuePair<string, string>>;
				if (variables != null) {
					templateContent = variables.Aggregate(templateContent, (current, variable) => current.Replace(string.Format("[{0}]", variable.Key), variable.Value));
				}
			}

			return templateContent.ToString();
		}
	}
}