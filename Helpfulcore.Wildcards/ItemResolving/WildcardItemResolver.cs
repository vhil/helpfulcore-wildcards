using Sitecore.Configuration;

namespace Helpfulcore.Wildcards.ItemResolving
{
    public class WildcardItemResolver
    {
        private static readonly ProviderHelper<ItemResolver, ItemResolverCollection> Configuration;

        static WildcardItemResolver()
        {
            Configuration = new ProviderHelper<ItemResolver, ItemResolverCollection>("wildcardItemResolver");
        }

        public static ItemResolver Current
        {
            get { return Configuration.Provider; }
        }

        public static ItemResolverCollection Providers
        {
            get { return Configuration.Providers; }
        }
    }
}
