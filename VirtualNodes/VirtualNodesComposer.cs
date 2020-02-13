using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace VirtualNodes
{
    public class VirtualNodesComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<VirtualNodesComponent>();

            composition.ContentFinders().Insert<VirtualNodesContentFinder>();

            composition.UrlProviders().Insert<VirtualNodesUrlProvider>();
        }
    }
}