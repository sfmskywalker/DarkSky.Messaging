using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace DarkSky.Messaging.Models {
    public class MessagingSiteSettingsPart : ContentPart<MessagingSiteSettingsPartRecord> {
        public string DefaultParserId {
            get { return Record.DefaultParserId; }
            set { Record.DefaultParserId = value; }
        }
    }

    public class MessagingSiteSettingsPartRecord : ContentPartRecord {
        public virtual string DefaultParserId { get; set; }
    }
}