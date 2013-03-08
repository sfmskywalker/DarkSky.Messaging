using DarkSky.Messaging.Models;
using Orchard;

namespace DarkSky.Messaging.Services {
	public interface IParserEngine : IDependency {
		string Id { get; }
		string DisplayText { get; }
		string LayoutBeacon { get; }
		string ParseTemplate(MessageTemplatePart template, ParseTemplateContext context);
	}

	public class ParseTemplateContext {
		public object Model { get; set; }
		public object ViewBag { get; set; }
	}
}