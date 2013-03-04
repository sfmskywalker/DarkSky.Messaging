using DarkSky.Messaging.Models;
using Orchard;

namespace DarkSky.Messaging.Services {
	public interface IParserEngine : IDependency {
		string Id { get; }
		string DisplayText { get; }
		string LayoutBeacon { get; }
		string ParseTemplate(ParseTemplateContext context);
		ParserDescriptor Describe();
	}

	public class ParseTemplateContext {
		public MessageTemplatePart Template { get; set; }
		public object Model { get; set; }
		public object ViewBag { get; set; }
	}
}