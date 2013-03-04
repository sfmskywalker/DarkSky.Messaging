using System.ComponentModel.DataAnnotations;

namespace DarkSky.Messaging.Settings {

    public class MessageTemplatePartSettings {
        [UIHint("ParserPicker")]
        public string DefaultParserId { get; set; }
    }
}
