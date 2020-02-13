using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using Umbraco.Core.Cache;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace VirtualNodes
{
    public class VirtualNodesContentFinder : IContentFinder
    {
        public bool TryFindContent(PublishedRequest contentRequest)
        {
            var runtimeCache = Current.AppCaches.RuntimeCache;
            var umbracoContext = contentRequest.UmbracoContext;
            var cachedVirtualNodeUrls = runtimeCache.GetCacheItem<Dictionary<string, int>>("CachedVirtualNodes");
            var path = contentRequest.Uri.AbsolutePath;

            // If found in the cached dictionary
            if (cachedVirtualNodeUrls?.ContainsKey(path) == true)
            {
                var nodeId = cachedVirtualNodeUrls[path];

                contentRequest.PublishedContent = umbracoContext.Content.GetById(nodeId);

                return true;
            }

            // If not found in the cached dictionary, traverse nodes and find the node that corresponds to the URL
            var rootNodes = umbracoContext.Content.GetAtRoot();
            var item = rootNodes.DescendantsOrSelf<IPublishedContent>()
                .FirstOrDefault(x => x.Url == path + "/" || x.Url == path);

            // If item is found, return it after adding it to the cache so we don't have to go through the same process again.
            if (cachedVirtualNodeUrls == null) cachedVirtualNodeUrls = new Dictionary<string, int>();

            // If we have found a node that corresponds to the URL given
            if (item != null)
            {
                // Update cache
                runtimeCache.InsertCacheItem("CachedVirtualNodes", () => cachedVirtualNodeUrls, null, false,
                    CacheItemPriority.High);

                // That's all folks
                contentRequest.PublishedContent = item;

                return true;
            }

            return false;
        }
    }
}