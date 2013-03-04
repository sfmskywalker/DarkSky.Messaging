using System.ComponentModel.DataAnnotations;

namespace DarkSky.Messaging.ViewModels {
	public class MessageTemplateSettingsViewModel {
		[UIHint("ParserPicker")]
		public string ParserId { get; set; }
	}
}