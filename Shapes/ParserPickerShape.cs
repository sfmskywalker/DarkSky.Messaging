using System.Linq;
using DarkSky.Messaging.Services;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment;
using Orchard.Environment.Extensions;

namespace DarkSky.Messaging.Shapes {
	[OrchardFeature("DarkSky.Messaging")]
	public class ParserPickerShape : IShapeTableProvider {
		private readonly Work<IMessageTemplateService> _messageTemplateService;

		public ParserPickerShape(Work<IMessageTemplateService> messageTemplateService) {
			_messageTemplateService = messageTemplateService;
		}

		public void Discover(ShapeTableBuilder builder) {
			builder.Describe("ParserPicker").OnDisplaying(context => {
				context.Shape.Parsers = _messageTemplateService.Value.GetParserDescriptors().ToList();
			});
		}
	}
}