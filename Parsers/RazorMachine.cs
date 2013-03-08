using Orchard;
using Orchard.Environment.Extensions;
using Xipton.Razor;

namespace DarkSky.Messaging.Parsers {
    public interface IRazorMachine : ISingletonDependency {
        ITemplate ExecuteContent(string templateContent, object model = null, object viewBag = null);
        void RegisterLayout(string virtualPath, string templateContent);
    }

    [OrchardFeature("DarkSky.Messaging.Parsers.Razor")]
    public class RazorMachineWrapper : IRazorMachine {
        private readonly RazorMachine _razorMachine;

        public RazorMachineWrapper() {
            _razorMachine = new RazorMachine();
        }

        public ITemplate ExecuteContent(string templateContent, object model = null, object viewBag = null) {
            return _razorMachine.ExecuteContent(templateContent, model, viewBag);
        }

        public void RegisterLayout(string virtualPath, string templateContent) {
            _razorMachine.RegisterTemplate(virtualPath, templateContent);
        }
    }
}