using DarkSky.Messaging.Models;
using DarkSky.Messaging.Services;
using Orchard;

namespace DarkSky.Messaging.Parsers {
	public abstract class ParserEngineBase : Component, IParserEngine {
		public virtual string Id {
			get { return GetType().FullName; }
		}

		public virtual string DisplayText {
			get { return GetType().Name; }
		}

		public abstract string LayoutBeacon { get; }
		public abstract string ParseTemplate(MessageTemplatePart template, ParseTemplateContext context);
	}
}