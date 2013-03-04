using DarkSky.Messaging.Settings;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.Records;
using Orchard.ContentManagement.Utilities;
using Orchard.Data.Conventions;

namespace DarkSky.Messaging.Models {
    public class MessageTemplatePart : ContentPart<MessageTemplatePartRecord>, ITitleAspect {
        internal LazyField<MessageTemplatePart> LayoutField = new LazyField<MessageTemplatePart>();

        public string Title {
            get { return Record.Title; }
            set { Record.Title = value; }
        }

        public string Subject {
            get { return Record.Subject; }
            set { Record.Subject = value; }
        }

        public string Text {
            get { return Record.Text; }
            set { Record.Text = value; }
        }

        public bool IsLayout {
            get { return Record.IsLayout; }
            set { Record.IsLayout = value; }
        }

        public MessageTemplatePart Layout {
            get { return LayoutField.Value; }
            set { LayoutField.Value = value; }
        }

        public string DefaultParserId {
            get { return Settings.GetModel<MessageTemplatePartSettings>().DefaultParserId; }
        }
    }

    public class MessageTemplatePartRecord : ContentPartRecord {
        public virtual string Title { get; set; }
        public virtual string Subject { get; set; }

        [StringLengthMax]
        public virtual string Text { get; set; }
        public virtual int? LayoutId { get; set; }
        public virtual bool IsLayout { get; set; }
    }
}