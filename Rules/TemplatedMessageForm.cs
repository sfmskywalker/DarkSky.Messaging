using System;
using System.Linq;
using System.Web.Mvc;
using DarkSky.Messaging.Services;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Forms.Services;
using Orchard.Localization;
using Orchard.Messaging.Services;

namespace DarkSky.Messaging.Rules {
    [OrchardFeature("DarkSky.Messaging.Rules")]
    public class TemplatedMessageForm : IFormProvider {
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageManager _messageManager;
        protected dynamic Shape { get; set; }
        public Localizer T { get; set; }

        public TemplatedMessageForm(IShapeFactory shapeFactory, IMessageTemplateService messageTemplateService, IMessageManager messageManager) {
            Shape = shapeFactory;
            _messageTemplateService = messageTemplateService;
            _messageManager = messageManager;
            T = NullLocalizer.Instance;
        }

        public void Describe(DescribeContext context) {
            Func<IShapeFactory, dynamic> form =
                shape => Shape.Form(
                Id: "ActionTemplatedMessage",
                _Recipient: Shape.TextBox(
                    Id: "Recipient", Name: "Recipient",
                    Title: T("Recipient"),
                    Description: "The recipient's address"),
                    Classes: new[] { "textMedium", "tokenized" },
                _TemplateId: Shape.SelectList(
                    Id: "TemplateId", Name: "TemplateId",
                    Title: T("Template"),
                    Description: T("The template of the e-mail."),
                    Items: _messageTemplateService.GetTemplates().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString()})
                    ),
                _Channel: Shape.SelectList(
                    Id: "Channel", Name: "Channel",
                    Title: T("Channel"),
                    Description: T("The channel through which to send the message."),
                    Items: _messageManager.GetAvailableChannelServices().Select(x => new SelectListItem { Text = x, Value = x })
                    ),
                _DataTokens: Shape.TextArea(
                    Id: "DataTokens", Name: "DataTokens",
                    Title: T("Data Tokens"),
                    Description: T("Enter a key:value pair per line. Each key will become available as a property on the ViewBag of the Razor template. E.g. \"Order:{Order.OrderNumber}\" will create a \"Order\" property on the ViewBag"),
                    Classes: new[] { "textMedium" }
                    )
                );

            context.Form("ActionTemplatedMessage", form);
        }
    }

    [OrchardFeature("DarkSky.Messaging.Rules")]
    public class TemplatedMessageFormValidator : Component, IFormEventHandler {
        public void Validating(ValidatingContext context) {}
        public void Validated(ValidatingContext context) {}
        public void Building(BuildingContext context) {}
        public void Built(BuildingContext context) {}
    }
}