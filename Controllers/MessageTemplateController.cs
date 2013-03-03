using System;
using System.Web.Mvc;
using DarkSky.Messaging.Services;
using Orchard.Environment.Extensions;
using Orchard.UI.Admin;

namespace DarkSky.Messaging.Controllers {
    [Admin, OrchardFeature("DarkSky.Messaging")]
    public class MessageTemplateController : Controller {
        private readonly IMessageTemplateService _messageTemplateService;

        public MessageTemplateController(IMessageTemplateService messageTemplateService) {
            _messageTemplateService = messageTemplateService;
        }

        public string LayoutContent(int id) {
            var template = _messageTemplateService.GetTemplate(id);

            if(!template.IsLayout)
                throw new InvalidOperationException("That is not a Layout");

            return template.Text;
        }
    }
}