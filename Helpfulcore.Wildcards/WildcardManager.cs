using Sitecore.Configuration;

namespace Helpfulcore.Wildcards
{
    public class WildcardManager
    {
        private static readonly ProviderHelper<RouteResolver, RouteResolverCollection> Configuration;

		static WildcardManager()
        {
            Configuration = new ProviderHelper<RouteResolver, RouteResolverCollection>("wildcardManager");
        }

		public static RouteResolver Current
        {
            get { return Configuration.Provider; }
        }

		public static RouteResolverCollection Providers
        {
            get { return Configuration.Providers; }
        }
    }
}
