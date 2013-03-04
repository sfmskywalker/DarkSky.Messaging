using DarkSky.Messaging.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;

namespace DarkSky.Messaging.Handlers {
    [OrchardFeature("DarkSky.Messaging")]
    public class MessagingSiteSettingsPartHandler : ContentHandler {
        public MessagingSiteSettingsPartHandler(IRepository<MessagingSiteSettingsPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<MessagingSiteSettingsPart>("Site"));
            T = NullLocalizer.Instance;
            OnGetContentItemMetadata<MessagingSiteSettingsPart>((context, part) => context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Messaging"))));
        }

        public Localizer T { get; set; }
    }
}