using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DarkSky.Messaging.Models;

namespace DarkSky.Messaging.ViewModels {
    public class MessageTemplateViewModel {
        [Required, StringLength(256)]
        public string Title { get; set; }

        [Required, StringLength(256)]
        public string Subject { get; set; }
        public string Text { get; set; }

        [UIHint("TemplateLayoutPicker")]
        public int? LayoutId { get; set; }
        public bool IsLayout { get; set; }
        public IList<MessageTemplatePart> Layouts { get; set; }
    }
}