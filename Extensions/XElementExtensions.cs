using System.Xml.Linq;

namespace DarkSky.Messaging.Extensions {
    public static class XElementExtensions {
        public static string ElementValue(this XElement element, string childElementName) {
            var childElement = element.Element(childElementName);
            return childElement != null ? childElement.Value : null;
        }

        public static void ConditionallyAddElement(this XElement parent, string elementName, string value) {
            if(!string.IsNullOrWhiteSpace(value))
                parent.Add(new XElement(elementName, value));
        }
    }
}